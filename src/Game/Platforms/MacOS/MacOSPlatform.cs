/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Microsoft.Xna.Framework;
using MonoMac.AppKit;
using MonoMac.Foundation;
using Frenzied.Platforms.Config;

namespace Frenzied.Platforms.MacOS
{
	public class MacOSPlatform : PlatformHandler 
	{
		public MacOSPlatform()
		{            
			this.Config = new PlatformConfig
			{
				Screen =
				{
					IsFullScreen = false,
					Width = 1280,
					Height = 720,
				},
				Input =
				{
					IsMouseVisible = true,
				},
				Graphics =
				{
					
					IsFixedTimeStep = false,
					IsVsyncEnabled = false,
					PostprocessEnabled = false,
					ExtendedEffects = true,
				},
			};
		}
		
		public override void PlatformEntrance()
		{
			NSApplication.Init ();
			
			using (var p = new NSAutoreleasePool ()) {
				NSApplication.SharedApplication.Delegate = new AppDelegate ();
				NSApplication.Main (null);
			}
		}
	}

	class AppDelegate : NSApplicationDelegate
	{
		FrenziedGame game;
		
		public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
		{
			game = new FrenziedGame ();
			game.Run ();
		}
		
		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}  
}

