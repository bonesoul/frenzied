/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Frenzied.Assets;
using Frenzied.Screens;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay
{
    public class BlockContainer : DrawableGameComponent
    {
        public static Vector2 Size = new Vector2(210, 210);

        private readonly Dictionary<BlockLocation, Block> _blockLocations = new Dictionary<BlockLocation, Block>();

        private IScoreManager _scoreManager;
        private IAssetManager _assetManager;

        public IEnumerable<Block> Blocks
        {
            get { return this._blockLocations.Values; }
        }

        public Vector2 Position { get; protected set; }
        public Rectangle Bounds { get; protected set; }

        public BlockContainer(Game game, Vector2 position)
            : base(game)
        {
            this.Position = position;
            this.Bounds = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);

            this._blockLocations[BlockLocation.topleft] = new Block(BlockLocation.none);
            this._blockLocations[BlockLocation.topright] = new Block(BlockLocation.none);
            this._blockLocations[BlockLocation.bottomleft] = new Block(BlockLocation.none);
            this._blockLocations[BlockLocation.bottomright] = new Block(BlockLocation.none);

            this._scoreManager = (IScoreManager)this.Game.Services.GetService(typeof(IScoreManager));
            this._assetManager = (IAssetManager) this.Game.Services.GetService(typeof (IAssetManager));

            if (this._scoreManager == null)
                throw new NullReferenceException("Can not find score manager component.");
        }

        public bool IsEmpty(BlockLocation position)
        {
            return this._blockLocations[position].IsEmpty;
        }

        public bool IsFull()
        {
            return !this.IsEmpty(BlockLocation.topleft) && !this.IsEmpty(BlockLocation.topright) && !this.IsEmpty(BlockLocation.bottomleft) && !this.IsEmpty(BlockLocation.bottomright);
        }

        private void Explode()
        {
            if (!this.IsFull())
                return;

            this._assetManager.Sounds.Explode.Play();

            this._scoreManager.CorrectMove(this);

            this._blockLocations[BlockLocation.topleft] = new Block(BlockLocation.none);
            this._blockLocations[BlockLocation.topright] = new Block(BlockLocation.none);
            this._blockLocations[BlockLocation.bottomleft] = new Block(BlockLocation.none);
            this._blockLocations[BlockLocation.bottomright] = new Block(BlockLocation.none);
        }


        public void AddBlock(Block block)
        {
            if (block.IsEmpty) // users can't empty blocks!
                return;

            if (!this._blockLocations[block.Location].IsEmpty)
                return;

            this._blockLocations[block.Location] = block;
            block.AttachTo(this);
        }

        public override void Update(GameTime gameTime)
        {
            if(this.IsFull())
                this.Explode();
        }

        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.Instance.SpriteBatch.Begin();
            //ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.BlockContainerTexture,
            //                                        this.Bounds,
            //                                        Color.White);

            //foreach (var pair in this._blockLocations)
            //{
            //    var block = pair.Value;

            //    if(block.IsEmpty)
            //        continue;

            //    var texture = AssetManager.Instance.GetBlockTexture(block);
            //    ScreenManager.Instance.SpriteBatch.Draw(texture, block.Bounds, Color.White);
            //}

            //ScreenManager.Instance.SpriteBatch.End();   

            base.Draw(gameTime);
        }
    }
}
