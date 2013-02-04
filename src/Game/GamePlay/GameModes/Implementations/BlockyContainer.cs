/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenzied.Assets;
using Frenzied.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay.GameModes.Implementations
{
    internal class BlockyContainer : ShapeContainer
    {
        public static Vector2 Size = new Vector2(210, 210);

        public BlockyContainer(Vector2 position)
            : base(position)
        {
            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Size.X, (int)Size.Y);

            this[BlockLocations.TopLeft] = new BlockShape(BlockColors.Blue, BlockLocations.TopLeft);
            this[BlockLocations.TopRight] = new BlockShape();
            this[BlockLocations.BottomRight] = new BlockShape();
            this[BlockLocations.BottomLeft] = new BlockShape();
        }

        public override IEnumerable<Shape> GetEnumerator()
        {
            yield return this[BlockLocations.TopLeft];
            yield return this[BlockLocations.TopRight];
            yield return this[BlockLocations.BottomRight];
            yield return this[BlockLocations.BottomLeft];
        }

        public override bool IsEmpty()
        {
            return this.IsEmpty(BlockLocations.TopLeft) && this.IsEmpty(BlockLocations.TopRight) &&
                   this.IsEmpty(BlockLocations.BottomLeft) && this.IsEmpty(BlockLocations.BottomRight);
        }

        public override bool IsEmpty(byte locationIndex)
        {
            return this[locationIndex].IsEmpty;
        }

        public override bool IsFull()
        {
            return !this.IsEmpty(BlockLocations.TopLeft) && !this.IsEmpty(BlockLocations.TopRight) &&
                   !this.IsEmpty(BlockLocations.BottomLeft) && !this.IsEmpty(BlockLocations.BottomRight);
        }

        public override void Draw(GameTime gameTime)
        {            
            ScreenManager.Instance.SpriteBatch.Begin();
            
            ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.BlockContainerTexture, this.Bounds, Color.White);

            foreach (var shape in this.GetEnumerator())
            {
                var block = ((BlockShape) shape);

                if (block.IsEmpty)
                    continue;

                var texture = GetBlockTexture(block);
                ScreenManager.Instance.SpriteBatch.Draw(texture, block.Bounds, Color.White);
            }

            ScreenManager.Instance.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public Texture2D GetBlockTexture(BlockShape block)
        {
            switch (block.ColorIndex)
            {
                case BlockColors.Orange:
                    return AssetManager.Instance.BlockTextures[Color.Orange];
                case BlockColors.Purple:
                    return AssetManager.Instance.BlockTextures[Color.Purple];
                case BlockColors.Green:
                    return AssetManager.Instance.BlockTextures[Color.Green];
                case BlockColors.Blue:
                    return AssetManager.Instance.BlockTextures[Color.Blue];
                default:
                    return null;
            }
        }
    }
}
