using System;
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
    /// <summary>
    /// 
    /// </summary>
    public class ACIOAffairService : IJob
    {
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"] ?? "mfg";
        private static readonly string Password = ConfigurationManager.AppSettings["Password"] ?? "mfg123";
        private static readonly string HostName = ConfigurationManager.AppSettings["HostName"] ?? "192.168.90.90";
        private static readonly SshClient Ssh = new SshClient(HostName, UserName, Password);
        private static readonly SftpClient Sftp = new SftpClient(HostName, UserName, Password);
       
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                BootStrapper.ServicesRegistry();
                if (!Ssh.IsConnected)
                    Ssh.Connect();
                if (!Sftp.IsConnected)
                    Sftp.Connect();
                var sw = new Stopwatch();
                sw.Start();
                var service = ServicesFactory.GetInstance<IAffair>();
                logger.Info("调拨入库开始");
                var aci = service.Run(Ssh,Sftp,"ACI");
                var aco = service.Run(Ssh,Sftp,"ACO");
                sw.Stop();
                logger.Info("同步调拨事务主数据任务结束运行,总运行时间:" + sw.Elapsed.TotalMilliseconds + "毫秒");
                Ssh.RunCommand("exit");
                Ssh.Disconnect();
                Sftp.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error("同步调拨事务主数据任务运行异常", ex);
            }
        }
    }
}
