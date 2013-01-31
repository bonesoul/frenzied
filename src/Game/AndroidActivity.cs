using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Frenzied;
using Microsoft.Xna.Framework;

namespace AndroidGame1
{
    [Activity(
        Label = "Voxeliq",
        MainLauncher = true,
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // Watch for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            base.OnCreate(bundle);

            FrenziedGame.Activity = this;

            var g = new FrenziedGame();
            SetContentView(g.Window);
            g.Run();
        }

        // <summary>
        /// Unhandled exception handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">UnhandledExceptionEventArgs</param>
        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(
                e.IsTerminating
                    ? "Voxeliq terminating because of unhandled exception: "
                    : "Caught unhandled exception: ", ((Exception) e.ExceptionObject).StackTrace);
        }
    }
}

