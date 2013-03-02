/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.Platforms.Config
{
    public class PlatformConfig
    {
        /// <summary>
        /// Screen config.
        /// </summary>
        public ScreenConfig Screen { get; private set; }

        /// <summary>
        /// Graphics config.
        /// </summary>
        public GraphicsConfig Graphics { get; private set; }

        /// <summary>
        /// Input config.
        /// </summary>
        public InputConfig Input { get; private set; }

        /// <summary>
        /// Debugger config.
        /// </summary>
        public DebuggerConfig Debugger { get; private set; }

        /// <summary>
        /// Creates a new instance of platform-config.
        /// </summary>
        public PlatformConfig()
        {
            // init. sub-configs.
            this.Screen = new ScreenConfig();
            this.Graphics = new GraphicsConfig();
            this.Input = new InputConfig();
            this.Debugger = new DebuggerConfig();
        }
    }
}
