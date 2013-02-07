﻿/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System.Collections.Generic;

namespace Frenzied.GamePlay.Modes
{
    /// <summary>
    /// Defines shape color.
    /// </summary>
    public class ShapeColors
    {
        /// <summary>
        /// No color assigned (empty).
        /// </summary>
        public const byte None = 0;

        /// <summary>
        /// Returns the enumerator for values in shape-colors list.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<byte> GetEnumerator()
        {
            yield return None;
        }

        /// <summary>
        /// Returns the array of shape-colors list.
        /// </summary>
        /// <returns></returns>
        public static byte[] ToArray()
        {
            return new[] {None};
        }
    }
}
