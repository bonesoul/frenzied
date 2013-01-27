/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

// original code by: http://nullcandy.com/2d-metaballs-in-xna/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frenzied.Graphics.Effects
{
    public class Metaball
    {
        public delegate Vector3 ColorPicker(float alpha, float innerGradient);

        private float Radius =
            FrenziedGame.Instance.Configuration.Background.MetaballRadius*
            FrenziedGame.Instance.Configuration.Background.MetaballScale;

        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public Color Glow;

        public void Update()
        {
            Position += Velocity;

            var viewport = FrenziedGame.Instance.GraphicsDevice.Viewport;
            if (Position.X > viewport.Width - Radius)
                Velocity.X = -Math.Abs(Velocity.X);
            if (Position.X < -Radius)
                Velocity.X = Math.Abs(Velocity.X);
            if (Position.Y > viewport.Height - Radius)
                Velocity.Y = -Math.Abs(Velocity.Y);
            if (Position.Y < -Radius)
                Velocity.Y = Math.Abs(Velocity.Y);
        }


        // static functions

        /// <summary>
        /// Creates the textures used for the metaballs.
        /// </summary>
        /// <param name="radius">Determines the distance at which the metaball has influence.</param>
        /// <param name="picker">A function that determines how to colour the metaball. Has no effect on their shape. It is purely aesthetic.</param>
        /// <returns>The texture used for making metabalss</returns>
        public static Texture2D GenerateTexture(int radius, ColorPicker picker)
        {
            int length = radius * 2;
            Color[] colors = new Color[length * length];

            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    float distance = Vector2.Distance(Vector2.One, new Vector2(x, y) / radius);

                    // this is the falloff function used to make the metaballs
                    float alpha = Falloff(distance);

                    // we'll use a smaller, inner gradient to colour the center of the metaballs a different colour. This is purely aesthetic.
                    float innerGradient = Falloff(distance, 0.6f, 0.8f);
                    colors[y * length + x] = new Color(picker(alpha, innerGradient));
                    colors[y * length + x].A = (byte)MathHelper.Clamp(alpha * 256f + 0.5f, 0f, 255f);
                }
            }

            Texture2D tex = new Texture2D(FrenziedGame.Instance.GraphicsDevice, radius * 2, radius * 2);
            tex.SetData(colors);
            return tex;
        }

        /// <summary>
        /// Colours the metaballs with a gradient between the two specified colours. For best result, the center colour
        /// should be brighter than the border colour.
        /// </summary>
        public static ColorPicker CreateTwoColorPicker(Color border, Color center)
        {
            return new ColorPicker((alpha, innerGradient) =>
                    Color.Lerp(border, center, innerGradient).ToVector3());
        }


        private static float Falloff(float distance)
        {
            return Falloff(distance, 1f, 1f);
        }

        /// <summary>
        /// The falloff function for the metaballs.
        /// </summary>
        /// <param name="distance">The distance from the center of the metaball.</param>
        /// <param name="maxDistance">How far before the function goes to zero.</param>
        /// <param name="scalingFactor">Multiplies the function by this value.</param>
        /// <returns>The metaball value at the given distance.</returns>
        private static float Falloff(float distance, float maxDistance, float scalingFactor)
        {
            if (distance <= maxDistance / 3)
            {
                return scalingFactor * (1 - 3 * distance * distance / (maxDistance * maxDistance));
            }
            else if (distance <= maxDistance)
            {
                float x = 1 - distance / maxDistance;
                return (3f / 2f) * scalingFactor * x * x;
            }
            else
                return 0;
        }
    }
}
