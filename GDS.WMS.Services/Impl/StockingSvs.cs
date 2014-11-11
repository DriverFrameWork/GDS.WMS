using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using EStudio.Framework;
using FileHelpers;
using GDS.WMS.Model;
using GDS.WMS.Persistence.Dao;
using GDS.WMS.Services.Interface;
using Renci.SshNet;

namespace GDS.WMS.Services.Impl
{
    public class StockingSvs : IStocking
    {
        public readonly string Path = ConfigurationManager.AppSettings["DownloadPath"];
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"] ?? "mfg";
        private static readonly string Password = ConfigurationManager.AppSettings["Password"] ?? "mfg123";
        private static readonly string HostName = ConfigurationManager.AppSettings["HostName"] ?? "192.168.90.90";
        private static readonly string FilePath = ConfigurationManager.AppSettings["Path"];
        private static readonly string IsTrue = ConfigurationManager.AppSettings["IsTrue"];

        public EStudio.Framework.BaseResponse Run(SshClient ssh,SftpClient sftp)
        {
            var response = new BaseResponse();
            var dao = new ServicesBase<Stocking>(new Dao<Stocking>());
            var filename = Guid.NewGuid() + ".csv";
            var file = new FileInfo(Path + filename);
            var entities = dao.FetchMany("gds.wms.stocking.get", new Hashtable());
            if (entities == null || entities.Count <= 0) return null;
            //var ssh = new SshClient(HostName, UserName, Password);
            //var sftp = new SftpClient(HostName, UserName, Password);
            //ssh.Connect();
            //sftp.Connect();
            using (var sw = file.CreateText())
            {
                foreach (var entity in entities)
                {
                    var id = entity.Id;
                    var partNo = entity.PartNo.Trim();
                    var loc = entity.Location;
                    var lotser = string.IsNullOrEmpty(entity.Lotser) ? " " : entity.Lotser;
                    var xh = string.IsNullOrEmpty(entity.Ref) ? " " : entity.Ref;
                    var qty = entity.Qty;
                    var date = string.IsNullOrEmpty(entity.InputTime)
                        ? DateTime.Now.ToString("yyyy-MM-dd").Split('-')
                        : DateTime.Parse(entity.InputTime).ToString("yyyy-MM-dd").Split('-');
                    var input = date[1] + "/" + date[2] + "/" + date[0];
                    sw.WriteLine(id + "," + partNo + "," + loc + "," + lotser + "," + xh + "," + qty + "," + input);
                }
                sw.Flush();
                sw.Close();
            }
            var fileStream = new FileStream(Path + filename, FileMode.Open);
            var stream = string.Empty;
            sftp.UploadFile(fileStream, FilePath + "in/" + filename);
            var command = IsTrue == "false"
                       ? ssh.RunCommand("/backup/qad/bat/client.test" + " " + filename + ",cyc")
                       : ssh.RunCommand("/backup/qad/bat/client.auto" + " " + filename + ",cyc");
            if (sftp.Exists(FilePath + "out/cyc-result.csv"))
            {
                stream = sftp.ReadAllText(FilePath + "out/cyc-result.csv", Encoding.Default);
            }
            ssh.RunCommand("rm " + FilePath + "in/" + filename);
            if (!string.IsNullOrEmpty(stream))
            {
                var enginer = new FileHelperEngine<QADResponse>();
                var res = enginer.ReadStringAsList(stream);
                var result = new List<Stocking>();
                var dic = new Dictionary<int, int>();
                foreach (var t in res.Where(t => !dic.ContainsKey(t.Id)))
                {
                    dic.Add(t.Id, t.Status);
                }
                for (var i = 0; i < entities.Count; i++)
                {
                    var id = entities[i].Id;
                    if (!dic.ContainsKey(id) || dic[id] != 1) continue;
                    entities[i].Status = dic[id];
                    result.Add(entities[i]);
                    dic.Remove(entities[i].Id);
                }
                if (result.Count > 0)
                {
                    dao.Update("gds.wms.stocking", result);
                }
                response.Data = result;
                response.IsSuccess = true;
                response.Count = result.Count;
            }
            else
            {
                response.IsSuccess = false;
            }
            //ssh.RunCommand("exit");
            //ssh.Disconnect();
            //sftp.Disconnect();
            return response;
        }
    }
}
