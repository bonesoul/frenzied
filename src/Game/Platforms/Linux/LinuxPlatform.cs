/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Microsoft.Xna.Framework;
using Frenzied.Platforms.Config;

namespace Frenzied.Platforms.Linux
{
	public class LinuxPlatform : PlatformHandler 
	{
        public LinuxPlatform()
        {
            this.Config = new PlatformConfig
                    {
                        Screen =
                        {
                            IsFullScreen = false,
                            Width = 1280,
                            Height = 720,
                        },
                        Input =
                        {
                            IsMouseVisible = true,
                        },
                        Graphics =
                        {

                            IsFixedTimeStep = false,
                            IsVsyncEnabled = false,
                            PostprocessEnabled = false,
                            ExtendedEffects = true,
                        },
                    };
        }

        public override void PlatformEntrance()
        {
            using (var game = new FrenziedGame())
            {
                game.Run();
            }
        }
	}
}

