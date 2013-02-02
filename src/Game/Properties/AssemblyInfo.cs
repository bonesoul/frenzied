using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

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
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: ComVisible(false)]

#if ANDROID
// Add some common permissions, these can be removed if not needed
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
#endif

#if WINPHONE7
[assembly: NeutralResourcesLanguageAttribute("en-US")]

// On Windows, the following GUID is for the ID of the typelib if this
// project is exposed to COM. On other platforms, it unique identifies the
// title storage container when deploying this assembly to the device.
[assembly: Guid("868741df-2e75-4934-aff5-82b3ba5445c5")]
#endif