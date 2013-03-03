/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using Frenzied.Platforms.Config;
using Microsoft.Xna.Framework;

namespace Frenzied.Platforms.IOS
{
	public class IOSPlatform : PlatformHandler 
	{
		public IOSPlatform()
		{
            this.Config = new PlatformConfig
            {
                Screen =
                {
                    Width = 480,
                    Height = 800,
                    IsFullScreen = true,
                    SupportedOrientations = DisplayOrientation.FaceDown | DisplayOrientation.FaceUp,
                },
                Input =
                {
                    IsMouseVisible = false,
                },
                Graphics =
                {
                    IsFixedTimeStep = false,
                    IsVsyncEnabled = false,
                    PostprocessEnabled = true,
                    ExtendedEffects = false,
                },
            };
		}
		
		public override void PlatformEntrance()
		{
			FrenziedGame game;

			game = new FrenziedGame ();
			game.Run ();
		}
	}
}

