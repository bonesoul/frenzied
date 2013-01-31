/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using Frenzied.Graphics.Effects;
using Frenzied.Utils.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Components
{
    public class Background : DrawableGameComponent
    {
        private ColorScheme _colorScheme; 

        Texture2D metaballTexture;
        List<Metaball> balls = new List<Metaball>();
        RenderTarget2D metaballTarget;
        SpriteBatch spriteBatch;
        AlphaTestEffect effect;

        public Background(Game game)
            : base(game)
        { }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            metaballTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // initialize the alpha test effect.
            effect = new AlphaTestEffect(GraphicsDevice);
            var viewport = GraphicsDevice.Viewport;
            effect.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            effect.ReferenceAlpha = 256;            

            // create a bunch of metaballs
            var rand = new Random();
            for (int i = 0; i < FrenziedGame.Instance.Configuration.Background.MetaballCount; i++)
            {
                var ball = new Metaball
                    {
                        Position =
                            new Vector2(rand.Next(GraphicsDevice.Viewport.Width), rand.Next(GraphicsDevice.Viewport.Height)) -
                            new Vector2(128),
                        Velocity = rand.NextVector2(1)
                    };
                balls.Add(ball);
            }

            var scheme = new ColorScheme
                {
                    BackgroundColor = new Color(51,51,51),
                    GlowColor = Color.DarkGray,
                    GradientStartColor = Color.DarkGray,
                    GradientEndColor = Color.Gray
                };
            this.ApplyColorScheme(scheme);
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


            // some good combinations;
            // generate some differently coloured metaball textures. You could tint them with SpriteBatch instead, 
            // but then you couldn't have a two colour gradient.
            // metaballTextures = new Texture2D[5];
            //metaballTextures[0] = Metaball.GenerateTexture(FrenziedGame.Instance.Configuration.Background.MetaballRadius, Metaball.CreateTwoColorPicker(Color.Red, Color.Yellow));
            //metaballTextures[1] = Metaball.GenerateTexture(FrenziedGame.Instance.Configuration.Background.MetaballRadius, Metaball.CreateTwoColorPicker(Color.Blue, Color.Cyan));
            //metaballTextures[2] = Metaball.GenerateTexture(FrenziedGame.Instance.Configuration.Background.MetaballRadius, Metaball.CreateTwoColorPicker(Color.Green, Color.Yellow));
            //metaballTextures[3] = Metaball.GenerateTexture(FrenziedGame.Instance.Configuration.Background.MetaballRadius, Metaball.CreateTwoColorPicker(Color.Magenta, Color.White));
            //metaballTextures[4] = Metaball.GenerateTexture(FrenziedGame.Instance.Configuration.Background.MetaballRadius, Metaball.CreateTwoColorPicker(Color.Red, Color.Red));
            //glowColors = new Color[] { Color.Red, Color.Blue, Color.Lime, Color.Magenta, Color.Red };
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var ball in balls)
                ball.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            // first we render the metaballs to a render target using additive blending
            GraphicsDevice.SetRenderTarget(metaballTarget);
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (var ball in balls)
                spriteBatch.Draw(ball.Texture, ball.Position, null, Color.White, 0f, Vector2.Zero, FrenziedGame.Instance.Configuration.Background.MetaballScale, SpriteEffects.None, 0f);
            spriteBatch.End();

            // now we render everything to the backbuffer
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(this._colorScheme.BackgroundColor);

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

        public class ColorScheme
        {
            public Color BackgroundColor { get; set; }
            public Color GradientStartColor { get; set; }
            public Color GradientEndColor { get; set; }
            public Color GlowColor { get; set; }
        }
    }
}
