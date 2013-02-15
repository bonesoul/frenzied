/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.Assets;
using Frenzied.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Screens.Implementations
{
    public class AboutScreen : GameScreen
    {
        private SpriteBatch _spriteBatch;
        private Viewport _viewport;

        private Texture2D _studioLogoTexture;
        private SpriteFont _spriteFont;

        private float _horizantalCenter;

        private string _creditsText;
        private float _creditsTextScale = 0.7f;
        private Vector2 _creditsTextSize;

        private float _scrollOffset;

        public AboutScreen()
            : base()
        { }

        public override void LoadContent()
        {
            this._spriteBatch = ScreenManager.SpriteBatch;
            this._viewport = ScreenManager.GraphicsDevice.Viewport;
            
            this._horizantalCenter = this._viewport.Width/2;
            this._scrollOffset = this._viewport.Height - 100;

            this._studioLogoTexture = AssetManager.Instance.StudioLogo;
            this._spriteFont = AssetManager.Instance.GoodDog;

            this._creditsText =
                "Developer: Huseyin Uslu\nGraphics: Lorem Ipsum\nMusic & Audio: Dogac Yavuz" +
                "\n\nSpecial Thanks\nEsra Uslu\nIbrahim Oztelli\nTolga Yavuzer " +
                "\n\nCopyright (C) 2012 - 2013, Int6 Studios\nhttp://www.int6.org";
            this._creditsTextSize = this._spriteFont.MeasureString(this._creditsText);
            this._creditsTextSize.X *= _creditsTextScale;
            this._creditsTextSize.Y *= _creditsTextScale;
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                ExitScreen();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if(this._scrollOffset>0)
                this._scrollOffset--;

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            FrenziedGame.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            this._spriteBatch.Begin();

            var studioLogoPosition = new Vector2(this._horizantalCenter - this._studioLogoTexture.Width/2, 100 + _scrollOffset);
            this._spriteBatch.Draw(this._studioLogoTexture, studioLogoPosition, Color.White);

            var creditsTextPosition = new Vector2(this._horizantalCenter - this._creditsTextSize.X / 2, studioLogoPosition.Y + _studioLogoTexture.Height + 25);
            _spriteBatch.DrawString(_spriteFont, this._creditsText, creditsTextPosition, Color.White,0f, Vector2.Zero,_creditsTextScale,SpriteEffects.None, 0 );

            this._spriteBatch.End();
        }
    }
}
