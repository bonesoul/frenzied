using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
