/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Frenzied.Core.Input
{
    public class Cursor : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D cursorTex;
        private Vector2 cursorPos;

        public Cursor(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            this.Game.IsMouseVisible = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cursorTex = this.Game.Content.Load<Texture2D>(@"Textures/cursor");    

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            cursorPos = new Vector2(mouseState.X, mouseState.Y);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(cursorTex, cursorPos, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
