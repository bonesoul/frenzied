using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.WindowsPhone7
{
    public class WindowsPhone7Platform : Platform
    {
        public WindowsPhone7Platform()
        {
            this.PlatformConfig = new PlatformConfig()
            {
                IsMouseVisible = false,
                IsFixedTimeStep = false,
            };
        }

        public override void PlatformEntrance()
        {
            using (var game = new FrenziedGame())
            {
                game.Run();
            }
        }

        public override void Initialize(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.GraphicsDeviceManager = graphicsDeviceManager;

            this.GraphicsDeviceManager.PreferredBackBufferWidth = 480;
            this.GraphicsDeviceManager.PreferredBackBufferHeight = 800;
            this.GraphicsDeviceManager.IsFullScreen = true;
            this.GraphicsDeviceManager.ApplyChanges();
        }
    }
}
