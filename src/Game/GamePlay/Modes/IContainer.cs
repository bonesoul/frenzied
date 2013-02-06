/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

namespace Frenzied.GamePlay.Modes
{
    /// <summary>
    /// IContainer that a shape can be attached/detached.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Attachs a shape to IContainer.
        /// </summary>
        /// <param name="shape"><see cref="Shape"/></param>
        void Attach(Shape shape);

        /// <summary>
        /// Detachs a shape from IContainer.
        /// </summary>
        /// <param name="shape"><see cref="Shape"/></param>
        void Detach(Shape shape);
    }
}
