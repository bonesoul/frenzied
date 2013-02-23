/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.WindowsMetro
{
    public class WindowsMetroPlatform : PlatformHandler
    {
        public WindowsMetroPlatform()
        {
            this.PlatformConfig = new PlatformConfig()
            {
                IsMouseVisible = true,
                IsFixedTimeStep = false,
                Graphics = { ExtendedEffects = true },
            };
        }

        public override void PlatformEntrance()
        {
            var factory = new MonoGame.Framework.GameFrameworkViewSource<FrenziedGame>();
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
        }

        public override void Initialize(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.GraphicsDeviceManager = graphicsDeviceManager;
            //this.GraphicsDeviceManager.ApplyChanges();
        }
    }
}
