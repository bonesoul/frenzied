/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Assets;
using Frenzied.Audio;
using Frenzied.Config;
using Frenzied.GamePlay;
using Frenzied.GamePlay.Implementations.PieMode;
using Frenzied.Graphics;
using Frenzied.Graphics.Effects;
using Frenzied.Input;
using Frenzied.Platforms;
using Frenzied.Screens;
using Frenzied.Screens.Implementations;
using Frenzied.Utils.Debugging;
using Frenzied.Utils.Debugging.Graphs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FrenziedGame : Game
    {
        /// <summary>
        /// The game configuration.
        /// </summary>
        public GameConfig Configuration { get; private set; }

        /// <summary>
        /// Graphics device manager.
        /// </summary>
        GraphicsDeviceManager _graphicsDeviceManager;

        private ScreenManager _screenManager;

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
        RenderTarget2D sceneRenderTarget;
        RenderTarget2D normalDepthRenderTarget;

        // Choose what display settings to use.
        NonPhotoRealisticSettings Settings
        {
            get { return NonPhotoRealisticSettings.PresetSettings[settingsIndex]; }
        }

        int settingsIndex = 1;

        public FrenziedGame()
        {
            if (_instance != null)
                throw new Exception("Can not instantiate the game more than once!");

            _instance = this;

            var config = new GameConfig
                {
                    Background =
                        {
                            MetaballCount = 50,
                            MetaballRadius = 128,
                            MetaballScale = 1f
                        },
                };

            this.Configuration = config;

            #if WINPHONE7
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            #endif

            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";         
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // set platform specific stuff.
            this.IsMouseVisible = PlatformManager.PlatformHandler.PlatformConfig.IsMouseVisible;
            this.IsFixedTimeStep = PlatformManager.PlatformHandler.PlatformConfig.IsFixedTimeStep;

            PlatformManager.Initialize(this._graphicsDeviceManager);        

            // init the asset manager.
            var assetManager = new AssetManager(this);
            this.Components.Add(assetManager);

            var graphicsManager = new GraphicsManager(this._graphicsDeviceManager, this); // start the screen manager.
            graphicsManager.ToggleVerticalSync();


            // create the screen manager
            this._screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            // create the score manager
            var scoreManager = new ScoreManager(this);
            this.Components.Add(scoreManager);

            // Activate the first screens.
            //this._screenManager.AddScreen(new BackgroundScreen(), null);
            //this._screenManager.AddScreen(new MainMenuScreen(), null);
            this._screenManager.AddScreen(new GameplayScreen(new PieMode()), null);

            // add debug components
            this.Components.Add(new DebugBar(this));
            this.Components.Add(new GraphManager(this));

            // init the audio manager.
            var audioManager = new AudioManager(this);
            this.Components.Add(audioManager);

            if (PlatformManager.PlatformHandler.PlatformConfig.IsMouseVisible)
            {
                var cursor = new Cursor(this);
                this.Components.Add(cursor);
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.GraphicsDevice);

            postprocessEffect = Content.Load<Effect>(@"Effects\PostprocessEffect");
            sketchTexture = Content.Load<Texture2D>(@"Effects\SketchTexture");

            // Change the model to use our custom cartoon shading effect.
            Effect cartoonEffect = Content.Load<Effect>(@"Effects\CartoonEffect");

            // Create two custom rendertargets.
            PresentationParameters pp = this.GraphicsDevice.PresentationParameters;

            sceneRenderTarget = new RenderTarget2D(this.GraphicsDevice,
                                                   pp.BackBufferWidth, pp.BackBufferHeight, false,
                                                   pp.BackBufferFormat, pp.DepthStencilFormat);

            normalDepthRenderTarget = new RenderTarget2D(this.GraphicsDevice,
                                                         pp.BackBufferWidth, pp.BackBufferHeight, false,
                                                         pp.BackBufferFormat, pp.DepthStencilFormat);

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (Settings.EnableEdgeDetect || Settings.EnableSketch)
                this.GraphicsDevice.SetRenderTarget(sceneRenderTarget);
            else
                this.GraphicsDevice.SetRenderTarget(null);

            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Run the postprocessing filter over the scene that we just rendered.
            if (Settings.EnableEdgeDetect || Settings.EnableSketch)
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

            // Set effect parameters controlling the edge detection effect.
            if (Settings.EnableEdgeDetect)
            {
                Vector2 resolution = new Vector2(sceneRenderTarget.Width,
                                                 sceneRenderTarget.Height);

                Texture2D normalDepthTexture = normalDepthRenderTarget;

                parameters["EdgeWidth"].SetValue(Settings.EdgeWidth);
                parameters["EdgeIntensity"].SetValue(Settings.EdgeIntensity);
                parameters["ScreenResolution"].SetValue(resolution);
                parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

                // Choose which effect technique to use.
                if (Settings.EnableSketch)
                {
                    if (Settings.SketchInColor)
                        effectTechniqueName = "EdgeDetectColorSketch";
                    else
                        effectTechniqueName = "EdgeDetectMonoSketch";
                }
                else
                    effectTechniqueName = "EdgeDetect";
            }
            else
            {
                // If edge detection is off, just pick one of the sketch techniques.
                if (Settings.SketchInColor)
                    effectTechniqueName = "ColorSketch";
                else
                    effectTechniqueName = "MonoSketch";
            }

            // Activate the appropriate effect technique.
            postprocessEffect.CurrentTechnique = postprocessEffect.Techniques[effectTechniqueName];

            // Draw a fullscreen sprite to apply the postprocessing effect.
            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, postprocessEffect);
            spriteBatch.Draw(sceneRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private static FrenziedGame _instance; // the memory instance.

        /// <summary>
        /// Returns the memory instance of Engine.
        /// </summary>
        public static FrenziedGame Instance
        {
            get { return _instance; }
        }
    }
}
