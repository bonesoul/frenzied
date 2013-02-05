using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.Android
{
    public class AndroidPlatform : PlatformHandler
    {
        public AndroidPlatform()
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

            this.GraphicsDeviceManager.PreferredBackBufferWidth = 800;
            this.GraphicsDeviceManager.PreferredBackBufferHeight = 480;
            this.GraphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            this.GraphicsDeviceManager.ApplyChanges();
        }
    }
}