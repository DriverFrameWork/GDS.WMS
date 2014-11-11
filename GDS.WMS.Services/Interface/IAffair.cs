using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using EStudio.Framework;
using Renci.SshNet;

namespace GDS.WMS.Services.Interface
{
    public interface IAffair
    {
        BaseResponse Run(SshClient ssh, SftpClient sftp, string type);
    }
}
