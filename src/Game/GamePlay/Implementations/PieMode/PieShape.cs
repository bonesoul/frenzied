/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Frenzied.GamePlay.Modes;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.Implementations.Pie
{
    public class PieShape : Shape
    {
        public PieShape(byte colorIndex, byte locationIndex)
            : base(colorIndex, locationIndex)
        {
            this.Size = new Vector2(100, 100);
        }
    }
}
