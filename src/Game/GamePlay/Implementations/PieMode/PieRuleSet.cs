/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System.Collections.Generic;
using Frenzied.GamePlay.Modes;

namespace Frenzied.GamePlay.Implementations.PieMode
{
    public class PieRuleSet : RuleSet
    {
        public PieRuleSet()
        {
            this.ShapePlacementTimeout = 5000;
            this.StartingLifes = 5;
            this.BonusLifeOnPerfectExplosion = 1;

            this.ScoreDictionary = new Dictionary<byte, int>()
                                       {
                                           {1, 10},
                                           {2, 25},
                                           {3, 50},
                                           {4, 100},
                                           {5, 250},
                                           {6, 1000},
                                       };

            this.ShapeColorsType = typeof (PieColors);
            this.ShapeLocationsType = typeof (PieLocations);
        }
    }
}
