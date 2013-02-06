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
    /// Game mode class that defines a game-mode.
    /// </summary>
    internal abstract class GameMode
    {
        /// <summary>
        /// The shape containers.
        /// </summary>
        protected readonly List<ShapeContainer> ShapeContainers;

        /// <summary>
        /// The shape generator.
        /// </summary>
        protected ShapeGenerator ShapeGenerator;

        /// <summary>
        /// Creates a new GameMode instance.
        /// </summary>
        protected GameMode()
        {
            this.ShapeContainers = new List<ShapeContainer>();
        }

        /// <summary>
        /// Loads content for game-mode.
        /// </summary>
        internal virtual void LoadContent()
        { }

        /// <summary>
        /// Handles click.
        /// </summary>
        /// <param name="X">The x position of the cursor.</param>
        /// <param name="Y">The y position of the cursor.</param>
        internal virtual void HandleClick(int X, int Y)
        { }

        /// <summary>
        /// Updates game-mode.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        internal virtual void Update(GameTime gameTime)
        {
            // update containers.
            foreach (var container in this.ShapeContainers)
            {
                container.Update(gameTime);
            }

            // update shape generator.
            this.ShapeGenerator.Update(gameTime);
        }

        /// <summary>
        /// Draws game-mode.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        internal virtual void Draw(GameTime gameTime)
        {
            // draw containers.
            foreach (var container in this.ShapeContainers)
            {
                container.Draw(gameTime);
            }

            // draw generator.
            this.ShapeGenerator.Draw(gameTime);
        }
    }
}
