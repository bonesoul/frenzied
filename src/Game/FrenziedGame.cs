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
using Frenzied.Graphics;
using Frenzied.Input;
using Frenzied.Screens;
using Frenzied.Screens.Implementations;
using Frenzied.Utils.Debugging;
using Frenzied.Utils.Debugging.Graphs;
using Microsoft.Xna.Framework;

#if METRO || ANDROID
using Microsoft.Xna.Framework.Input.Touch;
#endif

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
            this.IsMouseVisible = true;

            #if DESKTOP
            this._graphicsDeviceManager.PreferredBackBufferWidth = 1280;
            this._graphicsDeviceManager.PreferredBackBufferHeight = 720;
            this._graphicsDeviceManager.ApplyChanges();
            #endif

            #if METRO || ANDROID
            TouchPanel.EnabledGestures = GestureType.Tap;
            #endif

            #if ANDROID
            this._graphicsDeviceManager.PreferredBackBufferWidth = 800;
            this._graphicsDeviceManager.PreferredBackBufferHeight = 480;
            this._graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            TouchPanel.EnabledGestures = GestureType.Tap;
            #endif

            // init the asset manager.
            var assetManager = new AssetManager(this);
            this.Components.Add(assetManager);

            var graphicsManager = new GraphicsManager(this._graphicsDeviceManager, this); // start the screen manager.
            graphicsManager.ToggleVerticalSync();

            //var background = new Background(this);
            //this.Components.Add(background);

            // create the screen manager
            this._screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            // create the score manager
            var scoreManager = new ScoreManager(this);
            this.Components.Add(scoreManager);

            // Activate the first screens.
            this._screenManager.AddScreen(new BackgroundScreen(), null);
            this._screenManager.AddScreen(new MainMenuScreen(), null);


            // add debug components
            this.Components.Add(new DebugBar(this));
            this.Components.Add(new GraphManager(this));

            // init the audio manager.
            var audioManager = new AudioManager(this);
            this.Components.Add(audioManager);

            var cursor = new Cursor(this);
            this.Components.Add(cursor);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
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
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
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
