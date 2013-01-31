/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Frenzied.Assets;
using Frenzied.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay
{
    public class BlockGenerator : DrawableGameComponent
    {
        public static Vector2 Size = new Vector2(205, 205);

        public Vector2 Position { get; protected set; }
        public Rectangle Bounds { get; protected set; }
        public Block CurretBlock { get; private set; }

        private readonly Random _randomizer = new Random(Environment.TickCount);
        private Texture2D _progressBarTexture;

        private List<BlockContainer> _containers;

        private bool _generatorStarted = false;
        private TimeSpan _timeOutStart;
        private int timeOut = 4000;

        // required services.       
        private IScoreManager _scoreManager;
        

        public BlockGenerator(Game game, Vector2 position, List<BlockContainer> containers)
            : base(game)
        {
            this.Position = position;
            this.Bounds = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);
            this._containers = containers;
            this.CurretBlock = null;

            this._progressBarTexture = AssetManager.Instance.BlockProgressBar;
        }

        public override void Initialize()
        {
            this._scoreManager = (IScoreManager)this.Game.Services.GetService(typeof(IScoreManager));
            if (this._scoreManager == null)
                throw new NullReferenceException("Can not find score manager component.");

            base.Initialize();
        }

        public bool IsEmpty
        {
            get { return this.CurretBlock == null; }
        }

        public void CurrentBlockUsed()
        {
            this.CurretBlock = null;
        }

        public override void Update(GameTime gameTime)
        {
            if(this.IsEmpty)
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

            base.Update(gameTime);
        }

        public void Generate()
        {
            var color = _randomizer.Next(1, 5);
            var availableLocations = this.GetAvailableLocations();
            if (availableLocations.Count == 0)
                return;

            var locationIndex = _randomizer.Next(availableLocations.Count);
            var location = availableLocations[locationIndex];

            this.CurretBlock = new Block((BlockLocation)location, (BlockColor)color);
        }

        private List<BlockLocation> GetAvailableLocations()
        {
            var availableLocations=new List<BlockLocation>();

            foreach (var container in this._containers)
            {
                if (container.IsEmpty(BlockLocation.topleft) && !availableLocations.Contains(BlockLocation.topleft))
                        availableLocations.Add(BlockLocation.topleft);
                else if (container.IsEmpty(BlockLocation.topright) && !availableLocations.Contains(BlockLocation.topright))
                    availableLocations.Add(BlockLocation.topright);
                else if (container.IsEmpty(BlockLocation.bottomleft) && !availableLocations.Contains(BlockLocation.bottomleft))
                    availableLocations.Add(BlockLocation.bottomleft);
                else if (container.IsEmpty(BlockLocation.bottomright) && !availableLocations.Contains(BlockLocation.bottomright))
                    availableLocations.Add(BlockLocation.bottomright);
            }

            return availableLocations;
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.IsEmpty)
                return;

            ScreenManager.Instance.SpriteBatch.Begin();

            var texture = AssetManager.Instance.GetBlockTexture(this.CurretBlock);


            var blockRectangle=new Rectangle();
            switch (this.CurretBlock.Location)
            {
                case BlockLocation.topleft:
                    blockRectangle = new Rectangle(this.Bounds.X + Block.PositionOffset,
                                                this.Bounds.Y + Block.PositionOffset, (int)Block.Size.X, (int)Block.Size.Y);
                    break;
                case BlockLocation.topright:
                    blockRectangle = new Rectangle(this.Bounds.X + (int)Block.Size.X + Block.PositionOffset,
                                                this.Bounds.Y + Block.PositionOffset, (int)Block.Size.X, (int)Block.Size.Y);
                    break;
                case BlockLocation.bottomleft:
                    blockRectangle = new Rectangle(this.Bounds.X + Block.PositionOffset,
                                                this.Bounds.Y + (int)Block.Size.Y + Block.PositionOffset, (int)Block.Size.X, (int)Block.Size.Y);
                    break;
                case BlockLocation.bottomright:
                    blockRectangle = new Rectangle(this.Bounds.X + (int)Block.Size.X + Block.PositionOffset,
                                                this.Bounds.Y + (int)Block.Size.Y + Block.PositionOffset, (int)Block.Size.X, (int)Block.Size.Y);
                    break;
                default:
                    break;
            }

            ScreenManager.Instance.SpriteBatch.Draw(texture, blockRectangle, Color.White);

            // progressbar.


            var timeOutLeft = this.timeOut - (int) (gameTime.TotalGameTime.TotalMilliseconds - this._timeOutStart.TotalMilliseconds);
            timeOutLeft = (int)(timeOutLeft/1000);
            timeOutLeft++;

            var width = (int)(31.25f * timeOutLeft);

            ScreenManager.Instance.SpriteBatch.Draw(this._progressBarTexture, new Rectangle(5, 200, width, 10),new Rectangle(0,0,width,10),
                                                    Color.White);
                

            ScreenManager.Instance.SpriteBatch.End();   
            
            base.Draw(gameTime);
        }
    }
}
