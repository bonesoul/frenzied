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
using Frenzied.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Screens.Implementations
{
    public class AboutScreen : GameScreen
    {
        Texture2D gradientTexture;

        public AboutScreen()
            : base()
        { }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>(@"Textures\Gradient");
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            spriteBatch.Begin();


            spriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
