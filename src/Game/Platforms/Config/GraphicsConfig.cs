/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.Config
{
    public class GraphicsConfig
    {


        /// <summary>
        /// Gets or sets a value indicating whether to use fixed time steps.
        /// </summary>
        public bool IsFixedTimeStep { get; set; }

        /// <summary>
        /// Enables or disables vsync.
        /// </summary>
        public bool IsVsyncEnabled { get; set; }

        // Is post-processing enabled?
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
            // set defaults.
            this.IsFixedTimeStep = false;
            this.IsVsyncEnabled = false;
            this.PostprocessEnabled = true;
            this.ExtendedEffects = false;
        }
    }
}
