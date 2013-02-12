/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Frenzied.GamePlay.Implementations.PieMode;
using Frenzied.Platforms;

namespace Frenzied.GamePlay.Modes
{
    public class RuleSet
    {
        /// <summary>
        /// Number of subshapes to construct a complete container.
        /// </summary>
        public int SubShapeCount { get; protected set; }

        /// <summary>
        /// Timeout value in miliseconds for placing the last generated shape.
        /// </summary>
        public int ShapePlacementTimeout { get; protected set; }

        /// <summary>
        /// Starting life.
        /// </summary>
        public int StartingLifes { get; protected set; }

        /// <summary>
        /// Extra provided life on perfect explosion.
        /// </summary>
        public int BonusLifeOnPerfectExplosion { get; protected set; }

        /// <summary>
        /// The associated game mode colors type.
        /// </summary>
        public Type ShapeColorsType { get; protected set; }

        /// <summary>
        /// The associated game mode shapes type.
        /// </summary>
        public Type ShapeLocationsType { get; protected set; }

        /// <summary>
        /// Dictionary of scores for SameColorCount keys.
        /// </summary>
        protected Dictionary<byte, int> ScoreDictionary; 

        public RuleSet()
        {
            // set the defaults for ruleset.
            this.SubShapeCount = 4;
            this.StartingLifes = 5;
            this.BonusLifeOnPerfectExplosion = 1;
            this.ShapePlacementTimeout = 5000;
            this.ShapeColorsType = typeof (ShapeColors);
            this.ShapeLocationsType = typeof (ShapeLocations);
        }

        /// <summary>
        /// Calculates the score on a container explosion.
        /// </summary>
        /// <param name="container"></param>
        public  int CalculateExplosionScore(ShapeContainer container, out bool isPerfect)
        {
            // call associated ShapeColor type's color value's enumerator.
            #if !METRO
                var colorIndexes = (IEnumerable<byte>)this.ShapeColorsType.GetMethod("GetEnumerator").Invoke(null, null);
            #else
                var colorIndexes = (IEnumerable<byte>) this.ShapeColorsType.GetRuntimeMethod("GetEnumerator", null).Invoke(null, null);
            #endif

            // create a list of dictionary that holds colorIndex => colorUsageCount.
            var colorUsages = colorIndexes.ToDictionary<byte, byte, byte>(colorIndex => colorIndex, colorIndex => 0);

            foreach (var shape in container.GetEnumerator())
            {
                colorUsages[shape.ColorIndex]++;
            }

            byte maximumUsedColorsIndex = 1;

            foreach (var pair in colorUsages)
            {
                if (pair.Value > colorUsages[maximumUsedColorsIndex])
                    maximumUsedColorsIndex = pair.Key;
            }

            isPerfect = colorUsages[maximumUsedColorsIndex] == this.SubShapeCount;

            return this.ScoreDictionary[colorUsages[maximumUsedColorsIndex]];
        }
    }
}
