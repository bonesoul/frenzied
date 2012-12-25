using Frenzied.Core.Assets;
using Frenzied.Core.Audio;
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
        GraphicsDeviceManager _graphics;

        private ScreenManager _screenManager;

        public FrenziedGame()
        {
            _graphics = new GraphicsDeviceManager(this);
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

            // init the audio manager.
            var audioManager = new AudioManager(this);
            this.Components.Add(audioManager);

            // create the screen manager
            this._screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            // add the background screen to the screen manager
            this._screenManager.AddScreen(new BackgroundScreen(this));
            this._screenManager.AddScreen(new GamePlayScreen(this));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
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
            // TODO: Add your update logic here

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
