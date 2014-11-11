using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EStudio.Framework;
using Renci.SshNet;

namespace GDS.WMS.Services.Interface
{
    public interface IStocking
    {
        BaseResponse Run(SshClient ssh,SftpClient sftp);
    }
}
