/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using Frenzied.Assets;
using Frenzied.GamePlay;
using Frenzied.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Frenzied.Screens.Implementations
{
    class GameplayScreen : GameScreen
    {
        private List<BlockContainer> _blockContainers = new List<BlockContainer>();
        private BlockGenerator _blockGenerator;

        float pauseAlpha;

        // required services.
        private IScoreManager _scoreManager;
        private IAssetManager _assetManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.EnabledGestures = GestureType.Tap;
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            this._scoreManager = (IScoreManager)FrenziedGame.Instance.Services.GetService(typeof(IScoreManager));
            this._assetManager = (IAssetManager)FrenziedGame.Instance.Services.GetService(typeof(IAssetManager));

            if (this._scoreManager == null)
                throw new NullReferenceException("Can not find score manager component.");

            var midScreenX = FrenziedGame.Instance.GraphicsDevice.Viewport.Bounds.Width / 2;
            var midScreenY = FrenziedGame.Instance.GraphicsDevice.Viewport.Bounds.Height / 2;


            // load block containers
            var offset = 100;

            this._blockContainers.Add(new BlockContainer(FrenziedGame.Instance, new Vector2(midScreenX - BlockContainer.Size.X / 2, midScreenY - BlockContainer.Size.Y - offset)));
            this._blockContainers.Add(new BlockContainer(FrenziedGame.Instance, new Vector2(midScreenX - BlockContainer.Size.X - offset, midScreenY - offset)));
            this._blockContainers.Add(new BlockContainer(FrenziedGame.Instance, new Vector2(midScreenX + offset, midScreenY - offset)));
            this._blockContainers.Add(new BlockContainer(FrenziedGame.Instance, new Vector2(midScreenX - BlockContainer.Size.X / 2, midScreenY + BlockContainer.Size.Y - offset)));

            this._blockGenerator = new BlockGenerator(FrenziedGame.Instance, new Vector2(midScreenX - BlockContainer.Size.X / 2, midScreenY - offset), this._blockContainers);
            this._blockGenerator.Initialize();

            base.LoadContent();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

            if (input.CurrentMouseState.LeftButton != ButtonState.Pressed || input.LastMouseState.LeftButton != ButtonState.Released)
                return;

            this.HandleClick(input.CurrentMouseState.X, input.CurrentMouseState.Y);
        }

        public override void HandleGestures()
        {
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Tap)
                    HandleClick((int)gesture.Position.X, (int)gesture.Position.Y);

            }
        }

        private void HandleClick(int X, int Y)
        {
            foreach (var container in this._blockContainers)
            {
                if (!container.Bounds.Contains(X, Y))
                    continue;

                if (this._blockGenerator.IsEmpty)
                    continue;

                if (!container.IsEmpty(this._blockGenerator.CurretBlock.Location))
                    continue;

                this._assetManager.Sounds.BlockEffect.PlayRandom();

                container.AddBlock(this._blockGenerator.CurretBlock);
                this._blockGenerator.CurrentBlockUsed();

                break;
            }
        }

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            this._blockGenerator.Update(gameTime);

            foreach (var container in this._blockContainers)
            {
                container.Update(gameTime);
            }

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (var container in this._blockContainers)
            {
                container.Draw(gameTime);
            }

            this._blockGenerator.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
