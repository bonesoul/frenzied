using System;
using System.Collections.Generic;
using Frenzied.Core.Assets;
using Frenzied.Core.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Core.GamePlay
{
    public enum BlockPosition
    {
        topleft,
        topright,
        bottomleft,
        bottomright
    }

    public enum Block
    {
        empty,
        orange,
        purple,
        green
    }

    public class BlockContainer : DrawableGameComponent
    {
        public static Vector2 Size = new Vector2(205, 205);

        public Vector2 Position { get; private set; }
        public Rectangle Bounds { get; private set; }

        private Dictionary<BlockPosition, Block> _blocks = new Dictionary<BlockPosition, Block>();  

        public BlockContainer(Game game, Vector2 position)
            : base(game)
        {
            this.Position = position;
            this.Bounds = new Rectangle((int)position.X, (int)position.Y, (int)position.X + (int)Size.X, (int)position.Y + (int)Size.Y);

            this._blocks[BlockPosition.topleft] =  Block.empty;
            this._blocks[BlockPosition.topright] = Block.empty;
            this._blocks[BlockPosition.topright] = Block.empty;
            this._blocks[BlockPosition.bottomright] = Block.empty;
        }

        public void AddBlock(BlockPosition position, Block block)
        {
            if (block == Block.empty) // users can't empty blocks!
                return;

            this._blocks[position] = block;
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
            ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.BlockContainerTexture,
                                                    new Rectangle((Int32)this.Position.X, (Int32)this.Position.Y, 205, 205),
                                                    Color.White);

            foreach (var pair in this._blocks)
            {
                Vector2 @pos = new Vector2();
                switch (pair.Key)
                {
                    case BlockPosition.topleft:
                        @pos = this.Position;
                        break;
                    case BlockPosition.topright:
                        @pos = new Vector2(this.Position.X + 50, this.Position.Y);
                        break;
                    case BlockPosition.bottomleft:
                        @pos=new Vector2(this.Position.X,this.Position.Y);
                        break;
                    case BlockPosition.bottomright:
                        @pos = new Vector2(this.Position.X + 50, this.Position.Y + 50);
                        break;
                    default:
                        break;
                }

                var bounds = new Rectangle((int)@pos.X, (int)@pos.Y, 50, 50);

                if(pair.Value==Block.empty)
                    continue;

                var texture = AssetManager.Instance.GetBlockTexture(pair.Value);
                ScreenManager.Instance.SpriteBatch.Draw(texture, bounds, Color.White);
            }

            ScreenManager.Instance.SpriteBatch.End();   

            base.Draw(gameTime);
        }
    }
}
