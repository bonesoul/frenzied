/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.Modes
{
    /// <summary>
    /// Defines a shape container.
    /// </summary>
    public class ShapeContainer : IContainer
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
        /// Contained shapes dictionary.
        /// </summary>
        private readonly Dictionary<byte, Shape> _shapes;

        /// <summary>
        /// Shape indexer for contained shapes.
        /// </summary>
        /// <param name="index">The LocationIndex of the shape.</param>
        /// <returns><see cref="Shape"/></returns>
        public Shape this[byte index]
        {
            get { return this._shapes.ContainsKey(index) ? this._shapes[index] : Shape.Empty; }
            set
            {
                this._shapes[index] = value;

                if (value.IsEmpty) 
                    return;

                if(value.Parent != null)
                    value.Parent.Detach(value);

                this.Attach(value);
            }
        }

        /// <summary>
        /// Creates a new shape container at given coordinates.
        /// </summary>
        /// <param name="position">The shape position.</param>
        public ShapeContainer(Vector2 position)
        {
            this.Position = position;
            this._shapes = new Dictionary<byte, Shape>();
        }

        /// <summary>
        /// Initializes the shape-container.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Loads content for shape-container.
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Attachs a shape to ShapeContainer.
        /// </summary>
        /// <param name="shape"><see cref="Shape"/></param>
        public virtual void Attach(Shape shape)
        { }

        /// <summary>
        /// Detachs a shape from ShapeContainer.
        /// </summary>
        /// <param name="shape"><see cref="Shape"/></param>
        public virtual void Detach(Shape shape)
        { }

        /// <summary>
        /// Returns shape enumurator for the container.
        /// </summary>
        /// <returns><see cref="Shape"/> enumurator.</returns>
        public virtual IEnumerable<Shape> GetEnumerator()
        {
            return null;
        }

        /// <summary>
        /// Is container empty?
        /// </summary>
        /// <returns>Returns true if container is empty, false otherwise.</returns>
        public virtual bool IsEmpty()
        {
            return true;
        }

        /// <summary>
        /// Is shape et given LocationIndex empty?
        /// </summary>
        /// <param name="locationIndex">The LocationIndex of the shape to query.</param>
        /// <returns>Returns true if shape at given LocationIndex is empty, false otherwise.</returns>
        public virtual bool IsEmpty(byte locationIndex)
        {
            return true;
        }
        
        /// <summary>
        /// Is container full?
        /// </summary>
        /// <returns>Returns true if containers is full, false otherwise.</returns>
        public virtual bool IsFull()
        {
            return false;
        }

        /// <summary>
        /// Explodes shape's in the container.
        /// </summary>
        public virtual void Explode()
        { }

        /// <summary>
        /// Updates container.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void Update(GameTime gameTime)
        { }

        /// <summary>
        /// Draws the container and contained shapes.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void Draw(GameTime gameTime)
        { }
    }
}
