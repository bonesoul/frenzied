/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Screens.Implementations
{
    // some karma building time!
    public class IntroScreen : GameScreen
    {
        // required resources.
        private SpriteBatch _spriteBatch;
        private Viewport _viewport;

        // textures.
        private Texture2D _blankTexture;
        private Texture2D _studioLogo;       

        // fade-in & fade out stuff.
        private float _fadeAlpha; // current fade alpha.
        private bool _fadingIn; // are we currently fading in? (true if yes, false if we are instead fading-out).
        private TimeSpan _fadeInTimer = TimeSpan.FromMilliseconds(1000); // fade-in timer.
        private TimeSpan _fadeOutTimer = TimeSpan.FromMilliseconds(1500); // fade-out timer.
        private Color _fadeOutColor = new Color(91, 113, 134);

        public override void Initialize()
        {
            this._fadingIn = true; // we should start by fading-in.

            base.Initialize();
        }

        public override void LoadContent()
        {
            this._viewport = FrenziedGame.Instance.GraphicsDevice.Viewport;
            this._spriteBatch = new SpriteBatch(FrenziedGame.Instance.GraphicsDevice);           

            // load assets.
            this._studioLogo = AssetManager.Instance.StudioLogo;
            this._blankTexture = FrenziedGame.Instance.Content.Load<Texture2D>(@"Textures\Blank");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (_fadingIn) // if we are currently fading-in.
            {
                _fadeInTimer -= gameTime.ElapsedGameTime; // process the timer.

                if (_fadeInTimer < TimeSpan.Zero) // if fading-in is done,
                    _fadingIn = false; // start fading-out now.
            }
            else // if we are currently fading-out.
            {
                _fadeOutTimer -= gameTime.ElapsedGameTime; // process the timer.

                if (_fadeOutTimer < TimeSpan.Zero) // if fading-out is done, we should now load the main menu screen.
                {
                    ScreenManager.Instance.AddScreen(new MainMenuScreen2(), null); // add menu-screen.
                    ScreenManager.Instance.RemoveScreen(this); // remove the intro screen.
                }
            }

            // calculate the current alpha value for fade.
            _fadeAlpha = _fadingIn
            ? (float)_fadeInTimer.TotalMilliseconds / 1000f
            : 1f - ((float)_fadeOutTimer.TotalMilliseconds / 1000f);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            // draw the studio logo.
            this._spriteBatch.Draw(this._studioLogo, this._viewport.Bounds, Color.White);

            // draw the fade-in & fade-out alpha.
            this._spriteBatch.Draw(_blankTexture, new Rectangle(0, 0, this._viewport.Width, this._viewport.Height),
                                   this._fadingIn ? Color.Black*_fadeAlpha : this._fadeOutColor*_fadeAlpha);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
