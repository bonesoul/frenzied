﻿/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Frenzied.Assets;
using Frenzied.Graphics.Drawing;
using Frenzied.Platforms;
using Frenzied.Utils.Debugging.Graphs.Implementations;
using Frenzied.Utils.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Utils.Debugging.Graphs
{
    /// <summary>
    /// GraphManager can render debug graphs.
    /// </summary>
    public interface IGraphManager
    { }

    /// <summary>
    /// GraphManager is DrawableGameComponent that can render debug graphs.
    /// </summary>
    public class GraphManager : DrawableGameComponent, IGraphManager
    {
        // stuff needed for drawing.
        private PrimitiveBatch _primitiveBatch;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Matrix _localProjection;
        private Matrix _localView;

        private IAssetManager _assetManager;

        private readonly List<DebugGraph> _graphs = new List<DebugGraph>(); // the current graphs list.

        public GraphManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IGraphManager), this); // export the service.
        }

        public override void Initialize()
        {
            // create the graphs modules.
            this._graphs.Add(new FPSGraph(this.Game, new Rectangle(this.Game.GraphicsDevice.Viewport.Bounds.Width - 280, 40, 270, 35)));
            this._graphs.Add(new MemGraph(this.Game, new Rectangle(this.Game.GraphicsDevice.Viewport.Bounds.Width - 280, 95, 270, 35)));

            // import required services.
            this._assetManager = ServiceHelper.GetService<IAssetManager>(typeof(IAssetManager)); 

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // init the drawing related objects.
            _primitiveBatch = new PrimitiveBatch(this.GraphicsDevice, 1000);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = this._assetManager.Verdana;
            _localProjection = Matrix.CreateOrthographicOffCenter(0f, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            _localView = Matrix.Identity;

            // attach the drawing objects to the graph modules.
            foreach (var graph in this._graphs)
            {
                graph.AttachGraphics(this._primitiveBatch, this._spriteBatch, this._spriteFont, this._localProjection, this._localView);
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!PlatformManager.Handler.Config.Debugger.GraphsEnabled)
                return;

            foreach (var graph in this._graphs)
            {
                graph.Update(gameTime); // let the graphs update themself.
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!PlatformManager.Handler.Config.Debugger.GraphsEnabled)
                return;

            // backup  the raster and depth-stencil states.
            var previousRasterizerState = this.Game.GraphicsDevice.RasterizerState;
            var previousDepthStencilState = this.Game.GraphicsDevice.DepthStencilState;

            // set new states for drawing primitive shapes.
            this.Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _primitiveBatch.Begin(this._localProjection, this._localView); // initialize the primitive batch.

            foreach (var graph in this._graphs)
            {
                graph.DrawGraph(gameTime); // let the graphs draw their primitives.
            }

            _primitiveBatch.End(); // end the batch.

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); // initialize the sprite batch.

            foreach (var graph in this._graphs)
            {
                graph.DrawStrings(gameTime); // let the graphs draw their sprites.
            }

            _spriteBatch.End(); // end the batch.

            // restore old states.
            this.Game.GraphicsDevice.RasterizerState = previousRasterizerState;
            this.Game.GraphicsDevice.DepthStencilState = previousDepthStencilState;

            base.Draw(gameTime);
        }
    }
}
