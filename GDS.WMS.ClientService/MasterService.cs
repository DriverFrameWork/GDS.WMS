﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using GDS.WMS.Services;
using GDS.WMS.Services.Interface;
using Quartz;
using Renci.SshNet;

namespace GDS.WMS.ClientService
{
    public class MasterService : IJob
    {
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"] ?? "mfg";
        private static readonly string Password = ConfigurationManager.AppSettings["Password"] ?? "mfg123";
        private static readonly string HostName = ConfigurationManager.AppSettings["HostName"] ?? "192.168.90.90";
        private static readonly ScpClient Scp = new ScpClient(HostName, UserName, Password);
        private static readonly SftpClient Sftp = new SftpClient(HostName, UserName, Password);

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                BootStrapper.ServicesRegistry();
                if (!Scp.IsConnected)
                    Scp.Connect();
                if (!Sftp.IsConnected)
                    Sftp.Connect(); ;
                logger.Info("同步事务主数据任务开始运行");
                var sw = new Stopwatch();
                sw.Start();
                var service = ServicesFactory.GetInstance<IMaster>();
                service.Run(Sftp, Scp, "WOO");
                service.Run(Sftp, Scp, "PNI");
                service.Run(Sftp, Scp, "PNO");
                service.Run(Sftp, Scp, "ACI");
                service.Run(Sftp, Scp, "ACO");
                sw.Stop();
                logger.Info("同步事务主数据任务结束运行,总运行时间:" + sw.Elapsed.TotalMilliseconds + "毫秒");
                Scp.Disconnect();
                Sftp.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error("同步事务主数据任务运行异常", ex);
            }
        }
    }
}
