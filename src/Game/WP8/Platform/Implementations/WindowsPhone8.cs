using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Frenzied.Platform.Implementations
{
    public class WindowsPhone8 : Platform
    {
        public WindowsPhone8()
        {
            this.PlatformConfig = new PlatformConfig()
                {
                    IsMouseVisible = false,
                    IsFixedTimeStep = false,
                };
        }

        public override void Initialize(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.GraphicsDeviceManager = graphicsDeviceManager;

            this.GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
            this.GraphicsDeviceManager.PreferredBackBufferHeight = 720;
            this.GraphicsDeviceManager.ApplyChanges();
        }
    }
}
