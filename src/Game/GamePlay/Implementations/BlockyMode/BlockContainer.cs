/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Frenzied.Assets;
using Frenzied.GamePlay.Modes;
using Frenzied.Screens;
using Frenzied.Utils.Services;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.Implementations.BlockyMode
{
    internal class BlockContainer : ShapeContainer
    {
        public static Vector2 Size = new Vector2(210, 210);

        private IScoreManager _scoreManager;
        private IGameMode _gameMode;  

        public BlockContainer(Vector2 position)
            : base(position)
        { }

        public override void Initialize()
        {
            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Size.X, (int)Size.Y);

            this[BlockLocations.TopLeft] = Shape.Empty;
            this[BlockLocations.TopRight] = Shape.Empty;
            this[BlockLocations.BottomRight] = Shape.Empty;
            this[BlockLocations.BottomLeft] = Shape.Empty;

            // import required services.
            this._gameMode = ServiceHelper.GetService<IGameMode>(typeof(IGameMode));
            this._scoreManager = ServiceHelper.GetService<IScoreManager>(typeof(IScoreManager));
        }

        public override void Attach(Shape shape)
        {
            shape.Parent = this;

            if (shape.LocationIndex == ShapeLocations.None)
                return;

            switch (shape.LocationIndex)
            {
                case BlockLocations.TopLeft:
                    shape.Position = new Vector2(this.Position.X, this.Position.Y);
                    break;
                case BlockLocations.TopRight:
                    shape.Position = new Vector2(this.Position.X + shape.Size.X, this.Position.Y);
                    break;
                case BlockLocations.BottomRight:
                    shape.Position = new Vector2(this.Position.X + shape.Size.X, this.Position.Y + shape.Size.Y);
                    break;
                case BlockLocations.BottomLeft:
                    shape.Position = new Vector2(this.Position.X, this.Position.Y + shape.Size.Y);
                    break;
            }

            shape.Bounds = new Rectangle((int)shape.Position.X, (int)shape.Position.Y, (int)shape.Size.X, (int)shape.Size.Y);
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

        public override void Explode()
        {
            if (!this.IsFull())
                return;

            this._scoreManager.AddScore(this._gameMode.RuleSet.CalculateExplosionScore(this));

            this[BlockLocations.TopLeft] = Shape.Empty;
            this[BlockLocations.TopRight] = Shape.Empty;
            this[BlockLocations.BottomRight] = Shape.Empty;
            this[BlockLocations.BottomLeft] = Shape.Empty;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsFull())
                this.Explode();
        }

        public override void Draw(GameTime gameTime)
        {            
            ScreenManager.Instance.SpriteBatch.Begin();
            
            ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.BlockContainerTexture, this.Bounds, Color.White);

            foreach (var shape in this.GetEnumerator())
            {
                if (shape.IsEmpty)
                    continue;

                var block = ((BlockShape) shape);

                var texture = this._gameMode.GetShapeTexture(block);
                ScreenManager.Instance.SpriteBatch.Draw(texture, block.Bounds, Color.White);
            }

            ScreenManager.Instance.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
