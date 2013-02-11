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
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay.Implementations.PieMode
{
    public class PieContainer : ShapeContainer
    {
        public static Vector2 Size = new Vector2(210, 210);

        private IScoreManager _scoreManager;
        private IGameMode _gameMode;       

        public PieContainer(Vector2 position)
            : base(position)
        { }

        public override void Initialize()
        {
            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Size.X, (int)Size.Y);

            this[PieLocations.TopLeft] = Shape.Empty;
            this[PieLocations.TopMiddle] = Shape.Empty;
            this[PieLocations.TopRight] = Shape.Empty;
            this[PieLocations.BottomRight] = Shape.Empty;
            this[PieLocations.BottomMiddle] = Shape.Empty;
            this[PieLocations.BottomLeft] = Shape.Empty;

            // import required services.
            this._gameMode = ServiceHelper.GetService<IGameMode>(typeof (IGameMode));
            this._scoreManager = ServiceHelper.GetService<IScoreManager>(typeof (IScoreManager));
        }

        public override void Attach(Shape shape)
        {
            shape.Parent = this;

            if (shape.LocationIndex == ShapeLocations.None)
                return;

            switch (shape.LocationIndex)
            {
                case PieLocations.TopLeft:
                    shape.Position = new Vector2(this.Position.X, this.Position.Y);
                    break;
                case PieLocations.TopRight:
                    shape.Position = new Vector2(this.Position.X + shape.Size.X, this.Position.Y);
                    break;
                case PieLocations.BottomRight:
                    shape.Position = new Vector2(this.Position.X + shape.Size.X, this.Position.Y + shape.Size.Y);
                    break;
                case PieLocations.BottomLeft:
                    shape.Position = new Vector2(this.Position.X, this.Position.Y + shape.Size.Y);
                    break;
            }

            shape.Bounds = new Rectangle((int)shape.Position.X, (int)shape.Position.Y, (int)shape.Size.X, (int)shape.Size.Y);
        }

        public override IEnumerable<Shape> GetEnumerator()
        {
            yield return this[PieLocations.TopLeft];
            yield return this[PieLocations.TopMiddle];
            yield return this[PieLocations.TopRight];
            yield return this[PieLocations.BottomRight];
            yield return this[PieLocations.BottomMiddle];
            yield return this[PieLocations.BottomLeft];
        }

        public override bool IsEmpty()
        {
            return this.IsEmpty(PieLocations.TopLeft) && this.IsEmpty(PieLocations.TopMiddle) &&
                   this.IsEmpty(PieLocations.TopRight) && this.IsEmpty(PieLocations.BottomLeft) &&
                   this.IsEmpty(PieLocations.BottomMiddle) && this.IsEmpty(PieLocations.BottomRight);
        }

        public override bool IsEmpty(byte locationIndex)
        {
            return this[locationIndex].IsEmpty;
        }

        public override bool IsFull()
        {
            return !this.IsEmpty(PieLocations.TopLeft) && !this.IsEmpty(PieLocations.TopMiddle) &&
                   !this.IsEmpty(PieLocations.TopRight) && !this.IsEmpty(PieLocations.BottomLeft) && 
                   !this.IsEmpty(PieLocations.BottomMiddle) && !this.IsEmpty(PieLocations.BottomRight);
        }

        public override void Explode()
        {
            if (!this.IsFull())
                return;

            bool isPerfect;
            var score = this._gameMode.RuleSet.CalculateExplosionScore(this, out isPerfect);
            this._scoreManager.AddScore(score, isPerfect);

            this[PieLocations.TopLeft] = Shape.Empty;
            this[PieLocations.TopMiddle] = Shape.Empty;
            this[PieLocations.TopRight] = Shape.Empty;
            this[PieLocations.BottomRight] = Shape.Empty;
            this[PieLocations.BottomMiddle] = Shape.Empty;
            this[PieLocations.BottomLeft] = Shape.Empty;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsFull())
                this.Explode();
        }


        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Instance.SpriteBatch.Begin();

            ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.PieContainerTexture, this.Bounds, Color.White);

            foreach (var shape in this.GetEnumerator())
            {
                if (shape.IsEmpty)
                    continue;

                var pie = ((PieShape)shape);

                var texture = this._gameMode.GetShapeTexture(pie);
                ScreenManager.Instance.SpriteBatch.Draw(texture, new Vector2(this.Bounds.Center.X, this.Bounds.Center.Y), null,
                                        Color.White, MathHelper.ToRadians(pie.LocationIndex * 60f ), new Vector2(48, 95),
                                        1f, SpriteEffects.None, 0);
            }

            ScreenManager.Instance.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
