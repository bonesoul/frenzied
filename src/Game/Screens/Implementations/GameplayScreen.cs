/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.GamePlay.Modes;
using Frenzied.Input;
using Frenzied.Platforms;
using Frenzied.PostProcessing.Effects;
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
        // common stuff.
        private Viewport _viewport;
        private GameMode _gameMode;
        private float pauseAlpha;

        // post-process effects
        private RenderTarget2D _scene; // the render-target that we draw the scene.
        private SketchEffect _sketchEffect; // the extended sketch post-process effect.
        private NoiseEffect _noiseEffect; // the replaced noise effect for WP7 which doesn't actually support custom-effects.

        // required services.
        private IBackgroundScene _backgroundScene;

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
            this._viewport = ScreenManager.GraphicsDevice.Viewport;

            // load contents for post-process effects
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.ExtendedEffects)
                this._sketchEffect = new SketchEffect(ScreenManager.Game, ScreenManager.SpriteBatch);
            else
                this._noiseEffect = new NoiseEffect(ScreenManager.Game, ScreenManager.SpriteBatch);

            // Create custom rendertarget for the scene.
            _scene = new RenderTarget2D(ScreenManager.GraphicsDevice, this._viewport.Width, this._viewport.Height);

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
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.ExtendedEffects)
                this._sketchEffect.UpdateJitter(gameTime);


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
            // create a render target for the scene which will be later using with post process effect.
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.ExtendedEffects)
                FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(_scene);

            this.ScreenManager.SpriteBatch.Begin();

            // draw background scene.
            this._backgroundScene.Draw(BackgroundScene.Season.Spring);

            this.ScreenManager.SpriteBatch.End();

            // apply post-process effect.
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.ExtendedEffects)
                this._sketchEffect.Apply(_scene);
            else
                this._noiseEffect.Apply(_scene);

            this._gameMode.Draw(gameTime);

            base.Draw(gameTime);
        }        
    }
}
