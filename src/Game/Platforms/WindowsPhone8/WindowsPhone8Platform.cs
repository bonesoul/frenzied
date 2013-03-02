/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.Platforms.Config;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.WindowsPhone8
{
    public class WindowsPhone8Platform : PlatformHandler
    {
        public WindowsPhone8Platform()
        {
            this.Config = new PlatformConfig
            {
                Screen =
                {                    
                    Width = 1280,
                    Height = 720,
                    IsFullScreen = false,
                    SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
                },
                Input =
                {
                    IsMouseVisible = false,
                },
                Graphics =
                {

                    IsFixedTimeStep = false,
                    IsVsyncEnabled = false,
                    PostprocessEnabled = true,
                    ExtendedEffects = true,
                },
            };
        }
    }
}
