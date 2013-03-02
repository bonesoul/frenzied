/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.Config
{
    public class ScreenConfig
    {
        /// <summary>
        /// Screen width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Screen height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Is full screen enabled?
        /// </summary>
        public bool IsFullScreen { get; set; }

        /// <summary>
        /// Supported orientations.
        /// </summary>
        public DisplayOrientation SupportedOrientations { get; set; }

        public ScreenConfig()
        {
            // set defaults.
            this.Width = 0; // make default resolution 0x0, so that it'll be only set if a platform requires so and sets the values - otherwise the system default will be used.
            this.Height = 0;
            this.IsFullScreen = false;
            this.SupportedOrientations = DisplayOrientation.Default;
        }
    }
}
