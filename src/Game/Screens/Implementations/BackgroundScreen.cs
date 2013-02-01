/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

#region Using Statements

using System;
using System.Collections.Generic;
using Frenzied.Graphics.Effects;
using Frenzied.Utils.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Frenzied.Screens.Implementations
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        private ColorScheme _colorScheme;

        private Texture2D metaballTexture;
        private List<Metaball> balls = new List<Metaball>();
        private RenderTarget2D metaballTarget;
        private SpriteBatch spriteBatch;
        private AlphaTestEffect effect;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(FrenziedGame.Instance.GraphicsDevice);
            metaballTarget = new RenderTarget2D(FrenziedGame.Instance.GraphicsDevice, FrenziedGame.Instance.GraphicsDevice.Viewport.Width, FrenziedGame.Instance.GraphicsDevice.Viewport.Height);

            // initialize the alpha test effect.
            effect = new AlphaTestEffect(FrenziedGame.Instance.GraphicsDevice);
            var viewport = FrenziedGame.Instance.GraphicsDevice.Viewport;
            effect.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            effect.ReferenceAlpha = 256;

            // create a bunch of metaballs
            var rand = new Random();
            for (int i = 0; i < FrenziedGame.Instance.Configuration.Background.MetaballCount; i++)
            {
                var ball = new Metaball
                {
                    Position =
                        new Vector2(rand.Next(FrenziedGame.Instance.GraphicsDevice.Viewport.Width), rand.Next(FrenziedGame.Instance.GraphicsDevice.Viewport.Height)) -
                        new Vector2(128),
                    Velocity = rand.NextVector2(1)
                };
                balls.Add(ball);
            }

            var scheme = new ColorScheme
            {
                BackgroundColor = new Color(51, 51, 51),
                GlowColor = Color.DarkGray,
                GradientStartColor = Color.DarkGray,
                GradientEndColor = Color.Gray
            };
            this.ApplyColorScheme(scheme);
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
        }


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            foreach (var ball in balls)
                ball.Update();

            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // first we render the metaballs to a render target using additive blending
            FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(metaballTarget);
            FrenziedGame.Instance.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (var ball in balls)
                spriteBatch.Draw(ball.Texture, ball.Position, null, Color.White, 0f, Vector2.Zero, FrenziedGame.Instance.Configuration.Background.MetaballScale, SpriteEffects.None, 0f);
            spriteBatch.End();

            // now we render everything to the backbuffer
            FrenziedGame.Instance.GraphicsDevice.SetRenderTarget(null);
            FrenziedGame.Instance.GraphicsDevice.Clear(this._colorScheme.BackgroundColor);

            // draw a faint glow behind the metaballs. We accomplish this by rendering the 
            // metaball texture without threshholding it. This is purely aesthetic.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (var ball in balls)
            {
                var tint = new Color(ball.Glow.ToVector3() * 0.34f);
                spriteBatch.Draw(ball.Texture, ball.Position, null, tint, 0f, Vector2.Zero, FrenziedGame.Instance.Configuration.Background.MetaballScale, SpriteEffects.None, 0f);
            }
            spriteBatch.End();

            // now draw the metaball's render target to the screen using the AlphaTestEffect to threshhold it.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(metaballTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public void ApplyColorScheme(ColorScheme scheme)
        {
            this._colorScheme = scheme;

            metaballTexture = Metaball.GenerateTexture(
                FrenziedGame.Instance.Configuration.Background.MetaballRadius,
                Metaball.CreateTwoColorPicker(this._colorScheme.GradientStartColor, this._colorScheme.GradientEndColor));

            foreach (var ball in this.balls)
            {
                ball.Texture = metaballTexture;
                ball.Glow = this._colorScheme.GlowColor;
            }
        }

        public class ColorScheme
        {
            public Color BackgroundColor { get; set; }
            public Color GradientStartColor { get; set; }
            public Color GradientEndColor { get; set; }
            public Color GlowColor { get; set; }
        }
    }
}
