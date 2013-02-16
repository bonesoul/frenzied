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

        private Texture2D _textureStudioLogo;
        private SpriteFont _spriteFont;

        private float _horizantalCenter;

        private string _creditsText;
        private float _creditsTextScale = 0.7f;
        private Vector2 _creditsTextSize;

        private float _scrollOffset;

        public AboutScreen()
            : base()
        {
        }

        public override void LoadContent()
        {
            this._spriteBatch = ScreenManager.SpriteBatch;
            this._viewport = ScreenManager.GraphicsDevice.Viewport;
            
            this._horizantalCenter = this._viewport.Width/2;
            this._scrollOffset = this._viewport.Height - 100;

            this._textureStudioLogo = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Common/Logo");
            this._spriteFont = ScreenManager.Game.Content.Load<SpriteFont>(@"Fonts/GoodDog");

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

            var studioLogoPosition = new Vector2(this._horizantalCenter - this._textureStudioLogo.Width/2, 100 + _scrollOffset);
            this._spriteBatch.Draw(this._textureStudioLogo, studioLogoPosition, Color.White);

            var creditsTextPosition = new Vector2(this._horizantalCenter - this._creditsTextSize.X / 2, studioLogoPosition.Y + _textureStudioLogo.Height + 25);
            _spriteBatch.DrawString(_spriteFont, this._creditsText, creditsTextPosition, Color.White,0f, Vector2.Zero,_creditsTextScale,SpriteEffects.None, 0 );

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
