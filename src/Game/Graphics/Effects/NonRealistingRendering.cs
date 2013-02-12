/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.Graphics.Effects
{
    /// <summary>
    /// Class holds all the settings used to tweak the non-photorealistic rendering.
    /// </summary>
    public class SketchSettings
    {
        #region Fields

        // Name of a preset setting, for display to the user.
        public readonly string Name;

        public readonly float SketchThreshold;
        public readonly float SketchBrightness;
        public readonly float SketchJitterSpeed;

        #endregion


        /// <summary>
        /// Constructs a new non-photorealistic settings descriptor.
        /// </summary>
        public SketchSettings(string name,                                          
                                         float sketchThreshold, float sketchBrightness,
                                         float sketchJitterSpeed)
        {
            Name = name;
            SketchThreshold = sketchThreshold;
            SketchBrightness = sketchBrightness;
            SketchJitterSpeed = sketchJitterSpeed;
        }

        public static SketchSettings Settings = new SketchSettings("Colored Hatching", 0.2f, 0.5f, 0.09f);
    }
}
