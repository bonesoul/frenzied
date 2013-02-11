/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System.Collections.Generic;
using Frenzied.Assets;
using Frenzied.Utils.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.GamePlay.Modes
{
    public interface IGameMode
    {
        /// <summary>
        /// Returns a texture for a given shape.
        /// </summary>
        /// <param name="shape">The shape to query for.</param>
        /// <returns><see cref="Shape"/></returns>
        Texture2D GetShapeTexture(Shape shape);

        /// <summary>
        /// The rule-set for the game-mode.
        /// </summary>
        RuleSet RuleSet { get; }
    }

    /// <summary>
    /// Game mode class that defines a game-mode.
    /// </summary>
    public abstract class GameMode : IGameMode
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
        /// The rule-set for the game-mode.
        /// </summary>
        public RuleSet RuleSet { get; protected set; }

        // required services.       
        private IScoreManager _scoreManager;

        /// <summary>
        /// Creates a new GameMode instance.
        /// </summary>
        protected GameMode()
        {
            this.RuleSet = new RuleSet();
            this.ShapeContainers = new List<ShapeContainer>();
        }

        /// <summary>
        /// Initializes the game-mode.
        /// </summary>
        public virtual void Initialize()
        {
            // import required services.
            this._scoreManager = ServiceHelper.GetService<IScoreManager>(typeof(IScoreManager));

            // initialize generator.
            this.ShapeGenerator.Initialize();

            // initialize containers.
            foreach (var container in this.ShapeContainers)
            {
                container.Initialize();
            }
        }

        /// <summary>
        /// Loads content for game-mode.
        /// </summary>
        public virtual void LoadContent()
        {
            // load-content for generator.
            this.ShapeGenerator.LoadContent();

            // load-content for containers.
            foreach (var container in this.ShapeContainers)
            {
                container.LoadContent();
            }
        }

        /// <summary>
        /// Handles click.
        /// </summary>
        /// <param name="X">The x position of the cursor.</param>
        /// <param name="Y">The y position of the cursor.</param>
        public void HandleClick(GameTime gameTime, int X, int Y)
        {
            if (this.ShapeGenerator.IsEmpty())
                return;

            foreach (var container in this.ShapeContainers)
            {
                if (!container.Bounds.Contains(X, Y))
                    continue;

                if (!container.IsEmpty(this.ShapeGenerator.CurrentShape.LocationIndex))
                    continue;

                container[this.ShapeGenerator.CurrentShape.LocationIndex] = this.ShapeGenerator.CurrentShape;
                this._scoreManager.MoveCommitted(gameTime);
                break;
            }
        }

        public virtual Texture2D GetShapeTexture(Shape shape)
        {
            return null;
        }

        /// <summary>
        /// Updates game-mode.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void Update(GameTime gameTime)
        {
            // update shape generator.
            this.ShapeGenerator.Update(gameTime);

            // update containers.
            foreach (var container in this.ShapeContainers)
            {
                container.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws game-mode.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public virtual void Draw(GameTime gameTime)
        {
            // draw generator.
            this.ShapeGenerator.Draw(gameTime);

            // draw containers.
            foreach (var container in this.ShapeContainers)
            {
                container.Draw(gameTime);
            }
        }
    }
}
