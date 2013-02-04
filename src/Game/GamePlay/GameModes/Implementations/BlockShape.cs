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

namespace Frenzied.GamePlay.GameModes.Implementations
{
    internal class BlockShape : Shape
    {
        public BlockShape()
            : this(ShapeColor.None, ShapeLocation.None)
        { }

        public BlockShape(byte colorIndex)
            : this(colorIndex, ShapeLocation.None)
        { }

        public BlockShape(byte colorIndex, byte locationIndex)
            : base(colorIndex, locationIndex)
        {
            this.Size = new Vector2(100, 100);
        }

        public override void AttachTo(ShapeContainer container)
        {
            this.Parent = container;

            if (this.LocationIndex == ShapeLocation.None)
                return;

            switch (this.LocationIndex)
            {
                case BlockLocations.TopLeft:
                    this.Position = new Vector2(this.Parent.Position.X, this.Parent.Position.Y);
                    break;
                case BlockLocations.TopRight:
                    this.Position = new Vector2(this.Parent.Position.X + this.Size.X, this.Parent.Position.Y);
                    break;
                case BlockLocations.BottomRight:
                    this.Position = new Vector2(this.Parent.Position.X + this.Size.X, this.Parent.Position.Y + this.Size.Y);
                    break;
                case BlockLocations.BottomLeft:
                    this.Position = new Vector2(this.Parent.Position.X, this.Parent.Position.Y + this.Size.Y);
                    break;
            }

            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Size.X, (int)Size.Y);
        }
    }
}
