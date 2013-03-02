/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.Platforms.Config
{
    public class GraphicsConfig
    {
        /// <summary>
        /// Should we set a custom resolution?
        /// </summary>
        public bool CustomResolutionEnabled { get; set; }

        /// <summary>
        /// Screen width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Screen height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use fixed time steps.
        /// </summary>
        public bool IsFixedTimeStep { get; set; }

        /// <summary>
        /// Enables or disables vsync.
        /// </summary>
        public bool IsVsyncEnabled { get; set; }

        /// <summary>
        /// Is full screen enabled?
        /// </summary>
        public bool IsFullScreen { get; set; }

        public bool PostprocessEnabled { get; set; }

        /// <summary>
        /// Gets or sets if custom shaders are enabled for platform.
        /// </summary>
        public bool ExtendedEffects { get; set; }
        
        /// <summary>
        /// Creates a new instance of graphics-config.
        /// </summary>
        public GraphicsConfig()
        {
            this.CustomResolutionEnabled = true;
            this.Width = 1024;
            this.Height = 768;
            this.IsFullScreen = false;
            this.IsFixedTimeStep = false;
            this.IsVsyncEnabled = false;
            this.PostprocessEnabled = true;
            this.ExtendedEffects = false;
        }
    }
}
