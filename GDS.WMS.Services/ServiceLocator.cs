using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Autofac;
using Autofac.Configuration;

namespace GDS.WMS.Services
{
    public class ServiceLocator:IServiceProvider
    {
        private static IContainer _container;
        private static readonly ServiceLocator instance = new ServiceLocator();

        public ServiceLocator()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            _container = builder.Build();
        }
        public static ServiceLocator Instance
        {
            get { return instance; }
        }
        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
        public T GetService<T>()
        {
            return _container.Resolve<T>();
        }

        public T GetService<T>(string name)
        {
            return _container.ResolveNamed<T>(name);
        }
    }
}
