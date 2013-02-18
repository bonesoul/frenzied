/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms
{
    public static class PlatformManager
    {
        public static Platforms Platform { get; private set; }

        public static string DotNetFramework { get; private set; }

        public static Version DotNetFrameworkVersion { get; private set; }

        public static GameFrameworks GameFramework { get; private set; }

        public static Version GameFrameworkVersion { get; private set; }

        public static GraphicsAPI GraphicsApi { get; private set; }

        public static PlatformHandler PlatformHandler { get; private set; }

        public static PlatformHelper PlatformHelper { get; private set; }

        static PlatformManager()
        {
            IdentifyPlatform();
        }

        public static void Startup()
        {
            PlatformHandler.PlatformEntrance();
        }

        public static void Initialize(GraphicsDeviceManager graphicsDeviceManager)
        {
            PlatformHandler.Initialize(graphicsDeviceManager);
        }

        private static void IdentifyPlatform()
        {
            // find base platform.
            #if WINDOWS && DESKTOP
                Platform = Platforms.Windows;
                PlatformHandler = new Windows.WindowsPlatform();
                PlatformHelper = new Windows.WindowsHelper();
            #elif WINDOWS && METRO
                Platform = Platforms.WindowsMetro;
                PlatformHandler = new WindowsMetro.WindowsMetroPlatform();
			#elif LINUX && DESKTOP
				Platform = Platforms.Linux;
				PlatformHandler = new Linux.LinuxPlatform();
			#elif MACOS && DESKTOP
				Platform=Platforms.MacOS;
				PlatformHandler = new MacOS.MacOSPlatform();
            #elif WINPHONE7
                Platform = Platforms.WindowsPhone7;
                PlatformHandler = new WindowsPhone7.WindowsPhone7Platform();
            #elif WINPHONE8
                Platform = Platforms.WindowsPhone8;
                PlatformHandler = new WindowsPhone8.WindowsPhone8Platform();
            #elif ANDROID
                Platform = Platforms.Android;
                PlatformHandler = new Android.AndroidPlatform();
			#elif IOS
				Platform=Platforms.IOS;
				PlatformHandler=new IOS.IOSPlatform();
            #endif

            if (PlatformHandler == null)
                throw new Exception("Unsupported platform!");

            // find dot.net framework.
            DotNetFramework = IsRunningOnMono() ? "Mono" : ".Net";

            // find dot.net framework and game framework version.
            #if METRO
                DotNetFrameworkVersion = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(Object)).Assembly.GetName().Version;
                GameFrameworkVersion = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(Microsoft.Xna.Framework.Game)).Assembly.GetName().Version;
            #else
                DotNetFrameworkVersion = Environment.Version;
                #if WINPHONE7 || WINPHONE8
                    GameFrameworkVersion = new Version(typeof(Microsoft.Xna.Framework.Game).Assembly.FullName.Split(',')[1].Split('=')[1]);
                #else
                    GameFrameworkVersion = System.Reflection.Assembly.GetAssembly(typeof(Microsoft.Xna.Framework.Game)).GetName().Version;
                #endif
            #endif

            // find game framework & graphics-api.
            #if XNA
                GameFramework = GameFrameworks.XNA;
                GraphicsApi = GraphicsAPI.DirectX9;
            #elif MONOGAME
                GameFramework = GameFrameworks.MonoGame;
                #if DIRECTX11
                    GraphicsApi = GraphicsAPI.DirectX11;
                #elif OPENGL
                    GraphicsApi = GraphicsAPI.OpenGL;
                #endif
            #endif
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
