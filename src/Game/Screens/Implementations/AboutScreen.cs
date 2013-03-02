/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Input;
using Frenzied.Platforms;
using Frenzied.PostProcessing.Effects;
using Frenzied.Screens.Scenes;
using Frenzied.Utils.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Frenzied.Screens.Implementations
{
    public class AboutScreen : GameScreen
    {
        // common stuff.
        private SpriteBatch _spriteBatch;
        private Viewport _viewport;

        // post-process effects
        private RenderTarget2D _scene; // the render-target that we draw the scene.
        private SketchEffect _sketchEffect; // the extended sketch post-process effect.
        private NoiseEffect _noiseEffect; // the replaced noise effect for WP7 which doesn't actually support custom-effects.

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

            // game logo stuff.
            this._textureGameLogo = ScreenManager.Game.Content.Load<Texture2D>(@"Textures\Common\game-logo");
            this._targetGameLogoWidth = this._viewport.Width / 2;
            this._actualGameLogoScale = (float)_targetGameLogoWidth / this._textureGameLogo.Width;
            this._gameLogoPosition = new Vector2(this._viewport.Width / 2 - _targetGameLogoWidth / 2, 25);

            // load contents for post-process effects
            if (PlatformManager.Handler.Config.Graphics.PostprocessEnabled)
            {
                if (PlatformManager.Handler.Config.Graphics.ExtendedEffects)
                    this._sketchEffect = new SketchEffect(ScreenManager.Game, ScreenManager.SpriteBatch);
                else
                    this._noiseEffect = new NoiseEffect(ScreenManager.Game, ScreenManager.SpriteBatch);
            }

            // Create custom rendertarget for the scene.
            _scene = new RenderTarget2D(ScreenManager.GraphicsDevice, this._viewport.Width, this._viewport.Height);
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

            if (PlatformManager.Handler.Config.Graphics.PostprocessEnabled && PlatformManager.Handler.Config.Graphics.ExtendedEffects)
                this._sketchEffect.UpdateJitter(gameTime);

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
            // create a render target for the scene which will be later using with post process effect.
            if (PlatformManager.Handler.Config.Graphics.PostprocessEnabled)
                FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(_scene);

            this._spriteBatch.Begin();

            // draw background scene.
            this._backgroundScene.Draw(BackgroundScene.Season.Autumn);

            // draw game-logo.
            this._spriteBatch.Draw(this._textureGameLogo, this._gameLogoPosition, null, Color.White, 0f, Vector2.Zero,
                       this._pulsatedGameLogoScale, SpriteEffects.None, 0);

            // draw other stuff.
            var studioLogoPosition = new Vector2(this._horizantalCenter - this._textureStudioLogo.Width / 2, 200 + _scrollOffset);
            this._spriteBatch.Draw(this._textureStudioLogo, studioLogoPosition, Color.White);

            this._spriteBatch.End();

            if (PlatformManager.Handler.Config.Graphics.PostprocessEnabled)
            {
                // apply post-process effect.
                if (PlatformManager.Handler.Config.Graphics.ExtendedEffects)
                    this._sketchEffect.Apply(_scene);
                else
                    this._noiseEffect.Apply(_scene);
            }

            this._spriteBatch.Begin();

            var creditsTextPosition = new Vector2(this._horizantalCenter - this._creditsTextSize.X / 2, studioLogoPosition.Y + _textureStudioLogo.Height + 25);
            _spriteBatch.DrawString(_spriteFont, this._creditsText, creditsTextPosition, Color.Black, 0f, Vector2.Zero,_creditsTextScale,SpriteEffects.None, 0 );

            this._spriteBatch.End();
        }

        public override void UnloadContent()
        {
            this._textureStudioLogo = null;
            this._spriteFont = null;

            base.UnloadContent();
        }
    }
}
