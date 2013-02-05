/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Platforms.Android;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms
{
    public class PlatformManager
    {
        public Platform CurrentPlatform { get; private set; }

        private PlatformManager()
        {
            #if XNA && DESKTOP
                this.CurrentPlatform = new Windows.WindowsPlatform();
            #elif WINPHONE7
                this.CurrentPlatform = new WindowsPhone7.WindowsPhone7Platform();
            #elif WINPHONE8
                this.CurrentPlatform = new WindowsPhone8.WindowsPhone8();
            #elif ANDROID
                this.CurrentPlatform = new Android.AndroidPlatform();
            #endif

            if (this.CurrentPlatform == null)
                throw new Exception("Unsupported platform!");
        }

        public void Startup()
        {
            this.CurrentPlatform.PlatformEntrance();
        }

        public void Initialize(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.CurrentPlatform.Initialize(graphicsDeviceManager);
        }

        private static PlatformManager _instance = new PlatformManager(); // the memory instance.

        /// <summary>
        /// Returns the memory instance of Engine.
        /// </summary>
        public static PlatformManager Instance
        {
            get { return _instance; }
        }
    }
}
