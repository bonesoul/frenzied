using System;
using System.Collections.Generic;
using Frenzied.Core.Assets;
using Frenzied.Core.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Core.GamePlay
{
    public class BlockContainer : DrawableGameComponent
    {
        public static Vector2 Size = new Vector2(210, 210);

        private Dictionary<BlockLocation, Block> _blocks = new Dictionary<BlockLocation, Block>();

        public Vector2 Position { get; protected set; }
        public Rectangle Bounds { get; protected set; }

        public BlockContainer(Game game, Vector2 position)
            : base(game)
        {
            this.Position = position;
            this.Bounds = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);

            this._blocks[BlockLocation.topleft] = new Block(BlockLocation.none);
            this._blocks[BlockLocation.topright] = new Block(BlockLocation.none);
            this._blocks[BlockLocation.bottomleft] = new Block(BlockLocation.none);
            this._blocks[BlockLocation.bottomright] = new Block(BlockLocation.none);
        }

        public bool IsEmpty(BlockLocation position)
        {
            return this._blocks[position].IsEmpty;
        }

        public bool IsFull()
        {
            return !this.IsEmpty(BlockLocation.topleft) && !this.IsEmpty(BlockLocation.topright) && !this.IsEmpty(BlockLocation.bottomleft) && !this.IsEmpty(BlockLocation.bottomright);
        }

        private void Explode()
        {
            if (!this.IsFull())
                return;

            this._blocks[BlockLocation.topleft] = new Block(BlockLocation.none);
            this._blocks[BlockLocation.topright] = new Block(BlockLocation.none);
            this._blocks[BlockLocation.bottomleft] = new Block(BlockLocation.none);
            this._blocks[BlockLocation.bottomright] = new Block(BlockLocation.none);
        }


        public void AddBlock(Block block)
        {
            if (block.IsEmpty) // users can't empty blocks!
                return;

            if (!this._blocks[block.Location].IsEmpty)
                return;

            this._blocks[block.Location] = block;
            block.AttachTo(this);
        }

        public override void Update(GameTime gameTime)
        {
            if(this.IsFull())
                this.Explode();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Instance.SpriteBatch.Begin();
            ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.BlockContainerTexture,
                                                    this.Bounds,
                                                    Color.White);

            foreach (var pair in this._blocks)
            {
                var block = pair.Value;

                if(block.IsEmpty)
                    continue;

                var texture = AssetManager.Instance.GetBlockTexture(block);
                ScreenManager.Instance.SpriteBatch.Draw(texture, block.Bounds, Color.White);
            }

            ScreenManager.Instance.SpriteBatch.End();   

            base.Draw(gameTime);
        }
    }
}
