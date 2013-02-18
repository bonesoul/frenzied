/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.WindowsPhone8
{
    public class WindowsPhone8Platform : PlatformHandler
    {
        public WindowsPhone8Platform()
        {
            this.PlatformConfig = new PlatformConfig()
            {
                IsMouseVisible = false,
                IsFixedTimeStep = false,
                Graphics = { CustomShadersEnabled = false },
            };
        }

        public override void Initialize(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.GraphicsDeviceManager = graphicsDeviceManager;

            this.GraphicsDeviceManager.IsFullScreen = true;
            this.GraphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            this.GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
            this.GraphicsDeviceManager.PreferredBackBufferHeight = 720;
            this.GraphicsDeviceManager.ApplyChanges();
        }
    }
}
