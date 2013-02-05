/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;

namespace Frenzied.Platforms
{
    public static class PlatformInfo
    {
        /// <summary>
        /// Game frameworks.
        /// </summary>
        public enum GameFrameworks
        {
            XNA,
            MonoGame
        }

        /// <summary>
        /// Graphics API's.
        /// </summary>
        public enum GraphicsAPI
        {
            DirectX9,
            DirectX11,
            OpenGL,
        }

        /// <summary>
        /// Platforms.
        /// </summary>
        public enum Platforms
        {
            Windows,
            WindowsMetro,
            WinPhone7,
            WinPhone8,
            Linux,
            MacOS,
            Android,
            IOS,
            PSP,
            Ouya,
        }

        public static Platforms Platform { get; private set; }

        public static string DotNetFramework { get; private set; }

        public static Version DotNetFrameworkVersion { get; private set; }

        public static GameFrameworks GameFramework { get; private set; }

        public static Version GameFrameworkVersion { get; private set; }

        public static GraphicsAPI GraphicsApi { get; private set; }

        static PlatformInfo()
        {
            #if METRO
                Platform = Platforms.WindowsMetro;
            #elif WINDOWS
                Platform = Platforms.Windows;
            #elif ANDROID
                Platform = Platforms.Android;
            #endif

            DotNetFramework = IsRunningOnMono() ? "Mono" : ".Net";

            #if METRO
                DotNetFrameworkVersion = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(Object)).Assembly.GetName().Version;
                GameFrameworkVersion = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof (Microsoft.Xna.Framework.Game)).Assembly.GetName().Version;
            #else
                DotNetFrameworkVersion = Environment.Version;
                #if WINPHONE7 || WINPHONE8
                    GameFrameworkVersion = new Version(typeof(Microsoft.Xna.Framework.Game).Assembly.FullName.Split(',')[1].Split('=')[1]);
                #else
                    GameFrameworkVersion = System.Reflection.Assembly.GetAssembly(typeof(Microsoft.Xna.Framework.Game)).GetName().Version;
                #endif
            #endif

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
