/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Assets;
using Frenzied.Audio;
using Frenzied.Graphics;
using Frenzied.Input;
using Frenzied.Platforms;
using Frenzied.Screens;
using Frenzied.Screens.Implementations;
using Frenzied.Screens.Scenes;
using Frenzied.Utils.Debugging;
using Frenzied.Utils.Debugging.Graphs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Frenzied
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FrenziedGame : Game
    {
        /// <summary>
        /// Graphics device manager.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        private ScreenManager _screenManager;

        public FrenziedGame()
        {
            if (_instance != null)
                throw new Exception("Can not instantiate the game more than once!");

            _instance = this;

            GraphicsDeviceManager = new GraphicsDeviceManager(this);
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
            PlatformManager.Initialize(this, this.GraphicsDeviceManager);

            // init the asset manager.
            var assetManager = new AssetManager(this);
            this.Components.Add(assetManager);

            // create the screen manager
            this._screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            var backgroundScene = new BackgroundScene(this);

            // Activate the first screens.
            //this._screenManager.AddScreen(new BackgroundScreen(), null);
            this._screenManager.AddScreen(new MainScreen(), null);

            // add debug components
            this.Components.Add(new DebugBar(this));
            this.Components.Add(new GraphManager(this));

            // init the audio manager.
            var audioManager = new AudioManager(this);
            this.Components.Add(audioManager);

            if (PlatformManager.Handler.Config.Input.IsMouseVisible)
            {
                var cursor = new Cursor(this);
                this.Components.Add(cursor);
            }

            base.Initialize();
        }

        #if (DESKTOP || METRO) && DEBUG // check for global key-events.
        
        private KeyboardState _previousKeyboardState; // tracks previous keyboard state.

        protected override void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();

            if (_previousKeyboardState.IsKeyUp(Keys.F8) && currentState.IsKeyDown(Keys.F8))
                PlatformManager.Handler.Config.Debugger.ToggleBar();

            if (_previousKeyboardState.IsKeyUp(Keys.F9) && currentState.IsKeyDown(Keys.F9))
                PlatformManager.Handler.Config.Debugger.ToggleGraphs();

            this._previousKeyboardState = currentState;

            base.Update(gameTime);
        }

        #endif

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
