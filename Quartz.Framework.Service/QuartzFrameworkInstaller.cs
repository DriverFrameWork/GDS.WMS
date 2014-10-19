using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace Quartz.Framework.Service
{
    [RunInstaller(true)]
    public partial class QuartzFrameworkInstaller : System.Configuration.Install.Installer
    {
        public QuartzFrameworkInstaller()
        {
            InitializeComponent();

            var serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            var serviceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                Description = "QuartzFrameworkService",
                DisplayName = "QuartzFrameworkService"
            };
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
