using System;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using GDS.WMS.Services;
using GDS.WMS.Services.Interface;
using Quartz;

namespace GDS.WMS.ClientService
{
    public class WOOAffairService : IJob
    {
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                BootStrapper.ServicesRegistry();
                logger.Info("同步事务数据任务开始运行");
                var sw = new Stopwatch();
                sw.Start();
                var service = ServicesFactory.GetInstance<IAffair>();
                service.Run("WOO");
                sw.Stop();
                logger.Info("同步事务数据任务结束运行,总运行时间:" + sw.Elapsed.TotalMilliseconds + "毫秒");
            }
            catch (Exception ex)
            {
                logger.Error("同步事务数据任务运行异常", ex);
            }
        }
    }
}
