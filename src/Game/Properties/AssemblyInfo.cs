/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;
using Frenzied.Utils.Platform;

#if ANDROID
using Android.App;
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Frenzied")]
[assembly: AssemblyProduct("Frenzied")]
[assembly: AssemblyDescription("Frenzied")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Int6 Studios")]
[assembly: AssemblyCopyright("Copyright ©  2012 - 2013, Int6 Studios.")]
[assembly: AssemblyTrademark("Int6 Studios")]
[assembly: AssemblyCulture("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//

// Set the assembly version from VersionInfo.cs file.
[assembly: AssemblyVersion(VersionInfo.VersionPattern)]
[assembly: ComVisible(false)]

#if ANDROID
// Add some common permissions, these can be removed if not needed
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
#endif

#if WINPHONE7 || WINPHONE8
[assembly: NeutralResourcesLanguageAttribute("en-US")]
#endif

#if WINPHONE7
// On Windows, the following GUID is for the ID of the typelib if this
// project is exposed to COM. On other platforms, it unique identifies the
// title storage container when deploying this assembly to the device.
[assembly: Guid("868741df-2e75-4934-aff5-82b3ba5445c5")]
#endif

#if WINPHONE8
// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("884d4b41-7b23-496c-ba72-db2c249b074e")]
#endif