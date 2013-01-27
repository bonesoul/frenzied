/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.Utils.Extensions
{
    public static class NumberExtensions
    {
        public static string GetKiloString(this int value)
        {
            int i;
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            double dblSByte = 0;
            for (i = 0; (int)(value / 1024) > 0; i++, value /= 1024) dblSByte = value / 1024.0;
            return dblSByte.ToString("0.00") + suffixes[i];
        }

        public static string GetKiloString(this long value)
        {
            int i;
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            double dblSByte = 0;
            for (i = 0; (int)(value / 1024) > 0; i++, value /= 1024) dblSByte = value / 1024.0;
            return dblSByte.ToString("0.00") + suffixes[i];
        }
    }
}
