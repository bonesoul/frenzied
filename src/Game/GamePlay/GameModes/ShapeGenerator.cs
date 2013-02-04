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
    internal class ShapeGenerator : IContainer
    {
        /// <summary>
        /// The position of the shape container.
        /// </summary>
        public Vector2 Position { get; protected set; }

        /// <summary>
        /// The bounds of the shape container.
        /// </summary>
        public Rectangle Bounds { get; protected set; }

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
        {
            this.Position = position;
            this.Containers = containers;
        }

        public virtual bool IsEmpty()
        {
            return true;
        }

        public virtual void Attach(Shape shape)
        { }

        public virtual void Detach(Shape shape)
        { }

        public virtual void Generate()
        { }

        public virtual List<byte> GetAvailableLocations()
        {
            return null;
        }

        public virtual void Update(GameTime gameTime)
        { }

        public virtual void Draw(GameTime gameTime)
        { }
    }
}
