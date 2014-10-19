using System;

namespace EStudio.Framework.Logging
{
    public interface ILogger
    {
        //写系统日记
        void Log(string msg);
        //写错误日记
        void Log(string msg, Exception ex);
    }
}
