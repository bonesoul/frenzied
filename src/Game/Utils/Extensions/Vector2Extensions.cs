using System;
using Microsoft.Xna.Framework;

namespace Frenzied.Utils.Extensions
{
    static class Vector2Extensions
    {
        // Returns a random Vector2 with a length less than maxLength.
        public static Vector2 NextVector2(this Random rand, float maxLength)
        {
            double theta = rand.NextDouble() * 2 * Math.PI;
            float length = (float)rand.NextDouble() * maxLength;
            return new Vector2(length * (float)Math.Cos(theta), length * (float)Math.Sin(theta));
        }
    }
}
