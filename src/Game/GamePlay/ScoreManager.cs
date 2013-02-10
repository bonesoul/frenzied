/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Diagnostics;
using System.Text;
using Frenzied.Assets;
using Frenzied.GamePlay.Implementations.BlockyMode;
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

        void TimeOut();

        /// <summary>
        /// Adds score to current score.
        /// </summary>
        /// <param name="score"></param>
        void AddScore(int score);
    }

    public class ScoreManager : DrawableGameComponent , IScoreManager
    {
        public int Score { get; private set; }
        public int Lives { get; private set; }

        // required services.       
        private IAssetManager _assetManager;
        private IGameMode _gameMode;

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
            // import required services.
            this._gameMode = ServiceHelper.GetService<IGameMode>(typeof(IGameMode));
            this._assetManager = ServiceHelper.GetService<IAssetManager>(typeof (IAssetManager));        

            this.Score = 0;
            this.Lives = this._gameMode.RuleSet.StartingLifes;

            base.Initialize();
        }

        public void TimeOut()
        {
            this.Lives--;
            #if !WINPHONE8
            //this._assetManager.Sounds.Timeout.Play();
            #endif
        }

        public void AddScore(int score)
        {
            this.Score += score;
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


            for (int i = 0; i < leftPiesCount; i++)
            {
                this.DrawTimeOutPie(gameTime, 6 -i);
            }                

            _spriteBatch.End();
        }

        private TimeSpan _targetTimeout;
        private int leftPiesCount;

        public override void Update(GameTime gameTime)
        {
            if (_targetTimeout == TimeSpan.Zero)
            {
                this._targetTimeout = gameTime.TotalGameTime +
                                      new TimeSpan(0, 0, 0,0, this._gameMode.RuleSet.ShapePlacementTimeout);
            }

            var left = this._targetTimeout - gameTime.TotalGameTime;
            leftPiesCount = (int) (left.TotalMilliseconds/(this._gameMode.RuleSet.ShapePlacementTimeout/6));
            leftPiesCount++;

            if (gameTime.TotalGameTime >= _targetTimeout)
            {
                this._targetTimeout = gameTime.TotalGameTime +
                                      new TimeSpan(0, 0, 0, 0, this._gameMode.RuleSet.ShapePlacementTimeout);
            }
        }

        private void DrawTimeOutPie(GameTime gameTime, int pieIndex)
        {
            var texture = AssetManager.Instance.PieTextures[Color.Orange];
            var radians = MathHelper.ToRadians((float) (pieIndex*60f - 30));
            _spriteBatch.Draw(texture, new Vector2(100, 300), null, Color.Green, radians,
                              new Vector2(48, 95),0.5f, SpriteEffects.None, 0);
        }
    }
}
