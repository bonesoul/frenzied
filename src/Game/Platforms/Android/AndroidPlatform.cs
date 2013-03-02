/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Frenzied.Platforms.Config;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.Android
{
    public class AndroidPlatform : PlatformHandler
    {
        public AndroidPlatform()
        {
			this.Config = new PlatformConfig
			{
				Screen =
				{
					Width = 800,
					Height = 480,
					IsFullScreen = true,
					SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
				},
				Input =
				{
					IsMouseVisible = false,
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
    }
}