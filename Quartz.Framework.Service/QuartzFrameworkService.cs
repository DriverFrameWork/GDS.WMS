using System.ServiceProcess;
using Common.Logging;
using Quartz.Impl;

namespace Quartz.Framework.Service
{
    public partial class QuartzFrameworkService : ServiceBase
    {
        private readonly ILog _logger;
        private readonly IScheduler _scheduler;
        public QuartzFrameworkService()
        {
            InitializeComponent();
            _logger = LogManager.GetLogger(GetType());
            _logger.Info("Quartz服务成功启动");
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();
            _logger.Info("Quartz服务成功启动");
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown(false);
            _logger.Info("Quartz服务成功启动");
        }
    }
}
