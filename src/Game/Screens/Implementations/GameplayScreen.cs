/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Frenzied.Assets;
using Frenzied.GamePlay;
using Frenzied.GamePlay.Modes;
using Frenzied.Graphics.Effects;
using Frenzied.Input;
using Frenzied.Platforms;
using Frenzied.Screens.Scenes;
using Frenzied.Utils.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Frenzied.Screens.Implementations
{
    class GameplayScreen : GameScreen
    {
        float pauseAlpha;

        // required services.
        private IBackgroundScene _backgroundScene;

        readonly Random _random = new Random();

        // Effect used to apply the edge detection and pencil sketch postprocessing.
        Effect _postprocessEffect;

        // Overlay texture containing the pencil sketch stroke pattern.
        Texture2D _sketchTexture;

        // Randomly offsets the sketch pattern to create a hand-drawn animation effect.
        Vector2 _sketchJitter;
        TimeSpan _timeToNextJitter;

        private const float SketchThreshold = 0.2f;
        private const float SketchBrightness = 0.35f;
        private const float SketchJitterSpeed = 0.09f;

        // Custom rendertargets.
        RenderTarget2D sceneRenderTarget;


        private GameMode _gameMode;

        /// <summary>
        /// Creates a new <see cref="GameplayScreen"/> instance.
        /// </summary>
        public GameplayScreen(GameMode gameMode)
        {
            this._gameMode = gameMode;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Initialize()
        {
            // import required services.
            this._backgroundScene = ServiceHelper.GetService<IBackgroundScene>(typeof(IBackgroundScene));

            // initialize game-mode.
            this._gameMode.Initialize();

            // set stuff.
            this.EnabledGestures = GestureType.Tap;

            base.Initialize();
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.CustomShadersEnabled)
            {
                _postprocessEffect = AssetManager.Instance.LoadEffectShader(@"Effects\PostprocessEffect");
                _sketchTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Effects\SketchTexture");
            }

            // Create custom rendertarget.
            var presentationParameters = FrenziedGame.Instance.GraphicsDevice.PresentationParameters;
            sceneRenderTarget = new RenderTarget2D(FrenziedGame.Instance.GraphicsDevice,
                                                   presentationParameters.BackBufferWidth, presentationParameters.BackBufferHeight, false,
                                                   presentationParameters.BackBufferFormat, presentationParameters.DepthStencilFormat);


            this._gameMode.LoadContent();

            base.LoadContent();
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

            if (input.CurrentMouseState.LeftButton != ButtonState.Pressed || input.LastMouseState.LeftButton != ButtonState.Released)
                return;

            this._gameMode.HandleClick(gameTime, input.CurrentMouseState.X, input.CurrentMouseState.Y);
        }

        public override void HandleGestures(GameTime gameTime, InputState input)
        {
            if (input.Gestures.Count == 0)
                return;

            foreach (var gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                    this._gameMode.HandleClick(gameTime, (int)gesture.Position.X, (int)gesture.Position.Y);
            }
        }

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Update the sketch overlay texture jitter animation.
            if (SketchJitterSpeed > 0)
            {
                _timeToNextJitter -= gameTime.ElapsedGameTime;

                if (_timeToNextJitter <= TimeSpan.Zero)
                {
                    _sketchJitter.X = (float)_random.NextDouble();
                    _sketchJitter.Y = (float)_random.NextDouble();

                    _timeToNextJitter += TimeSpan.FromSeconds(SketchJitterSpeed);
                }
            }
            this._gameMode.Update(gameTime);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            // update the background scene.
            this._backgroundScene.Update();

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.CustomShadersEnabled)
                FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(sceneRenderTarget);

            this.ScreenManager.SpriteBatch.Begin();

            // draw background scene.
            this._backgroundScene.Draw(BackgroundScene.Season.Spring);

            this.ScreenManager.SpriteBatch.End();

            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.CustomShadersEnabled)
            {
                FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(null);
                ApplyPostprocess();
            }

            this._gameMode.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Helper applies the edge detection and pencil sketch postprocess effect.
        /// </summary>
        private void ApplyPostprocess()
        {
            EffectParameterCollection parameters = _postprocessEffect.Parameters;

            // Set effect parameters controlling the pencil sketch effect.
            parameters["SketchThreshold"].SetValue(SketchThreshold);
            parameters["SketchBrightness"].SetValue(SketchBrightness);
            parameters["SketchJitter"].SetValue(_sketchJitter);
            parameters["SketchTexture"].SetValue(_sketchTexture);

            // Activate the appropriate effect technique.
            _postprocessEffect.CurrentTechnique = _postprocessEffect.Techniques["ColorSketch"];

            // Draw a fullscreen sprite to apply the postprocessing effect.
            ScreenManager.Instance.SpriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, _postprocessEffect);
            ScreenManager.Instance.SpriteBatch.Draw(sceneRenderTarget, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();
        }
    }
}
