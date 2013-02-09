/*
 * Frenzied Game, Copyright (C) 2012 - 2013 Int6 Studios - All Rights Reserved. - http://www.int6.org
 *
 * This file is part of Frenzied Game project. Unauthorized copying of this file, via any medium is strictly prohibited.
 * Frenzied Gam or its components/sources can not be copied and/or distributed without the express permission of Int6 Studios.
 */

using System;

namespace Frenzied.Utils.Services
{
    /// <summary>
    /// Exposes helper methods for game-services.
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Queries supplied service and returns if it's found.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="service">Service type.</param>
        /// <returns>The service.</returns>
        public static T GetService<T>(Type service)
        {
            var ret = (T) FrenziedGame.Instance.Services.GetService(service);
            if (ret == null)
                throw new NullReferenceException(string.Format("Can not find {0} service!", service));

            return ret;
        }
    }
}
