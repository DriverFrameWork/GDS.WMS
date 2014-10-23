﻿using System;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using GDS.WMS.Services;
using GDS.WMS.Services.Interface;
using Quartz;

namespace GDS.WMS.ClientService
{
    public class MasterService : IJob
    {
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                BootStrapper.ServicesRegistry();
                logger.Info("同步事务主数据任务开始运行");
                var sw = new Stopwatch();
                sw.Start();
                var service = ServicesFactory.GetInstance<IMaster>();
                // logger.Info("读取工单领料开始");
                service.Run("WOO");
                //logger.Info("读取工单领料结束");

                //logger.Info("读取计划外入库开始");
                service.Run("PNI");
                //logger.Info("读取计划外入库结束");

                //logger.Info("读取计划外出库开始");
                service.Run("PNO");
                //logger.Info("读取计划外出库结束");

                //logger.Info("读取调拨入库开始");
                service.Run("ACI");
                //logger.Info("读取调拨入库结束");

                //logger.Info("读取调拨出库开始");
                service.Run("ACO");
                //logger.Info("读取调拨出库结束");
                sw.Stop();
                logger.Info("同步事务主数据任务结束运行,总运行时间:" + sw.Elapsed.TotalMilliseconds + "毫秒");
            }
            catch (Exception ex)
            {
                logger.Error("同步事务主数据任务运行异常", ex);
            }
        }
    }
}
