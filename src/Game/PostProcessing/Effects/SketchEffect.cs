/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.PostProcessing.Effects
{
    public class SketchEffect : PostprocessingEffect
    {
        private readonly Random _random = new Random();

        private Effect _postprocessEffect; // Effect used to apply the edge detection and pencil sketch postprocessing.
        private Texture2D _sketchTexture; // Overlay texture containing the pencil sketch stroke pattern.

        // effect constants.
        private const float SketchThreshold = 0.2f;
        private const float SketchBrightness = 0.35f;
        private const float SketchJitterSpeed = 0.09f;

        // Randomly offsets the sketch pattern to create a hand-drawn animation effect.
        private Vector2 _sketchJitter;
        private TimeSpan _timeToNextJitter;

        public SketchEffect(Game game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            _postprocessEffect = AssetManager.Instance.LoadEffectShader(@"Effects\PostprocessEffect");
            _sketchTexture = Game.Content.Load<Texture2D>(@"Effects\SketchTexture");
        }

        public void UpdateJitter(GameTime gameTime)
        {
            _timeToNextJitter -= gameTime.ElapsedGameTime;

            if (_timeToNextJitter > TimeSpan.Zero) 
                return;

            _sketchJitter.X = (float) _random.NextDouble();
            _sketchJitter.Y = (float) _random.NextDouble();

            _timeToNextJitter += TimeSpan.FromSeconds(SketchJitterSpeed);
        }

        public override void Apply(Texture2D input)
        {
            GraphicsDevice.SetRenderTarget(null);

            var parameters = _postprocessEffect.Parameters;

            // Set effect parameters controlling the pencil sketch effect.
            parameters["SketchThreshold"].SetValue(SketchThreshold);
            parameters["SketchBrightness"].SetValue(SketchBrightness);
            parameters["SketchJitter"].SetValue(_sketchJitter);
            parameters["SketchTexture"].SetValue(_sketchTexture);

            // Activate the appropriate effect technique.
            _postprocessEffect.CurrentTechnique = _postprocessEffect.Techniques["ColorSketch"];

            // Draw a fullscreen sprite to apply the postprocessing effect.
            this.SpriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, _postprocessEffect);
            this.SpriteBatch.Draw(input, Vector2.Zero, Color.White);
            this.SpriteBatch.End();
        }
    }
}
