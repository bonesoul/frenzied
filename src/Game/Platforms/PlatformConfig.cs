/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.Platforms
{
    public class PlatformConfig
    {
        /// <summary>
        /// Gets or sets if mouse is visible for the platform.
        /// </summary>
        public bool IsMouseVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use fixed time steps.
        /// </summary>
        public bool IsFixedTimeStep { get; set; }

        public GraphicsConfig Graphics { get; private set; }

        /// <summary>
        /// Creates a new instance of platform-config.
        /// </summary>
        public PlatformConfig()
        {
            // init. sub-configs.
            this.Graphics = new GraphicsConfig();

            // set the defaults.
            this.IsMouseVisible = false;
            this.IsFixedTimeStep = false;
        }
    }
}
