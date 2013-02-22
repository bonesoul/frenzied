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
using Microsoft.Phone.Tasks;

namespace Frenzied.Platforms.WindowsPhone7
{
    public class WindowsPhone7Helper : PlatformHelper
    {
        public override void LaunchURI(string url)
        {
            var task = new WebBrowserTask {Uri = new Uri(url)};
            task.Show();
        }
    }
}
