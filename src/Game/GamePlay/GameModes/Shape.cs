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
using Microsoft.Xna.Framework;

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
        /// The position of the shape.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The bounds of the shape.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Size of the shape container.
        /// </summary>
        public Vector2 Size { get; protected set; }

        /// <summary>
        /// Parent shape container of the shape.
        /// </summary>
        public IContainer Parent { get; set; }

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

        public static Shape Empty { get; protected set; }

        static Shape()
        {
            Empty = new Shape(ShapeColors.None, ShapeLocations.None);
        }

        /// <summary>
        /// Is the shape empty - in other words; non-colored?
        /// </summary>
        public bool IsEmpty
        {
            get { return this.ColorIndex == ShapeColors.None; }
        }
    }
}
