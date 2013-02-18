/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Assets;
using Frenzied.Input;
using Frenzied.Platforms;
using Frenzied.Screens.Scenes;
using Frenzied.Utils.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Frenzied.Screens.Implementations
{
    public class AboutScreen : GameScreen
    {
        private SpriteBatch _spriteBatch;
        private Viewport _viewport;

        private Texture2D _textureStudioLogo;
        private SpriteFont _spriteFont;

        private float _horizantalCenter;

        private string _creditsText;
        private float _creditsTextScale = 0.7f;
        private Vector2 _creditsTextSize;

        private float _scrollOffset;

        // game logo
        private Texture2D _textureGameLogo;
        private int _targetGameLogoWidth;
        private float _actualGameLogoScale;
        private float _pulsatedGameLogoScale;
        private Vector2 _gameLogoPosition;
        private const float PulsateFactor = 0.01f;

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


        readonly Random _random = new Random();

        // required services.       
        private IBackgroundScene _backgroundScene;

        public override void Initialize()
        {            
            // import required services.
            this._backgroundScene = ServiceHelper.GetService<IBackgroundScene>(typeof(IBackgroundScene)); 

            base.Initialize();
        }

        public override void LoadContent()
        {
            this._spriteBatch = ScreenManager.SpriteBatch;
            this._viewport = ScreenManager.GraphicsDevice.Viewport;
            
            this._horizantalCenter = this._viewport.Width/2;
            this._scrollOffset = this._viewport.Height - 200;

            this._textureStudioLogo = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Common/Logo");
            this._spriteFont = ScreenManager.Game.Content.Load<SpriteFont>(@"Fonts/GoodDog");

            this._creditsText =
                "            Developer: Huseyin Uslu\n"+
                "            Graphics: Lorem Ipsum\n"+
                "          Music & Audio: Lorem Ipsum\n\n" +
                "                     Special Thanks\n"+
                "                       Esra Uslu\n"+
                "                     Ibrahim Oztelli\n"+
                "                     Tolga Yavuzer\n\n" +
                "Copyright (C) 2012 - 2013, Int6 Studios\n"+
                "                  http://www.int6.org";
            this._creditsTextSize = this._spriteFont.MeasureString(this._creditsText);
            this._creditsTextSize.X *= _creditsTextScale;
            this._creditsTextSize.Y *= _creditsTextScale;

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

            // game logo stuff.
            this._textureGameLogo = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Common\game-logo");
            this._targetGameLogoWidth = this._viewport.Width / 2;
            this._actualGameLogoScale = (float)_targetGameLogoWidth / this._textureGameLogo.Width;
            this._gameLogoPosition = new Vector2(this._viewport.Width / 2 - _targetGameLogoWidth / 2, 25);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                ExitScreen();
            }

            if (input.CurrentMouseState.LeftButton != ButtonState.Pressed || input.LastMouseState.LeftButton != ButtonState.Released)
                return;

            ExitScreen();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if(this._scrollOffset>0)
                this._scrollOffset--;

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

            // Pulsate the game-logo.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            this._pulsatedGameLogoScale = this._actualGameLogoScale + pulsate * PulsateFactor;
            
            // update the background scene.
            this._backgroundScene.Update();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.CustomShadersEnabled)
                FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(sceneRenderTarget);

            this._spriteBatch.Begin();

            // draw background scene.
            this._backgroundScene.Draw(BackgroundScene.Season.Autumn);

            this._spriteBatch.Draw(this._textureGameLogo, this._gameLogoPosition, null, Color.White, 0f, Vector2.Zero,
                       this._pulsatedGameLogoScale, SpriteEffects.None, 0);

            var studioLogoPosition = new Vector2(this._horizantalCenter - this._textureStudioLogo.Width / 2, 200 + _scrollOffset);
            this._spriteBatch.Draw(this._textureStudioLogo, studioLogoPosition, Color.White);

            this._spriteBatch.End();

            if (PlatformManager.PlatformHandler.PlatformConfig.Graphics.CustomShadersEnabled)
            {
                FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(null);
                ApplyPostprocess();
            }

            this._spriteBatch.Begin();

            var creditsTextPosition = new Vector2(this._horizantalCenter - this._creditsTextSize.X / 2, studioLogoPosition.Y + _textureStudioLogo.Height + 25);
            _spriteBatch.DrawString(_spriteFont, this._creditsText, creditsTextPosition, Color.Black, 0f, Vector2.Zero,_creditsTextScale,SpriteEffects.None, 0 );

            this._spriteBatch.End();
        }

        /// <summary>
        /// Helper applies the edge detection and pencil sketch postprocess effect.
        /// </summary>
        private void ApplyPostprocess()
        {
            EffectParameterCollection parameters = _postprocessEffect.Parameters;
            string effectTechniqueName;

            // Set effect parameters controlling the pencil sketch effect.
            parameters["SketchThreshold"].SetValue(SketchThreshold);
            parameters["SketchBrightness"].SetValue(SketchBrightness);
            parameters["SketchJitter"].SetValue(_sketchJitter);
            parameters["SketchTexture"].SetValue(_sketchTexture);

            effectTechniqueName = "ColorSketch";

            // Activate the appropriate effect technique.
            _postprocessEffect.CurrentTechnique = _postprocessEffect.Techniques[effectTechniqueName];

            // Draw a fullscreen sprite to apply the postprocessing effect.
            _spriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, _postprocessEffect);
            _spriteBatch.Draw(sceneRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        public override void UnloadContent()
        {
            this._textureStudioLogo = null;
            this._spriteFont = null;

            base.UnloadContent();
        }
    }
}
