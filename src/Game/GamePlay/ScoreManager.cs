/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Text;
using Frenzied.Assets;
using Frenzied.GamePlay.Implementations;
using Frenzied.GamePlay.Modes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay
{
    public interface IScoreManager
    {
        int Score { get; }
        int Lives { get; }

        void CorrectMove(ShapeContainer container);
        void TimeOut();
    }

    public class ScoreManager : DrawableGameComponent , IScoreManager
    {
        public int Score { get; private set; }
        public int Lives { get; private set; }

        // required services.       
        private IAssetManager _assetManager;

        // resources.
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        // for grabbing internal string, we should init string builder capacity and max capacity ctor so that, grabbed internal string is always valid. - http://www.gavpugh.com/2010/03/23/xnac-stringbuilder-to-string-with-no-garbage/
        private readonly StringBuilder _stringBuilder = new StringBuilder(512, 512);

        public ScoreManager(Game game)
            : base(game)
        {
            this.Game.Services.AddService(typeof(IScoreManager), this); // export the service.
        }

        public override void Initialize()
        {
            this.Score = 0;
            this.Lives = 5;

            this._assetManager = (IAssetManager)this.Game.Services.GetService(typeof(IAssetManager));
            if (this._assetManager == null)
                throw new NullReferenceException("Can not find asset manager component.");

            base.Initialize();
        }

        public void CorrectMove(ShapeContainer container)
        {
            var emptyBlocks = 0;
            var orangeBlocks = 0;
            var purpleBlocks = 0;
            var greenBlocks = 0;
            var blueBlocks = 0;

            foreach (var block in container.GetEnumerator())
            {
                switch (block.ColorIndex)
                {
                    case BlockColors.None:
                        emptyBlocks++;
                        break;
                    case BlockColors.Orange:
                        orangeBlocks++;
                        break;
                    case BlockColors.Purple:
                        purpleBlocks++;
                        break;
                    case BlockColors.Green:
                        greenBlocks++;
                        break;
                    case BlockColors.Blue:
                        blueBlocks++;
                        break;
                }
            }

            var mostSameColor = 0;
            if (orangeBlocks > mostSameColor)
                mostSameColor = orangeBlocks;
            if (purpleBlocks > mostSameColor)
                mostSameColor = purpleBlocks;
            if (greenBlocks > mostSameColor)
                mostSameColor = greenBlocks;
            if (blueBlocks > mostSameColor)
                mostSameColor = blueBlocks;

            if (mostSameColor == 4)
                this.Score += 100;
            if (mostSameColor == 3)
                this.Score += 50;
            if (mostSameColor == 2)
                this.Score += 10;
            if (mostSameColor == 1)
                this.Score += 1;
        }

        public void TimeOut()
        {
            this.Lives--;
            #if !WINPHONE8
            //this._assetManager.Sounds.Timeout.Play();
            #endif
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = this._assetManager.GoodDog;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // lives
            _stringBuilder.Length = 0;
            _stringBuilder.Append("lifes:");
            _stringBuilder.Append(this.Lives);
            _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(5, 100), Color.White);

            // lives
            _stringBuilder.Length = 0;
            _stringBuilder.Append("score:");
            _stringBuilder.Append(this.Score);
            _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(5, 130), Color.White);


            _spriteBatch.End();
        }
    }
}
