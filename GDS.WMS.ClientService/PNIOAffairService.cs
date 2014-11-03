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
    /// <summary>
    /// 
    /// </summary>
    public class PNIOAffairService : IJob
    {
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                BootStrapper.ServicesRegistry();
                logger.Info("同步计划外事务主数据任务开始运行");
                var sw = new Stopwatch();
                sw.Start();
                var service = ServicesFactory.GetInstance<IAffair>();
                logger.Info("计划外入库开始");
                var pni = service.Run("PNI");
                logger.Info(pni.ErrorMessage + " " + pni.Count + " " + pni.IsSuccess);
                logger.Info("计划外入库结束");
                logger.Info("计划外出库开始");
                var pno = service.Run("PNO");
                logger.Info(pno.ErrorMessage + " " + pno.Count + " " + pno.IsSuccess);
                logger.Info("计划外出库结束");
                sw.Stop();
                logger.Info("同步计划外事务主数据任务结束运行,总运行时间:" + sw.Elapsed.TotalMilliseconds + "毫秒");
            }
            catch (Exception ex)
            {
                logger.Error("同步事务主数据任务运行异常", ex);
            }
        }
    }
}
