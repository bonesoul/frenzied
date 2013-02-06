/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.GamePlay.Modes;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.Implementations
{
    /// <summary>
    /// Blocked game mode.
    /// </summary>
    internal class BlockMode : GameMode
    {
        public BlockMode()
            : base()
        { }

        internal override void LoadContent()
        {
            var screenCenter = new Vector2(FrenziedGame.Instance.GraphicsDevice.Viewport.Bounds.Width/2,
                                           FrenziedGame.Instance.GraphicsDevice.Viewport.Bounds.Height/2);

            // add containers            
            this.ShapeContainers.Add(new BlockContainer(new Vector2(screenCenter.X - BlockContainer.Size.X / 2, screenCenter.Y - BlockContainer.Size.Y * 1.5f))); // top
            this.ShapeContainers.Add(new BlockContainer(new Vector2(screenCenter.X + BlockContainer.Size.X / 2, screenCenter.Y - BlockContainer.Size.Y / 2))); // right
            this.ShapeContainers.Add(new BlockContainer(new Vector2(screenCenter.X - BlockContainer.Size.X/2, screenCenter.Y + BlockContainer.Size.Y/2))); // bottom
            this.ShapeContainers.Add(new BlockContainer(new Vector2(screenCenter.X - BlockContainer.Size.X*1.5f, screenCenter.Y - BlockContainer.Size.Y/2))); // left

            // add generator
            this.ShapeGenerator = new BlockGenerator(new Vector2(screenCenter.X - BlockContainer.Size.X/2, screenCenter.Y - BlockContainer.Size.Y/2), this.ShapeContainers);

            base.LoadContent();
        }

        internal override void HandleClick(int X, int Y)
        {
            if (this.ShapeGenerator.IsEmpty())
                return;

            foreach (var container in this.ShapeContainers)
            {
                if (!container.Bounds.Contains(X, Y))
                    continue;

                if (!container.IsEmpty(this.ShapeGenerator.CurrentShape.LocationIndex))
                    continue;

                container[this.ShapeGenerator.CurrentShape.LocationIndex] = this.ShapeGenerator.CurrentShape;
                break;
            }
        }
    }
}
