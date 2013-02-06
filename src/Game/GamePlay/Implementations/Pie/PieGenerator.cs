using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenzied.Assets;
using Frenzied.GamePlay.Modes;
using Frenzied.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay.Implementations.Pie
{
    public class PieGenerator : ShapeGenerator
    {
        public static Vector2 Size = new Vector2(200, 200);

        private bool _generatorStarted = false;
        private TimeSpan _timeOutStart;
        private int timeOut = 4000;

        // required services.       
        private IScoreManager _scoreManager;
        private IGameMode _gameMode;

        public PieGenerator(Vector2 position, List<ShapeContainer> containers)
            : base(position, containers)
        {
            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Size.X, (int)Size.Y);

            this.CurrentShape = Shape.Empty;

            this._gameMode = (IGameMode) FrenziedGame.Instance.Services.GetService(typeof (IGameMode));
            this._scoreManager = (IScoreManager)FrenziedGame.Instance.Services.GetService(typeof(IScoreManager));

            if (this._scoreManager == null)
                throw new NullReferenceException("Can not find score manager component.");
        }

        public override void Attach(Shape shape)
        {
            shape.Parent = this;

            if (shape.LocationIndex == ShapeLocations.None)
                return;
        }

        public override void Detach(Shape shape)
        {
            this.CurrentShape = Shape.Empty;
        }

        public override bool IsEmpty()
        {
            return this.CurrentShape.IsEmpty;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsEmpty())
                this.Generate();

            if (this._generatorStarted)
            {
                this._timeOutStart = gameTime.TotalGameTime;
                this._generatorStarted = true;
            }
            else
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - this._timeOutStart.TotalMilliseconds > this.timeOut)
                {
                    this._scoreManager.TimeOut();
                    this._timeOutStart = gameTime.TotalGameTime;
                }
            }
        }

        public override void Generate()
        {
            var color = Randomizer.Next(1, 5);
            var availableLocations = this.GetAvailableLocations();

            if (availableLocations.Count == 0)
                return;

            var locationIndex = Randomizer.Next(availableLocations.Count);
            var location = availableLocations[locationIndex];

            this.CurrentShape = new PieShape((byte)color, (byte)location);
        }

        public override List<byte> GetAvailableLocations()
        {
            var availableLocations = new List<byte>();

            foreach (var container in this.Containers)
            {
                if (container.IsEmpty(PieLocations.TopLeft) && !availableLocations.Contains(PieLocations.TopLeft))
                    availableLocations.Add(PieLocations.TopLeft);
                if (container.IsEmpty(PieLocations.TopMiddle) && !availableLocations.Contains(PieLocations.TopMiddle))
                    availableLocations.Add(PieLocations.TopMiddle);
                if (container.IsEmpty(PieLocations.TopRight) && !availableLocations.Contains(PieLocations.TopRight))
                    availableLocations.Add(PieLocations.TopRight);
                if (container.IsEmpty(PieLocations.BottomLeft) && !availableLocations.Contains(PieLocations.BottomLeft))
                    availableLocations.Add(PieLocations.BottomLeft);
                if (container.IsEmpty(PieLocations.BottomMiddle) && !availableLocations.Contains(PieLocations.BottomMiddle))
                    availableLocations.Add(PieLocations.BottomMiddle);
                if (container.IsEmpty(PieLocations.BottomRight) && !availableLocations.Contains(PieLocations.BottomRight))
                    availableLocations.Add(PieLocations.BottomRight);
            }

            return availableLocations;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Instance.SpriteBatch.Begin();

            ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.PieContainerTexture, this.Bounds, Color.White);

            if (!this.IsEmpty())
            {
                var texture = this._gameMode.GetShapeTexture(this.CurrentShape);
                ScreenManager.Instance.SpriteBatch.Draw(texture, new Vector2(this.Bounds.Center.X, this.Bounds.Center.Y), null,
                                                        Color.White, MathHelper.ToRadians(this.CurrentShape.LocationIndex * 60f), new Vector2(48, 95),
                                                        1f, SpriteEffects.None, 0);
            }

            // progressbar.
            //var timeOutLeft = this.timeOut - (int)(gameTime.TotalGameTime.TotalMilliseconds - this._timeOutStart.TotalMilliseconds);
            //timeOutLeft = (int)(timeOutLeft / 1000);
            //timeOutLeft++;

            //var width = (int)(31.25f * timeOutLeft);

            //ScreenManager.Instance.SpriteBatch.Draw(this._progressBarTexture, new Rectangle(5, 200, width, 10), new Rectangle(0, 0, width, 10),
            //                                        Color.White);

            ScreenManager.Instance.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
