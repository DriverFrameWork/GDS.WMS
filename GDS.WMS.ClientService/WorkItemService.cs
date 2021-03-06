﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EStudio.Framework.DbHelper;
using FileHelpers;
using GDS.WMS.Model;
using GDS.WMS.Services;
using GDS.WMS.Services.Interface;
using Quartz;
using System.Diagnostics;
namespace GDS.WMS.ClientService
{
    /// <summary>
    /// 同步物料数据
    /// </summary>
    public class WorkItemService : IJob
    {
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void Execute(IJobExecutionContext context)
        {
            try
            {
                BootStrapper.ServicesRegistry();
                logger.Info("同步物料数据任务开始运行");
                var sw = new Stopwatch();
                sw.Start();
                var service = ServicesFactory.GetInstance<IWorkItem>();
                service.Run();
                sw.Stop();
                logger.Info("同步物料数据任务结束运行,总运行时间:" + sw.Elapsed.TotalMilliseconds + "毫秒");
            }
            catch (Exception ex)
            {
                logger.Error("同步物料数据任务运行异常", ex);
            }
        }
    }
}
