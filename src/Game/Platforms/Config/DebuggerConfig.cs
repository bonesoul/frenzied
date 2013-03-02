/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.Platforms.Config
{
    public class DebuggerConfig
    {
        /// <summary>
        /// Gets or sets if debug-bar is enabled.
        /// </summary>
        public bool BarEnabled { get; set; }

        /// <summary>
        /// Gets or sets if debug-graphs are enabled.
        /// </summary>
        public bool GraphsEnabled { get; set; }

        /// <summary>
        /// Creates a new instance of debugger-config.
        /// </summary>
        public DebuggerConfig()
        {
            #if DEBUG
                this.BarEnabled = true;
                this.GraphsEnabled = true;
            #else
                this.BarEnabled = false;
                this.GraphsEnabled = false;
            #endif
        }

        public void ToggleBar()
        {
            this.BarEnabled = !this.BarEnabled;
        }

        public void ToggleGraphs()
        {
            this.GraphsEnabled = !this.GraphsEnabled;
        }
    }
}
