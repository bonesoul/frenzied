/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.Platforms.Config;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms
{
    public class PlatformHandler
    {
        public PlatformConfig Config;

        protected Game Game { get; private set; }
        protected GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        public virtual void PlatformEntrance() 
        { }

        /// <summary>
        /// Initializes game & graphics based on config.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="graphicsDeviceManager"></param>
        public void Initialize(Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.Game = game;
            this.GraphicsDeviceManager = graphicsDeviceManager;

            // set custom resolution if required
            if (this.Config.Screen.Width != 0 && this.Config.Screen.Height != 0)
            {
                this.GraphicsDeviceManager.PreferredBackBufferWidth = this.Config.Screen.Width;
                this.GraphicsDeviceManager.PreferredBackBufferHeight = this.Config.Screen.Height;
            }

            // set full screen mode.
            this.GraphicsDeviceManager.IsFullScreen = this.Config.Screen.IsFullScreen;

            // set vsync.
            this.Game.IsFixedTimeStep = this.Config.Graphics.IsFixedTimeStep;
            this.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = this.Config.Graphics.IsVsyncEnabled;

            // set mouse mode.
            this.Game.IsMouseVisible = this.Config.Input.IsMouseVisible;

            // set orientation.
            this.GraphicsDeviceManager.SupportedOrientations = this.Config.Screen.SupportedOrientations;

            this.Initialize();

            this.GraphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Override to include platform specific initilization code.
        /// </summary>
        public virtual void Initialize()
        { }
    }
}
