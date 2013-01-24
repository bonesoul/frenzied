/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Microsoft.Xna.Framework;

namespace Frenzied.Core.GamePlay
{
    public enum BlockColor
    {
        None,
        Orange,
        Purple,
        Green
    }

    public enum BlockLocation
    {
        none,
        topleft,
        topright,
        bottomleft,
        bottomright
    }

    public class Block
    {
        public const int PositionOffset = 5;

        public static Vector2 Size = new Vector2(100, 100);

        public BlockColor Color { get; private set; }
        public BlockLocation Location { get; private set; }

        public BlockContainer ParentContainer { get; private set; }

        public Vector2 Position { get; private set; }
        public Rectangle Bounds { get; private set; }
        public bool IsEmpty { get; private set; }

        public Block(BlockLocation location, BlockColor color)
        {
            this.Location = location;
            this.Color = color;
            this.IsEmpty = false;
        }

        public Block(BlockLocation location)
        {
            this.Location = location;
            this.IsEmpty = true;
        }

        public void AttachTo(BlockContainer container)
        {
            this.ParentContainer = container;

            switch (this.Location)
            {
                case BlockLocation.topleft:
                    this.Position = new Vector2(this.ParentContainer.Position.X + PositionOffset,
                                                this.ParentContainer.Position.Y + PositionOffset);
                    break;
                case BlockLocation.topright:
                    this.Position = new Vector2(this.ParentContainer.Position.X + Size.X + PositionOffset,
                                                this.ParentContainer.Position.Y + PositionOffset);
                    break;
                case BlockLocation.bottomleft:
                    this.Position = new Vector2(this.ParentContainer.Position.X + PositionOffset,
                                                this.ParentContainer.Position.Y + Size.Y + PositionOffset);
                    break;
                case BlockLocation.bottomright:
                    this.Position = new Vector2(this.ParentContainer.Position.X + Size.X + PositionOffset,
                                                this.ParentContainer.Position.Y + Size.Y + PositionOffset);
                    break;
                default:
                    break;
            }
            this.Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Size.X, (int)Size.Y);
        }
    }
}

