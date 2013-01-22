using Microsoft.Xna.Framework;

namespace Frenzied.Core.Graphics
{
    /// <summary>
    /// Screen service for controlling screen.
    /// </summary>
    public interface IGraphicsManager
    {
        /// <summary>
        /// Returns true if game is set to fixed time steps.
        /// </summary>
        bool IsFixedTimeStep { get; }

        /// <summary>
        /// Returns true if vertical sync is enabled.
        /// </summary>
        bool VerticalSyncEnabled { get; }

        /// <summary>
        /// Returns true if full-screen is enabled.
        /// </summary>
        bool FullScreenEnabled { get; }

        /// <summary>
        /// Toggles fixed time steps.
        /// </summary>
        void ToggleFixedTimeSteps();

        /// <summary>
        /// Toggles vertical sync.
        /// </summary>
        void ToggleVerticalSync();
    }

    /// <summary>
    /// The screen manager that controls various graphical aspects.
    /// </summary>
    public sealed class GraphicsManager : IGraphicsManager
    {
        // settings
        public bool IsFixedTimeStep { get; private set; } // Returns true if game is set to fixed time steps.
        public bool VerticalSyncEnabled { get; private set; } // Returns true if vertical sync is enabled.
        public bool FullScreenEnabled { get; private set; } // Returns true if full-screen is enabled.

        // principal stuff
        private readonly Game _game; // the attached game.
        private readonly GraphicsDeviceManager _graphicsDeviceManager; // attached graphics device manager.

        public GraphicsManager(GraphicsDeviceManager graphicsDeviceManager, Game game)
        {
            this._game = game;
            this._graphicsDeviceManager = graphicsDeviceManager;
            this._game.Services.AddService(typeof(IGraphicsManager), this); // export service.

            this.FullScreenEnabled = this._graphicsDeviceManager.IsFullScreen;
            this._graphicsDeviceManager.PreferredBackBufferWidth = this._game.GraphicsDevice.Viewport.Width;
            this._graphicsDeviceManager.PreferredBackBufferHeight = this._game.GraphicsDevice.Viewport.Height;
            this.IsFixedTimeStep = this._game.IsFixedTimeStep = false;
            this.VerticalSyncEnabled = this._graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            this._graphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Toggles fixed time steps.
        /// </summary>
        public void ToggleFixedTimeSteps()
        {
            this.IsFixedTimeStep = !this.IsFixedTimeStep;
            this._game.IsFixedTimeStep = this.IsFixedTimeStep;
            this._graphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Toggles vertical syncs.
        /// </summary>
        public void ToggleVerticalSync()
        {
            this.VerticalSyncEnabled = !this.VerticalSyncEnabled;
            this._graphicsDeviceManager.SynchronizeWithVerticalRetrace = this.VerticalSyncEnabled;
            this._graphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Sets full screen on or off.
        /// </summary>
        /// <param name="enabled"></param>
        public void EnableFullScreen(bool enabled)
        {
            this.FullScreenEnabled = enabled;
            this._graphicsDeviceManager.IsFullScreen = this.FullScreenEnabled;
            this._graphicsDeviceManager.ApplyChanges();
        }
    }
}