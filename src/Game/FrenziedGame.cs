/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.Common.Debug;
using Frenzied.Common.Debug.Graphs;
using Frenzied.Core.Assets;
using Frenzied.Core.Audio;
using Frenzied.Core.GamePlay;
using Frenzied.Core.Graphics;
using Frenzied.Core.Input;
using Frenzied.Core.Screen;
using Frenzied.Screens;
using Microsoft.Xna.Framework;

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
        GraphicsDeviceManager _graphicsDeviceManager;

        private ScreenManager _screenManager;

        public FrenziedGame()
        {
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

            // add the background screen to the screen manager
            //this._screenManager.AddScreen(new BackgroundScreen(this));
            //this._screenManager.AddScreen(new GamePlayScreen(this));
            this._screenManager.AddScreen(new MenuScreen(this));

            // add debug components
            this.Components.Add(new Statistics(this));
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
            GraphicsDevice.Clear(Color.SteelBlue);

            base.Draw(gameTime);
        }
    }
}
