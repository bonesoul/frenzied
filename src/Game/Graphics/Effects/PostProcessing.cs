/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenzied.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Graphics.Effects
{
    public class PostProcessing:DrawableGameComponent
    {
        Random random = new Random();


        SpriteBatch spriteBatch;

        // Effect used to apply the edge detection and pencil sketch postprocessing.
        Effect postprocessEffect;

        // Overlay texture containing the pencil sketch stroke pattern.
        Texture2D sketchTexture;

        // Randomly offsets the sketch pattern to create a hand-drawn animation effect.
        Vector2 sketchJitter;
        TimeSpan timeToNextJitter;

        // Custom rendertargets.
        public static RenderTarget2D sceneRenderTarget;

        // Choose what display settings to use.
        public static NonPhotoRealisticSettings Settings
        {
            get { return NonPhotoRealisticSettings.PresetSettings[0]; }
        }

        public PostProcessing(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.GraphicsDevice);

            postprocessEffect = Game.Content.Load<Effect>(@"Effects\PostprocessEffect");
            sketchTexture = Game.Content.Load<Texture2D>(@"Effects\SketchTexture");

            // Create two custom rendertargets.
            PresentationParameters pp = this.GraphicsDevice.PresentationParameters;

            sceneRenderTarget = new RenderTarget2D(this.GraphicsDevice,
                                                   pp.BackBufferWidth, pp.BackBufferHeight, false,
                                                   pp.BackBufferFormat, pp.DepthStencilFormat);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Update the sketch overlay texture jitter animation.
            if (Settings.SketchJitterSpeed > 0)
            {
                timeToNextJitter -= gameTime.ElapsedGameTime;

                if (timeToNextJitter <= TimeSpan.Zero)
                {
                    sketchJitter.X = (float)random.NextDouble();
                    sketchJitter.Y = (float)random.NextDouble();

                    timeToNextJitter += TimeSpan.FromSeconds(Settings.SketchJitterSpeed);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {


            // Run the postprocessing filter over the scene that we just rendered.
            if (Settings.EnableSketch)
            {
                this.GraphicsDevice.SetRenderTarget(null);

                ApplyPostprocess();
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Helper applies the edge detection and pencil sketch postprocess effect.
        /// </summary>
        void ApplyPostprocess()
        {
            EffectParameterCollection parameters = postprocessEffect.Parameters;
            string effectTechniqueName;

            // Set effect parameters controlling the pencil sketch effect.
            if (Settings.EnableSketch)
            {
                parameters["SketchThreshold"].SetValue(Settings.SketchThreshold);
                parameters["SketchBrightness"].SetValue(Settings.SketchBrightness);
                parameters["SketchJitter"].SetValue(sketchJitter);
                parameters["SketchTexture"].SetValue(sketchTexture);
            }


            effectTechniqueName = "ColorSketch";

            // Activate the appropriate effect technique.
            postprocessEffect.CurrentTechnique = postprocessEffect.Techniques[effectTechniqueName];

            // Draw a fullscreen sprite to apply the postprocessing effect.
            spriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, postprocessEffect);
            spriteBatch.Draw(sceneRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
