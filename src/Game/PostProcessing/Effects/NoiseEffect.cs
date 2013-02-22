/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.PostProcessing.Effects
{
    public class NoiseEffect : PostprocessingEffect
    {
        private Viewport _viewport;
        private readonly BlendState _blendState;
        private Texture2D _noiseTexture;
        private readonly Random _random = new Random();

        public NoiseEffect(Game game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            this._viewport = GraphicsDevice.Viewport;
            
            // We want this to be subtle
            // FinalColor = (SourceColor*One) - 0.15f * (DestinationColor*One)
            // assuming DestinationColor is the Noise pattern, we get:
            // FinalColor = SourceColor - 0.15f * NoisePattern
            _blendState = new BlendState
                {
                    ColorBlendFunction = BlendFunction.Subtract,
                    AlphaBlendFunction = BlendFunction.Subtract
                };

            // You can play with this to make the noise more visible (big values) or more subtle (smaller values)
            // You can even add color tinting to noise, if you want
            _blendState.BlendFactor = new Color(0.15f, 0.15f, 0.15f, 0.15f);
            _blendState.AlphaSourceBlend = _blendState.ColorSourceBlend = Blend.One;
            _blendState.AlphaDestinationBlend = _blendState.ColorDestinationBlend = Blend.BlendFactor;

            this._noiseTexture = Game.Content.Load<Texture2D>(@"Textures\Noise");
        }

        public override void Apply(Texture2D input)
        {
            // Instead of using the whole noise image, we leave out a border of 100 pixels
            // This way, we can play with the position of the SourceRectangle to make the noise seem animated
            int pieceWidth = this._noiseTexture.Width - 100;
            int pieceHeight = this._noiseTexture.Height - 100;
            var sourceRect = new Rectangle(_random.Next(100), _random.Next(100), pieceWidth, pieceHeight);

            GraphicsDevice.SetRenderTarget(null);

            this.SpriteBatch.Begin(SpriteSortMode.Deferred, this._blendState);
            
            this.SpriteBatch.Draw(this._noiseTexture, new Rectangle(0, 0, this._viewport.Width, this._viewport.Height), sourceRect, Color.White);
            this.SpriteBatch.Draw(input, Vector2.Zero, Color.White);
            
            this.SpriteBatch.End();

            base.Apply(input);
        }
    }
}
