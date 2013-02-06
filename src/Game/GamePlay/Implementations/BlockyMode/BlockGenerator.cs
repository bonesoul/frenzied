﻿/*
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay.Implementations.BlockyMode
{
    internal class BlockGenerator : ShapeGenerator
    {
        public static Vector2 Size = new Vector2(210, 210);

        private Texture2D _progressBarTexture;

        private bool _generatorStarted = false;
        private TimeSpan _timeOutStart;
        private int timeOut = 4000;

        // required services.       
        private IScoreManager _scoreManager;
        private IGameMode _gameMode;  

        public BlockGenerator(Vector2 position, List<ShapeContainer> containers)
            : base(position, containers)
        {
            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Size.X, (int)Size.Y);

            this.CurrentShape = Shape.Empty;

            this._gameMode = (IGameMode)FrenziedGame.Instance.Services.GetService(typeof(IGameMode));
            this._scoreManager = (IScoreManager)FrenziedGame.Instance.Services.GetService(typeof(IScoreManager));

            if (this._scoreManager == null)
                throw new NullReferenceException("Can not find score manager component.");

            this._progressBarTexture = AssetManager.Instance.BlockProgressBar;
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

            this.CurrentShape = new BlockShape((byte)color, (byte)location);
        }

        public override List<byte> GetAvailableLocations()
        {
            var availableLocations = new List<byte>();

            foreach (var container in this.Containers)
            {
                if (container.IsEmpty(BlockLocations.TopLeft) && !availableLocations.Contains(BlockLocations.TopLeft))
                    availableLocations.Add(BlockLocations.TopLeft);
                if (container.IsEmpty(BlockLocations.TopRight) && !availableLocations.Contains(BlockLocations.TopRight))
                    availableLocations.Add(BlockLocations.TopRight);
                if (container.IsEmpty(BlockLocations.BottomLeft) && !availableLocations.Contains(BlockLocations.BottomLeft))
                    availableLocations.Add(BlockLocations.BottomLeft);
                if (container.IsEmpty(BlockLocations.BottomRight) && !availableLocations.Contains(BlockLocations.BottomRight))
                    availableLocations.Add(BlockLocations.BottomRight);
            }

            return availableLocations;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Instance.SpriteBatch.Begin();

            ScreenManager.Instance.SpriteBatch.Draw(AssetManager.Instance.BlockContainerTexture, this.Bounds, Color.White);

            if (!this.IsEmpty())
            {
                var texture = this._gameMode.GetShapeTexture(this.CurrentShape);
                ScreenManager.Instance.SpriteBatch.Draw(texture, this.CurrentShape.Bounds, Color.White);
            }

            // progressbar.
            var timeOutLeft = this.timeOut - (int)(gameTime.TotalGameTime.TotalMilliseconds - this._timeOutStart.TotalMilliseconds);
            timeOutLeft = (int)(timeOutLeft / 1000);
            timeOutLeft++;

            var width = (int)(31.25f * timeOutLeft);

            ScreenManager.Instance.SpriteBatch.Draw(this._progressBarTexture, new Rectangle(5, 200, width, 10), new Rectangle(0, 0, width, 10),
                                                    Color.White);

            ScreenManager.Instance.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
