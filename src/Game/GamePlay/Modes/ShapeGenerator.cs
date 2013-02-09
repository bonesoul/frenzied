/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.Modes
{
    /// <summary>
    /// Shape generator for game-modes.
    /// </summary>
    public class ShapeGenerator : IContainer
    {
        /// <summary>
        /// The position of the shape container.
        /// </summary>
        public Vector2 Position { get; protected set; }

        /// <summary>
        /// The bounds of the shape container.
        /// </summary>
        public Rectangle Bounds { get; protected set; }

        /// <summary>
        /// The shape containers in associated game-mode.
        /// </summary>
        protected readonly List<ShapeContainer> Containers;

        /// <summary>
        /// Randomizer.
        /// </summary>
        protected readonly Random Randomizer = new Random(Environment.TickCount);

        private Shape _currentShape; // latest generated shape.

        /// <summary>
        /// The latest generated shape.
        /// </summary>
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

        /// <summary>
        /// Creates a new shape generator instance.
        /// </summary>
        /// <param name="position">The position of the generator.</param>
        /// <param name="containers">The shape containers in associated game mode.</param>
        public ShapeGenerator(Vector2 position, List<ShapeContainer> containers)
        {
            this.Position = position;
            this.Containers = containers;
        }

        /// <summary>
        /// Initializes the shape-generator.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Loads content for shape-generator.
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Is container empty?
        /// </summary>
        /// <returns>Returns true if container is empty, false otherwise.</returns>
        public virtual bool IsEmpty()
        {
            return true;
        }

        /// <summary>
        /// Attachs a shape to ShapeGenerator.
        /// </summary>
        /// <param name="shape"><see cref="Shape"/></param>
        public virtual void Attach(Shape shape)
        { }

        /// <summary>
        /// Detachs a shape from ShapeGenerator.
        /// </summary>
        /// <param name="shape"><see cref="Shape"/></param>
        public virtual void Detach(Shape shape)
        { }

        /// <summary>
        /// Generates a new shape.
        /// </summary>
        public virtual void Generate()
        { }

        /// <summary>
        /// Gets available locations for generating a shape.
        /// </summary>
        /// <returns>The list of available locations.</returns>
        public virtual List<byte> GetAvailableLocations()
        {
            return new List<byte>();
        }

        /// <summary>
        /// Updates generator.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void Update(GameTime gameTime)
        { }

        /// <summary>
        /// Draws the shape generator.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void Draw(GameTime gameTime)
        { }
    }
}
