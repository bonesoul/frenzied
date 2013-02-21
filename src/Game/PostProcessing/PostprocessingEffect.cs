/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.PostProcessing
{
    public class PostprocessingEffect
    {
        protected Game Game;
        protected GraphicsDevice GraphicsDevice;
        protected SpriteBatch SpriteBatch;

        public PostprocessingEffect(Game game, SpriteBatch spriteBatch)
        {
            this.Game = game;
            this.GraphicsDevice = game.GraphicsDevice;
            this.SpriteBatch = spriteBatch;
        }

        public virtual Texture2D Apply(Texture2D input, GameTime gameTime)
        {
            return input;
        }
    }
}
