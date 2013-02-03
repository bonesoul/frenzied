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
using Microsoft.Xna.Framework;

namespace Frenzied.GamePlay.GameModes.Implementations
{
    /// <summary>
    /// Blocked game mode.
    /// </summary>
    internal class BlockyMode : GameMode
    {
        public BlockyMode()
            : base()
        { }

        internal override void LoadContent()
        {
            var screenCenter = new Vector2(FrenziedGame.Instance.GraphicsDevice.Viewport.Bounds.Width/2,
                                           FrenziedGame.Instance.GraphicsDevice.Viewport.Bounds.Height/2);

            var topContainer = new BlockyContainer(new Vector2(screenCenter.X - BlockyContainer.Size.X,
                                                               screenCenter.Y - BlockyContainer.Size.Y));


            base.LoadContent();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
