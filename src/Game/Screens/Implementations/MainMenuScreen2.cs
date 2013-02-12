/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Assets;
using Frenzied.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Screens.Implementations
{
    public class MainMenuScreen2 : GameScreen
    {
        private SpriteBatch _spriteBatch;

        Random random = new Random();

        // Effect used to apply the edge detection and pencil sketch postprocessing.
        Effect postprocessEffect;

        // Overlay texture containing the pencil sketch stroke pattern.
        Texture2D sketchTexture;

        // Randomly offsets the sketch pattern to create a hand-drawn animation effect.
        Vector2 sketchJitter;
        TimeSpan timeToNextJitter;

        // Custom rendertargets.
        RenderTarget2D sceneRenderTarget;

        private Texture2D _menuTextureCredits;
        private Texture2D _menuTextureCustomMode;
        private Texture2D _menuTextureOptions;
        private Texture2D _menuTextureQuickPlay;
        private Texture2D _menuTextureTutorial;

        public override void LoadContent()
        {
            this._spriteBatch = new SpriteBatch(FrenziedGame.Instance.GraphicsDevice);

            postprocessEffect = FrenziedGame.Instance.Content.Load<Effect>(@"Effects\PostprocessEffect");
            sketchTexture = FrenziedGame.Instance.Content.Load<Texture2D>(@"Effects\SketchTexture");

            // Create two custom rendertargets.
            PresentationParameters pp = FrenziedGame.Instance.GraphicsDevice.PresentationParameters;

            sceneRenderTarget = new RenderTarget2D(FrenziedGame.Instance.GraphicsDevice,
                                                   pp.BackBufferWidth, pp.BackBufferHeight, false,
                                                   pp.BackBufferFormat, pp.DepthStencilFormat);

            this._menuTextureCredits = AssetManager.Instance.MenuCredits;
            this._menuTextureCustomMode = AssetManager.Instance.MenuCustomMode;
            this._menuTextureOptions = AssetManager.Instance.MenuOptions;
            this._menuTextureQuickPlay = AssetManager.Instance.MenuQuickPlay;
            this._menuTextureTutorial = AssetManager.Instance.MenuTutorial;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Update the sketch overlay texture jitter animation.
            if (SketchSettings.Settings.SketchJitterSpeed > 0)
            {
                timeToNextJitter -= gameTime.ElapsedGameTime;

                if (timeToNextJitter <= TimeSpan.Zero)
                {
                    sketchJitter.X = (float)random.NextDouble();
                    sketchJitter.Y = (float)random.NextDouble();

                    timeToNextJitter += TimeSpan.FromSeconds(SketchSettings.Settings.SketchJitterSpeed);
                }
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            //FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(sceneRenderTarget);

            FrenziedGame.Instance.GraphicsDevice.Clear(new Color(51, 51, 51));

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate*0.05f;

            this._spriteBatch.Begin();
            this._spriteBatch.Draw(this._menuTextureQuickPlay, new Vector2(100, 100), null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            this._spriteBatch.Draw(this._menuTextureCustomMode, new Vector2(100, 175), Color.White);
            this._spriteBatch.Draw(this._menuTextureTutorial, new Vector2(100, 250), Color.White);
            this._spriteBatch.Draw(this._menuTextureOptions, new Vector2(100, 325), Color.White);
            this._spriteBatch.Draw(this._menuTextureCredits, new Vector2(100, 400), Color.White);
            this._spriteBatch.End();

            //FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(null);

            //ApplyPostprocess();

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
            parameters["SketchThreshold"].SetValue(SketchSettings.Settings.SketchThreshold);
            parameters["SketchBrightness"].SetValue(SketchSettings.Settings.SketchBrightness);
                parameters["SketchJitter"].SetValue(sketchJitter);
                parameters["SketchTexture"].SetValue(sketchTexture);


            effectTechniqueName = "ColorSketch";

            // Activate the appropriate effect technique.
            postprocessEffect.CurrentTechnique = postprocessEffect.Techniques[effectTechniqueName];

            // Draw a fullscreen sprite to apply the postprocessing effect.
            _spriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, postprocessEffect);
            _spriteBatch.Draw(sceneRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }
    }
}
