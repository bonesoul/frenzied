/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.GameModes
{
    /// <summary>
    /// Defines a shape container.
    /// </summary>
    internal class ShapeContainer
    {
        /// <summary>
        /// The position of the shape container.
        /// </summary>
        public Vector2 Position { get; protected set; }

        /// <summary>
        /// The bounds of the shape container.
        /// </summary>
        public Rectangle Bounds { get; protected set; }

        private readonly Dictionary<byte, Shape> _shapes;

        public Shape this[byte index]
        {
            get { return this._shapes[index]; }
            set
            {
                this._shapes[index] = value;
                value.AttachTo(this);
            }
        }

        public ShapeContainer(Vector2 position)
        {
            this.Position = position;
            this._shapes = new Dictionary<byte, Shape>();
        }

        public virtual IEnumerable<Shape> GetEnumerator()
        {
            return null;
        }

        public virtual bool IsEmpty()
        {
            return true;
        }


        public virtual bool IsEmpty(byte locationIndex)
        {
            return true;
        }

        public virtual bool IsFull()
        {
            return false;
        }

        public virtual void Update(GameTime gameTime)
        { }

        public virtual void Draw(GameTime gameTime)
        { }
    }
}
