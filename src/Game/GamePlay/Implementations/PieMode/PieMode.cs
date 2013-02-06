/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.Assets;
using Frenzied.GamePlay.Implementations.BlockyMode;
using Frenzied.GamePlay.Modes;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.Implementations.PieMode
{
    public class PieMode : GameMode
    {
        public override void LoadContent()
        {
            var screenCenter = new Vector2(FrenziedGame.Instance.GraphicsDevice.Viewport.Bounds.Width / 2,
                               FrenziedGame.Instance.GraphicsDevice.Viewport.Bounds.Height / 2);

            // add containers            
            this.ShapeContainers.Add(new PieContainer(new Vector2(screenCenter.X - PieContainer.Size.X * 1.5f , screenCenter.Y - PieContainer.Size.Y * 1.5f))); // top-left
            this.ShapeContainers.Add(new PieContainer(new Vector2(screenCenter.X - PieContainer.Size.X / 2, screenCenter.Y - PieContainer.Size.Y * 1.5f))); // top-middle
            this.ShapeContainers.Add(new PieContainer(new Vector2(screenCenter.X + PieContainer.Size.X / 2 , screenCenter.Y - PieContainer.Size.Y * 1.5f))); // top-right
            this.ShapeContainers.Add(new PieContainer(new Vector2(screenCenter.X + PieContainer.Size.X / 2, screenCenter.Y + PieContainer.Size.Y / 2))); // bottom-right
            this.ShapeContainers.Add(new PieContainer(new Vector2(screenCenter.X - PieContainer.Size.X / 2, screenCenter.Y + PieContainer.Size.Y / 2))); // bottom-middle
            this.ShapeContainers.Add(new PieContainer(new Vector2(screenCenter.X - PieContainer.Size.X * 1.5f, screenCenter.Y + PieContainer.Size.Y / 2))); // bottom-right
            //this.ShapeContainers.Add(new PieContainer(new Vector2(screenCenter.X + PieContainer.Size.X / 2, screenCenter.Y - PieContainer.Size.Y / 2))); // right
            //this.ShapeContainers.Add(new PieContainer(new Vector2(screenCenter.X - PieContainer.Size.X * 1.5f, screenCenter.Y - PieContainer.Size.Y / 2))); // left                                    

            // add generator.
            this.ShapeGenerator = new PieGenerator(new Vector2(screenCenter.X - BlockContainer.Size.X / 2, screenCenter.Y - BlockContainer.Size.Y / 2), this.ShapeContainers);
        }

        public override Microsoft.Xna.Framework.Graphics.Texture2D GetShapeTexture(Shape shape)
        {
            switch (shape.ColorIndex)
            {
                case PieColors.Orange:
                    return AssetManager.Instance.PieTextures[Color.Orange];
                case PieColors.Purple:
                    return AssetManager.Instance.PieTextures[Color.Purple];
                case PieColors.Green:
                    return AssetManager.Instance.PieTextures[Color.Green];
                case PieColors.Blue:
                    return AssetManager.Instance.PieTextures[Color.Blue];
                default:
                    return null;
            }
        }

        public override void HandleClick(int X, int Y)
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
