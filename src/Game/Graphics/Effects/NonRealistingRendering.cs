#region File Description
//-----------------------------------------------------------------------------
// NonPhotoRealisticSettings.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

namespace Frenzied.Graphics.Effects
{
    /// <summary>
    /// Class holds all the settings used to tweak the non-photorealistic rendering.
    /// </summary>
    public class NonPhotoRealisticSettings
    {
        #region Fields

        // Name of a preset setting, for display to the user.
        public readonly string Name;

        // Settings for the pencil sketch effect.
        public readonly bool EnableSketch;
        public readonly float SketchThreshold;
        public readonly float SketchBrightness;
        public readonly float SketchJitterSpeed;

        #endregion


        /// <summary>
        /// Constructs a new non-photorealistic settings descriptor.
        /// </summary>
        public NonPhotoRealisticSettings(string name, 
                                         bool enableSketch, 
                                         float sketchThreshold, float sketchBrightness,
                                         float sketchJitterSpeed)
        {
            Name = name;
            EnableSketch = enableSketch;
            SketchThreshold = sketchThreshold;
            SketchBrightness = sketchBrightness;
            SketchJitterSpeed = sketchJitterSpeed;
        }


        /// <summary>
        /// Table of preset settings, used by the sample program.
        /// </summary>
        public static NonPhotoRealisticSettings[] PresetSettings =
            {
                new NonPhotoRealisticSettings("Colored Hatching",
                                              true,  0.2f, 0.5f, 0.075f),
            };
    }
}
