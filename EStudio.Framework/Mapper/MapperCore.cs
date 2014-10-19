using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EmitMapper;
using EmitMapper.MappingConfiguration;

namespace EStudio.Framework.Mapper
{
    /// <summary>
    /// The mapper core.
    /// </summary>
    public class MapperCore
    {
        /// <summary>
        /// The default configuration.
        /// </summary>
        private static readonly IMappingConfigurator DefaultConfigurator;

        /// <summary>
        /// The list of mappers.
        /// </summary>
        private static readonly ConcurrentBag<object> Mappers;

        /// <summary>
        /// The list of configurations. 
        /// </summary>
        private static readonly ConcurrentBag<Tuple<Type, Type, IMappingConfigurator>> MappingConfigurations;

        /// <summary>
        /// Initializes the <see cref="MapperCore"/> class.
        /// </summary>
        static MapperCore()
        {
            DefaultConfigurator = new DefaultMapConfig();
            Mappers = new ConcurrentBag<object>();
            MappingConfigurations = new ConcurrentBag<Tuple<Type, Type, IMappingConfigurator>>();
        }

        /// <summary>
        /// Gets the configurators.
        /// </summary>
        public virtual Tuple<Type, Type, IMappingConfigurator>[] Configurations
        {
            get { return MappingConfigurations.ToArray(); }
        }

        /// <summary>
        /// Initializes the mapper.
        /// </summary>
        /// <param name="mapperInitializator">The mapper initialization.</param>
        public void Initialize(IMapperInitializator mapperInitializator)
        {
            mapperInitializator.ConfigureMapper(this);
        }

        public virtual void AddConfiguration<TFrom, TTo>(IMappingConfigurator configurator)
        {
            MappingConfigurations.Add(new Tuple<Type, Type, IMappingConfigurator>(typeof(TFrom), typeof(TTo), configurator));
        }

        public virtual TTo Map<TFrom, TTo>(TFrom @from)
        {
            var mapper = GetMapper<TFrom, TTo>();
            return mapper.Map(@from);
        }
        public virtual TTo Map<TFrom, TTo>(TFrom @from, TTo @to)
        {
            var mapper = GetMapper<TFrom, TTo>();
            return mapper.Map(@from, @to);
        }

        public virtual IEnumerable<TTo> MapCollection<TFrom, TTo>(IEnumerable<TFrom> @from)
        {
            var mapper = GetMapper<TFrom, TTo>();
            return mapper.MapEnum(@from);
        }

        protected virtual ObjectsMapper<TFrom, TTo> GetMapper<TFrom, TTo>()
        {
            var mapper = Mappers.FirstOrDefault(m => m is ObjectsMapper<TFrom, TTo>) as ObjectsMapper<TFrom, TTo>;

            if (mapper == null)
            {
                var configuration = MappingConfigurations.FirstOrDefault(mp => mp.Item1.IsAssignableFrom(typeof(TFrom)) && mp.Item2.IsAssignableFrom(typeof(TTo)));
                var config = configuration == null ? DefaultConfigurator : configuration.Item3;

                mapper = ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>(config);

                Mappers.Add(mapper);
            }

            return mapper;
        }
    }
}
