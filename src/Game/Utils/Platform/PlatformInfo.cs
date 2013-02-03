/*
 * Copyright (C) 2011 - 2013 Int6 Studios - http://www.int6.org,
 * Voxeliq Engine - http://www.voxeliq.org - https://github.com/raistlinthewiz/voxeliq
 *
 * This program is free software; you can redistribute it and/or modify 
 * it under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Frenzied.Utils.Platform
{
    public static class PlatformInfo
    {
        public enum GameFrameworks
        {
            XNA,
            MonoGame
        }

        public enum GraphicsAPI
        {
            DirectX9,
            DirectX11,
            OpenGL,
        }

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
