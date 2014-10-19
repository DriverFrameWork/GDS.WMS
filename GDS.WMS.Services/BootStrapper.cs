using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using EStudio.Framework.Configuration;

namespace GDS.WMS.Services
{
    public class BootStrapper
    {
        public static void ServicesRegistry()
        {
            var container = ServiceLocator.Instance;
            ApplicationSettingsFactory.InitializeApplicationSettingsFactory
                    (ServicesFactory.GetInstance<IApplicationSettings>());
        }
    }
}
