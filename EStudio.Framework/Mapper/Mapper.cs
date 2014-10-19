using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EStudio.Framework.Mapper
{
    public static class Mapper
    {
        /// <summary>
        /// The mapper core instance.
        /// </summary>
        private static readonly MapperCore MapperInstance;

        /// <summary>
        /// Initializes static members of the <see cref="Mapper"/> class. 
        /// </summary>
        static Mapper()
        {
            MapperInstance = new MapperCore();
        }
        public static MapperCore DataMapper
        {
            get { return MapperInstance; }
        }
        public static TTo Map<TFrom, TTo>(TFrom @from)
        {
            return MapperInstance.Map<TFrom, TTo>(@from);
        }

        public static TTo Map<TFrom, TTo>(TFrom @from, TTo @to)
        {
            return MapperInstance.Map(@from, @to);
        }

        public static IEnumerable<TTo> MapCollection<TFrom, TTo>(IEnumerable<TFrom> @from)
        {
            return MapperInstance.MapCollection<TFrom, TTo>(@from);
        }
    }

}
