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
    public class MainMenuScreen2 : GameScreen
    {
        private SpriteBatch _spriteBatch;

        private Texture2D _menuTextureCredits;
        private Texture2D _menuTextureCustomMode;
        private Texture2D _menuTextureOptions;
        private Texture2D _menuTextureQuickPlay;
        private Texture2D _menuTextureTutorial;

        public override void LoadContent()
        {
            this._spriteBatch = new SpriteBatch(FrenziedGame.Instance.GraphicsDevice);

            this._menuTextureCredits = AssetManager.Instance.MenuCredits;
            this._menuTextureCustomMode = AssetManager.Instance.MenuCustomMode;
            this._menuTextureOptions = AssetManager.Instance.MenuOptions;
            this._menuTextureQuickPlay = AssetManager.Instance.MenuQuickPlay;
            this._menuTextureTutorial = AssetManager.Instance.MenuTutorial;

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            FrenziedGame.Instance.GraphicsDevice.Clear(new Color(51, 51, 51));

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate*0.05f;

            this._spriteBatch.Begin();
            this._spriteBatch.Draw(this._menuTextureQuickPlay, new Vector2(100, 100), null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            this._spriteBatch.Draw(this._menuTextureCustomMode, new Vector2(100, 175), Color.White);
            this._spriteBatch.Draw(this._menuTextureTutorial, new Vector2(100, 250), Color.White);
            this._spriteBatch.Draw(this._menuTextureOptions, new Vector2(100, 325), Color.White);
            this._spriteBatch.Draw(this._menuTextureCredits, new Vector2(100, 400), Color.White);
            this._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
