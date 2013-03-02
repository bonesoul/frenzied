/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.Config
{
    public class GameConfig
    {
        /// <summary>
        /// Holds the chunk configuration parameters.
        /// </summary>
        public BackgroundConfig Background { get; private set; }

        /// <summary>
        /// Holds debugger related configuration parameters.
        /// </summary>
        public DebuggerConfig Debugger { get; private set; }

        /// <summary>
        /// Creates a new instance of engine configuration.
        /// </summary>
        public GameConfig()
        {
            this.Background = new BackgroundConfig();
            this.Debugger = new DebuggerConfig();
        }
    }
}
