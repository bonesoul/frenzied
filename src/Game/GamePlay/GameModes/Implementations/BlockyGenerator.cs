/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System.Collections.Generic;
using Frenzied.Assets;
using Frenzied.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay.GameModes.Implementations
{
    internal class BlockyGenerator:ShapeGenerator
    {
        public static Vector2 Size = new Vector2(210, 210);

        public BlockyGenerator(Vector2 position, List<ShapeContainer> containers)
            : base(position, containers)
        {
            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Size.X, (int)Size.Y);

            this.CurrentShape = new BlockShape();
        }

        public override bool IsEmpty()
        {
            return this.CurrentShape.IsEmpty;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsEmpty())
                this.Generate();
        }

        public override void Generate()
        {
            var color = Randomizer.Next(1, 5);
            var availableLocations = this.GetAvailableLocations();
            
            if (availableLocations.Count == 0)
                return;

            var locationIndex = Randomizer.Next(availableLocations.Count);
            var location = availableLocations[locationIndex];

            this.CurrentShape = new BlockShape((byte)location, (byte)color);
        }

        public override List<byte> GetAvailableLocations()
        {
            var availableLocations = new List<byte>();

            foreach (var container in this.Containers)
            {
                if (container.IsEmpty(BlockLocations.TopLeft) && !availableLocations.Contains(BlockLocations.TopLeft))
                    availableLocations.Add(BlockLocations.TopLeft);
                else if (container.IsEmpty(BlockLocations.TopRight) && !availableLocations.Contains(BlockLocations.TopRight))
                    availableLocations.Add(BlockLocations.TopRight);
                else if (container.IsEmpty(BlockLocations.BottomLeft) && !availableLocations.Contains(BlockLocations.BottomLeft))
                    availableLocations.Add(BlockLocations.BottomLeft);
                else if (container.IsEmpty(BlockLocations.BottomRight) && !availableLocations.Contains(BlockLocations.BottomRight))
                    availableLocations.Add(BlockLocations.BottomRight);
            }

            return availableLocations;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Instance.SpriteBatch.Begin();

            ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.BlockContainerTexture, this.Bounds, Color.White);

            var texture = GetBlockTexture((BlockShape) this.CurrentShape);
            ScreenManager.Instance.SpriteBatch.Draw(texture, this.CurrentShape.Bounds, Color.White);

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
