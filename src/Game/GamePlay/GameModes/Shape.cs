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

namespace Frenzied.GamePlay.GameModes
{
    /// <summary>
    /// Defines a basic shape for a game mode.
    /// </summary>
    internal class Shape
    {
        /// <summary>
        /// The color of the shape.
        /// </summary>
        public byte ColorIndex { get; private set; }

        /// <summary>
        /// The location index of the shape in container.
        /// </summary>
        public byte LocationIndex { get; private set; }

        /// <summary>
        /// Creates a new instance of the shape.
        /// </summary>
        /// <param name="colorIndex">The color index of the shape.</param>
        /// <param name="locationIndex">The location index of the shape.</param>
        public Shape(byte colorIndex, byte locationIndex)
        {
            this.ColorIndex = colorIndex;
            this.LocationIndex = locationIndex;
        }

        /// <summary>
        /// Is the shape empty - in other words; non-colored?
        /// </summary>
        public bool IsEmpty
        {
            get { return this.ColorIndex == ShapeColor.None; }
        }

        /// <summary>
        /// Defines shape color.
        /// </summary>
        internal class ShapeColor
        {
            public const byte None = 0;
        }

        /// <summary>
        /// Defines the location of the shape.
        /// </summary>
        internal class ShapeLocation
        {
            public const byte None = 0;
        }
    }
}
