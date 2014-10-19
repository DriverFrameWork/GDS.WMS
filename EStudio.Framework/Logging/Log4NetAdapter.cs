using System;
using EStudio.Framework.Configuration;
using log4net;
using log4net.Config;

namespace EStudio.Framework.Logging
{
    public class Log4NetAdapter : ILogger
    {
        private readonly ILog log;

        public Log4NetAdapter()
        {
            XmlConfigurator.Configure();
            log = LogManager
                .GetLogger(ApplicationSettingsFactory.GetApplicationSettings().LoggerName);
        }


        //系统日记
        public void Log(string msg)
        {
            if (!log.IsInfoEnabled) return;
            log.Info(msg);
        }

        //错误日记
        public void Log(string msg, Exception ex)
        {
            if (!log.IsErrorEnabled) return;
            log.Error(msg, ex);
        }
    }
}
