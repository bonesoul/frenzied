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
    /// Shape generator for game-modes.
    /// </summary>
    internal class ShapeGenerator:ShapeContainer
    {
        protected readonly List<ShapeContainer> Containers;

        protected readonly Random Randomizer = new Random(Environment.TickCount);

        private Shape _currentShape;

        public Shape CurrentShape
        {
            get { return this._currentShape; }
            protected set
            {
                this._currentShape = value;

                if (!value.IsEmpty)
                    this.Attach(value);
            }
        }

        public ShapeGenerator(Vector2 position, List<ShapeContainer> containers)
            : base(position)
        {
            this.Containers = containers;
        }

        public virtual void Generate()
        { }

        public virtual List<byte> GetAvailableLocations()
        {
            return null;
        }
    }
}
