/*
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
        protected readonly List<ShapeContainer> ShapeContainers;

        protected ShapeGenerator ShapeGenerator;

        protected GameMode()
        {
            this.ShapeContainers = new List<ShapeContainer>();
        }

        internal virtual void LoadContent()
        { }

        internal virtual void HandleClick(int X, int Y)
        { }

        internal virtual void Update(GameTime gameTime)
        {
            foreach (var container in this.ShapeContainers)
            {
                container.Update(gameTime);
            }

            this.ShapeGenerator.Update(gameTime);
        }

        internal virtual void Draw(GameTime gameTime)
        {
            foreach (var container in this.ShapeContainers)
            {
                container.Draw(gameTime);
            }

            this.ShapeGenerator.Draw(gameTime);
        }
    }
}
