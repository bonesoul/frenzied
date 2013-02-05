/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Frenzied.Assets;
using Frenzied.GamePlay;
using Frenzied.GamePlay.Modes;
using Frenzied.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Frenzied.Screens.Implementations
{
    class GameplayScreen : GameScreen
    {
        float pauseAlpha;

        // required services.
        private IScoreManager _scoreManager;
        private IAssetManager _assetManager;

        private GameMode _gameMode;

        /// <summary>
        /// Creates a new <see cref="GameplayScreen"/> instance.
        /// </summary>
        public GameplayScreen(GameMode gameMode)
        {
            this._gameMode = gameMode;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.EnabledGestures = GestureType.Tap;
        }

        // TODO: implement initialize for gamescreens!

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            this._gameMode.LoadContent();

            this._scoreManager = (IScoreManager)FrenziedGame.Instance.Services.GetService(typeof(IScoreManager));
            this._assetManager = (IAssetManager)FrenziedGame.Instance.Services.GetService(typeof(IAssetManager));

            if (this._scoreManager == null)
                throw new NullReferenceException("Can not find score manager component.");

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

            this._gameMode.HandleClick(input.CurrentMouseState.X, input.CurrentMouseState.Y);
        }

        public override void HandleGestures(InputState input)
        {
            if (input.Gestures.Count == 0)
                return;

            foreach (var gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                    this._gameMode.HandleClick((int)gesture.Position.X, (int)gesture.Position.Y);
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
            this._gameMode.Update(gameTime);

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
            this._gameMode.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
