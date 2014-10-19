using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using EStudio.Framework;
using FileHelpers;
using GDS.WMS.Model;
using GDS.WMS.Persistence.Dao;
using GDS.WMS.Services.Interface;
using Renci.SshNet;

namespace GDS.WMS.Services.Impl
{
    public class WorkItemSvs : IWorkItem
    {
        private static string domain = ConfigurationManager.AppSettings["Domain"] ?? "GDS";
        private static string userName = ConfigurationManager.AppSettings["UserName"] ?? "mfg";
        private static string password = ConfigurationManager.AppSettings["Password"] ?? "lin123";
        private static string hostName = ConfigurationManager.AppSettings["HostName"] ?? "192.168.0.200";
        private static string dbName = ConfigurationManager.AppSettings["dbName"] ?? "mfgprod.db";
        private static string filePath = ConfigurationManager.AppSettings["Path"];

        public BaseResponse Run()
        {
            var response = new BaseResponse();
            var dao = new ServicesBase<WorkItem>(new Dao<WorkItem>());
            var date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Split('-');
            var time = date[1] + "/" + date[2] + "/" + date[0];
            var engine = new FileHelperEngine<WorkItem>();
            var filename = Guid.NewGuid().ToString();
            var cmd = "/app/progress/102b/bin/mpro -b -db /app/mfgpro/qad2011/db/" + dbName + " -p /app/mfgpro/qad2011/xxsrc/xxout-pptmstr.p -param {0},{1},{2}";
            cmd = string.Format(cmd, filename, domain, time);
            try
            {
                var ssh = new SshClient(hostName, userName, password);
                ssh.Connect();
                ssh.RunCommand(cmd);
                var sftp = new SftpClient(hostName, userName, password);
                sftp.Connect();
                var stream = sftp.ReadAllText(filePath + filename + ".csv", Encoding.Default);
                if (string.IsNullOrEmpty(stream))
                {
                    response.IsSuccess = true;
                    response.Count = 0;
                    response.Data = null;
                    response.ErrorMessage = "没有可以更新的数据";
                    return response;
                }
                var entities = engine.ReadStringAsList(stream);
                ssh.Disconnect();
                sftp.Disconnect();
                var add = new List<WorkItem>();
                for (var index = 0; index < entities.Count; index++)
                {
                    var entity = entities[index];
                    var hashTable = new Hashtable { { "part", entity.PartNo } };
                    var item = dao.FetchOne("gds.wms.workitem.get", hashTable);
                    //新增物料数据
                    if (string.IsNullOrEmpty(item.PartNo))
                    {
                        add.Add(entity);
                    }
                    if (add.Count == 50)
                    {
                        dao.Add("gds.wms.workitem", add);
                        add.Clear();
                    }
                    if (index != entities.Count - 1) continue;
                    dao.Add("gds.wms.workitem", add);
                }
                response.IsSuccess = true;
                response.Count = entities.Count;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
            return response;
        }
    }
}
