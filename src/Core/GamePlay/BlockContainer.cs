using System;
using Frenzied.Core.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Core.GamePlay
{
    public class BlockContainer : DrawableGameComponent
    {
        Texture2D _blockContainer;

        public static Vector2 Size = new Vector2(205, 205);

        public Vector2 Position { get; private set; }

        public BlockContainer(Game game, Vector2 position)
            : base(game)
        {
            this.Position = position;

            this._blockContainer = ScreenManager.Instance.Game.Content.Load<Texture2D>(@"Textures/BlockContainer");
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Instance.SpriteBatch.Begin();
            ScreenManager.Instance.SpriteBatch.Draw(_blockContainer,
                                                    new Rectangle((Int32)this.Position.X, (Int32)this.Position.Y, 205, 205),
                                                    Color.White);
            ScreenManager.Instance.SpriteBatch.End();   

            base.Draw(gameTime);
        }
    }
}
