/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Text;
using Frenzied.Common.Extensions;
using Frenzied.Core.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Common.Debug
{
    /// <summary>
    /// Allows interaction with the statistics service.
    /// </summary>
    public interface IStatistics
    {
        /// <summary>
        /// Returns current FPS.
        /// </summary>
        int FPS { get; }

        /// <summary>
        /// Returns the memory size.
        /// </summary>
        /// <returns></returns>
        long MemoryUsed { get; }

        int GenerateQueue { get; }
        int LightenQueue { get; }
        int BuildQueue { get; }
        int ReadyQueue { get; }
        int RemovalQueue { get; }
    }

    public sealed class Statistics : DrawableGameComponent, IStatistics
    {
        // fps stuff.
        public int FPS { get; private set; } // the current FPS.
        private int _frameCounter = 0; // the frame count.
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        // memory stuff
        /// <summary>
        /// Returns the memory size.
        /// </summary>
        /// <returns></returns>
        public long MemoryUsed
        {
            get { return GC.GetTotalMemory(false); }
        }

        // drawn-text stuff.
        private string _drawnBlocks;
        private string _totalBlocks;

        // resources.
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        // for grabbing internal string, we should init string builder capacity and max capacity ctor so that, grabbed internal string is always valid.
        // http://www.gavpugh.com/2010/03/23/xnac-stringbuilder-to-string-with-no-garbage/
        private readonly StringBuilder _stringBuilder = new StringBuilder(512, 512);

        // required services.       
        private IAssetManager _assetManager;

        public int GenerateQueue { get; private set; }
        public int LightenQueue { get; private set; }
        public int BuildQueue { get; private set; }
        public int ReadyQueue { get; private set; }
        public int RemovalQueue { get; private set; }

        /// <summary>
        /// Creates a new statistics service instance.
        /// </summary>
        /// <param name="game"></param>
        public Statistics(Game game)
            : base(game)
        {
            this.Game.Services.AddService(typeof(IStatistics), this); // export the service.
        }

        /// <summary>
        /// Initializes the statistics service.
        /// </summary>
        public override void Initialize()
        {
            this._assetManager = (IAssetManager)this.Game.Services.GetService(typeof(IAssetManager));
            if (this._assetManager == null)
                throw new NullReferenceException("Can not find asset manager component.");

            base.Initialize();
        }

        /// <summary>
        /// Loads required assets & content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = this._assetManager.Verdana;
        }

        /// <summary>
        /// Calculates the FPS.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            this._elapsedTime += gameTime.ElapsedGameTime;
            if (this._elapsedTime < TimeSpan.FromSeconds(1))
                return;

            this._elapsedTime -= TimeSpan.FromSeconds(1);
            this.FPS = _frameCounter;
            this._frameCounter = 0;
        }

        /// <summary>
        /// Draws the statistics.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Attention: DO NOT use string.format as it's slower than string concat.

            _frameCounter++;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // FPS
            _stringBuilder.Length = 0;
            _stringBuilder.Append("fps:");
            _stringBuilder.Append(this.FPS);
            _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(5, 5), Color.White);

            // mem used
            _stringBuilder.Length = 0;
            _stringBuilder.Append("mem:");
            _stringBuilder.Append(this.MemoryUsed.GetKiloString());
            _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(75, 5), Color.White);

            _spriteBatch.End();
        }
    }
}