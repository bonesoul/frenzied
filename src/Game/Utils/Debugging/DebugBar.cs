/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Text;
using Frenzied.Assets;
using Frenzied.Graphics.Drawing;
using Frenzied.Utils.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Utils.Debugging
{
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
    }

    internal class DebugBar : DrawableGameComponent, IStatistics
    {
        /// <summary>
        /// Returns current FPS.
        /// </summary>
        public int FPS { get; private set; }

        /// <summary>
        /// Returns the memory size.
        /// </summary>
        /// <returns></returns>
        public long MemoryUsed { get { return GC.GetTotalMemory(false); } }

        // resources.
        private PrimitiveBatch _primitiveBatch;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Matrix _localProjection;
        private Matrix _localView;
        private Rectangle _bounds;
        private readonly Vector2[] _backgroundPolygon = new Vector2[4];

        // for grabbing internal string, we should init string builder capacity and max capacity ctor so that, grabbed internal string is always valid. - http://www.gavpugh.com/2010/03/23/xnac-stringbuilder-to-string-with-no-garbage/
        private readonly StringBuilder _stringBuilder = new StringBuilder(512, 512);

        // required services.       
        private IAssetManager _assetManager;

        // internal counters.
        private int _frameCounter = 0; // the frame count.
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        public DebugBar(Game game)
            : base(game)
        {
            this.Game.Services.AddService(typeof(IStatistics), this); // export the service.
        }

        /// <summary>
        /// Initializes the debug-bar service.
        /// </summary>
        public override void Initialize()
        {
            this._assetManager = (IAssetManager)this.Game.Services.GetService(typeof(IAssetManager));

            if (this._assetManager == null)
                throw new NullReferenceException("Can not find asset manager component.");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // load resources.
            this._primitiveBatch = new PrimitiveBatch(this.GraphicsDevice, 1000);
            this._localProjection = Matrix.CreateOrthographicOffCenter(0f, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            this._localView = Matrix.Identity;
            this._spriteBatch = new SpriteBatch(GraphicsDevice);
            this._spriteFont = this._assetManager.Verdana;

            // init bounds.
            this._bounds = new Rectangle(10, 10, this.Game.GraphicsDevice.Viewport.Bounds.Width - 20, 10);
            this._backgroundPolygon[0] = new Vector2(_bounds.X - 2, _bounds.Y - 2); // top left
            this._backgroundPolygon[1] = new Vector2(_bounds.X - 2, _bounds.Y + _bounds.Height + 14); // bottom left
            this._backgroundPolygon[2] = new Vector2(_bounds.X + 2 + _bounds.Width, _bounds.Y + _bounds.Height + 14); // bottom right
            this._backgroundPolygon[3] = new Vector2(_bounds.X + 2 + _bounds.Width, _bounds.Y - 2); // top right
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
            _frameCounter++;

            // backup  the raster and depth-stencil states.
            var previousRasterizerState = this.Game.GraphicsDevice.RasterizerState;
            var previousDepthStencilState = this.Game.GraphicsDevice.DepthStencilState;

            // set new states for drawing primitive shapes.
            this.Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _primitiveBatch.Begin(this._localProjection, this._localView); // initialize the primitive batch.

            BasicShapes.DrawSolidPolygon(this._primitiveBatch, this._backgroundPolygon, 4, Color.Black, true);

            _primitiveBatch.End(); // end the batch.

            // restore old states.
            this.Game.GraphicsDevice.RasterizerState = previousRasterizerState;
            this.Game.GraphicsDevice.DepthStencilState = previousDepthStencilState;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Attention: DO NOT use string.format as it's slower than string concat.

            // FPS
            _stringBuilder.Length = 0;
            _stringBuilder.Append("fps:");
            _stringBuilder.Append(this.FPS);
            _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(this._bounds.X + 5, this._bounds.Y + 5), Color.White);

            // mem used
            _stringBuilder.Length = 0;
            _stringBuilder.Append("mem:");
            _stringBuilder.Append(this.MemoryUsed.GetKiloString());
            _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(this._bounds.X + 75, this._bounds.Y + 5), Color.White);

            _spriteBatch.End();
        }
    }
}
