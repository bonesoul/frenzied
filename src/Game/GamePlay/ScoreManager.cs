/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Text;
using Frenzied.Assets;
using Frenzied.GamePlay.Modes;
using Frenzied.Utils.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay
{
    public interface IScoreManager
    {
        int Score { get; }
        int Lives { get; }

        /// <summary>
        /// Adds score to current score.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="isPerfect">Set to true when user commits a perfect move where he explodes a container with all same colors sub-shapes!</param>
        void AddScore(int score, bool isPerfect);

        /// <summary>
        /// Called when user commits a move.
        /// </summary>
        void MoveCommitted(GameTime gameTime);
    }

    public class ScoreManager : DrawableGameComponent, IScoreManager
    {
        public int Score { get; private set; }
        public int Lives { get; private set; }

        // required services.       
        private IAssetManager _assetManager;
        private IGameMode _gameMode;

        // resources.
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        // timeout resources.
        private TimeSpan NextTimeout; // next timeout.
        private TimeSpan TimeLeftForNextTimeout; // time left to next timeout.
        private int TimeoutPiesLeftCount; // number of pies to render.
        private int TimeoutStepCount; // total number of steps in a single timeout.

        // no-garbage-stringbuilder - for grabbing internal string, we should init string builder capacity and max capacity ctor so that, grabbed internal string is always valid. - http://www.gavpugh.com/2010/03/23/xnac-stringbuilder-to-string-with-no-garbage/
        private readonly StringBuilder _stringBuilder = new StringBuilder(512, 512);

        public ScoreManager(Game game)
            : base(game)
        {
            this.Game.Services.AddService(typeof(IScoreManager), this); // export the service.
        }

        public override void Initialize()
        {
            // import required services.
            this._gameMode = ServiceHelper.GetService<IGameMode>(typeof(IGameMode));
            this._assetManager = ServiceHelper.GetService<IAssetManager>(typeof(IAssetManager));

            this.Score = 0;
            this.Lives = this._gameMode.RuleSet.StartingLifes;
            this.TimeoutStepCount = this._gameMode.RuleSet.ShapePlacementTimeout / 7; // our timeout bar has 6 pies and we want additional step for empty state.

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = this._assetManager.GoodDog;
        }

        public void AddScore(int score, bool isPerfect)
        {
            this.Score += score;

            if (isPerfect)
                this.Lives++;
        }

        public void MoveCommitted(GameTime gameTime)
        {
            this.ResetNextTimeout(gameTime);
        }

        public void TimeOut(GameTime gameTime)
        {
            this.Lives--;
            this.ResetNextTimeout(gameTime);

#if !WINPHONE8
            //this._assetManager.Sounds.Timeout.Play();
#endif
        }

        private void ResetNextTimeout(GameTime gameTime)
        {
            this.NextTimeout = gameTime.TotalGameTime + new TimeSpan(0, 0, 0, 0, this._gameMode.RuleSet.ShapePlacementTimeout);
        }

        public override void Update(GameTime gameTime)
        {
            if (NextTimeout == TimeSpan.Zero) // initialize the next timeout if we don't have one yet.
                this.ResetNextTimeout(gameTime);

            TimeLeftForNextTimeout = this.NextTimeout - gameTime.TotalGameTime; // calculate the time left.
            TimeoutPiesLeftCount = (int)(TimeLeftForNextTimeout.TotalMilliseconds / this.TimeoutStepCount); // calculate number of pies to render.

            if (TimeoutPiesLeftCount == 0 && TimeLeftForNextTimeout.TotalMilliseconds < 100)
                this.TimeOut(gameTime); // run the actual timeout code.
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

            // render timeout-bar pie's.
            for (int i = 0; i < TimeoutPiesLeftCount; i++)
            {
                this.DrawTimeoutBarPie(6 - i);
            }

            _spriteBatch.DrawString(AssetManager.Instance.Verdana, string.Format("pie-count: {0}", TimeoutPiesLeftCount), new Vector2(0, 400), Color.White);
            _spriteBatch.DrawString(AssetManager.Instance.Verdana, string.Format("secs-left: {0}", TimeLeftForNextTimeout.TotalMilliseconds), new Vector2(0, 410), Color.White);

            _spriteBatch.End();
        }

        private void DrawTimeoutBarPie(int pieIndex)
        {
            var texture = AssetManager.Instance.PieTextures[Color.Orange];
            var radians = MathHelper.ToRadians((float)(pieIndex * 60f - 30));
            _spriteBatch.Draw(texture, new Vector2(100, 300), null, Color.Green, radians, new Vector2(48, 95), 0.5f, SpriteEffects.None, 0);
        }
    }
}
