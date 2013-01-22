#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using Frenzied.Core.Assets;
using Frenzied.Core.Screen;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

#endregion

namespace Frenzied.Screens
{
    class BackgroundScreen : GameScreen
    {
        Rectangle safeArea;

        /// <summary>
        /// Initializes a new instance of the screen.
        /// </summary>
        public BackgroundScreen(Game game)
            :base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        #region Loading
        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public override void LoadContent()
        {
            safeArea = ScreenManager.Game.GraphicsDevice.Viewport.TitleSafeArea;
            base.LoadContent();
        }
        #endregion

        #region Update and Render
        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(AssetManager.Instance.BackgroundTexture, ScreenManager.Game.GraphicsDevice.Viewport.Bounds, Color.White * TransitionAlpha);
            ScreenManager.SpriteBatch.End();            

            base.Draw(gameTime);
        }
        #endregion
    }
}
