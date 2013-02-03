/*
 * Copyright (C) 2011 - 2013 Int6 Studios - http://www.int6.org,
 * Voxeliq Engine - http://www.voxeliq.org - https://github.com/raistlinthewiz/voxeliq
 *
 * This program is free software; you can redistribute it and/or modify 
 * it under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Frenzied.Utils.Platform
{
    /// <summary>
    /// Version info.
    /// </summary>
    public static class VersionInfo
    {
            /// <summary>
            /// Main assemblies version pattern.
            /// </summary>
            public const string VersionPattern = "0.1.0.*";

            /// <summary>
            /// Main assemblies version.
            /// </summary>
            public static string Version { get; private set; }

            static VersionInfo()
            {
                #if METRO
                    Version = string.Format("{0}.{1}.{2}.{3}", Windows.ApplicationModel.Package.Current.Id.Version.Major,
                                        Windows.ApplicationModel.Package.Current.Id.Version.Minor,
                                        Windows.ApplicationModel.Package.Current.Id.Version.Build,
                                        Windows.ApplicationModel.Package.Current.Id.Version.Revision);
                #elif WINPHONE7
                    Version = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
                #else
                    Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                #endif
            }
    }
}
