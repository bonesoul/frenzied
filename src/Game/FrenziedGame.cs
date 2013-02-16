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
using Frenzied.Input;
using Frenzied.Platforms;
using Frenzied.Screens;
using Frenzied.Screens.Implementations;
using Frenzied.Utils.Debugging;
using Frenzied.Utils.Debugging.Graphs;
using Microsoft.Xna.Framework;

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
            //var scoreManager = new ScoreManager(this);
            //this.Components.Add(scoreManager);

            // Activate the first screens.
            //this._screenManager.AddScreen(new AboutScreen(), null);
            //this._screenManager.AddScreen(new BackgroundScreen(), null);
            //this._screenManager.AddScreen(new MainMenuScreen2(), null);
            //this._screenManager.AddScreen(new GameplayScreen(new PieMode()), null);


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
