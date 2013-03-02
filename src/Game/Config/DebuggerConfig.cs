using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frenzied.Config
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
