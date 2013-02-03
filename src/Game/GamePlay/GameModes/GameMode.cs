﻿/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.GameModes
{
    /// <summary>
    /// Game mode class that defines a game-mode.
    /// </summary>
    internal abstract class GameMode
    {
        protected List<ShapeContainer> ShapeContainers;

        protected GameMode()
        {
            this.ShapeContainers = new List<ShapeContainer>();
        }

        internal virtual void LoadContent()
        { }

        internal virtual void Update(GameTime gameTime) 
        { }

        internal virtual void Draw(GameTime gameTime)
        { }
    }
}
