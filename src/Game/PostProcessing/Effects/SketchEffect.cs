/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.Assets;
using Frenzied.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.PostProcessing.Effects
{
    public class SketchEffect : PostprocessingEffect
    {
        private readonly RenderTarget2D _buffer;
        private Viewport _viewport;

        private Effect _postprocessEffect; // Effect used to apply the edge detection and pencil sketch postprocessing.
        private Texture2D _sketchTexture; // Overlay texture containing the pencil sketch stroke pattern.

        public SketchEffect(Game game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            this._viewport = this.GraphicsDevice.Viewport;
            _buffer = new RenderTarget2D(this.GraphicsDevice, this._viewport.Width, this._viewport.Height);
            _postprocessEffect = AssetManager.Instance.LoadEffectShader(@"Effects\PostprocessEffect");
            _sketchTexture = Game.Content.Load<Texture2D>(@"Effects\SketchTexture");
        }

        public override Texture2D Apply(Texture2D input, GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(this._buffer);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.SpriteBatch.Begin();

            this.SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            return this._buffer;
        }
    }
}
