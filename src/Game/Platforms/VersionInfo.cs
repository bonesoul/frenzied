/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.Platforms
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
