/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.Platforms.Config;
using Microsoft.Xna.Framework.Input;

namespace Frenzied.Platforms.WindowsMetro
{
    public class WindowsMetroPlatform : PlatformHandler
    {
        public WindowsMetroPlatform()
        {
            this.Config = new PlatformConfig
            {
                Screen =
                {
                    IsFullScreen = false,
                },
                Input =
                {
                    IsMouseVisible = true,
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

        public override void PlatformEntrance()
        {
            var factory = new MonoGame.Framework.GameFrameworkViewSource<FrenziedGame>();
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
        }
    }
}
