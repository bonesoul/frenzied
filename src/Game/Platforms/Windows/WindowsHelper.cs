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

namespace Frenzied.Platforms.Windows
{
    public class WindowsHelper : PlatformHelper
    {
        public override void LaunchURI(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
    }
}
