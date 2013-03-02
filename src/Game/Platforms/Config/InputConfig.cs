using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frenzied.Platforms.Config
{
    public class InputConfig
    {
        /// <summary>
        /// Gets or sets if mouse is visible for the platform.
        /// </summary>
        public bool IsMouseVisible { get; set; }

        public InputConfig()
        {
            // set the defaults.
            this.IsMouseVisible = false;
        }
    }
}
