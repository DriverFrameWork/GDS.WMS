using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using EStudio.Framework;
using EStudio.Framework.Logging;
using FileHelpers;
using GDS.WMS.Model;
using GDS.WMS.Persistence.Dao;
using GDS.WMS.Services.Interface;
using Renci.SshNet;

namespace GDS.WMS.Services.Impl
{
    public class AffairSvs : IAffair
    {
        public readonly string Path = ConfigurationManager.AppSettings["DownloadPath"];
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"] ?? "mfg";
        private static readonly string Password = ConfigurationManager.AppSettings["Password"] ?? "mfg123";
        private static readonly string HostName = ConfigurationManager.AppSettings["HostName"] ?? "192.168.90.90";
        private static readonly string FilePath = ConfigurationManager.AppSettings["Path"];
        private static readonly string IsTrue = ConfigurationManager.AppSettings["IsTrue"];
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BaseResponse Run(SshClient ssh, SftpClient sftp, string type)
        {
            var response = new BaseResponse();
            try
            {
                var dao = new ServicesBase<AffairItem>(new Dao<AffairItem>());
                var filename = Guid.NewGuid() + ".csv";
                var file = new FileInfo(Path + filename);
                var data = GetAffairListByType(file, type);
                var stream = string.Empty;
                if (data != null && data.Count > 0)
                {
                    SshCommand command = null;
                    //var ssh = new SshClient(HostName, UserName, Password);
                    //var sftp = new SftpClient(HostName, UserName, Password);
                    ssh.Connect();
                    sftp.Connect();
                    var fileStream = new FileStream(Path + filename, FileMode.Open);
                    sftp.UploadFile(fileStream, FilePath + "in/" + filename);
                    //采购入库
                    if (type == "POI")
                    {
                        command = IsTrue == "false"
                            ? ssh.RunCommand("/backup/qad/bat/client.test" + " " + filename + ",poo")
                            : ssh.RunCommand("/backup/qad/bat/client.auto" + " " + filename + ",poo");
                        if (sftp.Exists(FilePath + "out/poo-result.csv"))
                        {
                            stream = sftp.ReadAllText(FilePath + "out/poo-result.csv", Encoding.Default);
                        }
                    }
                    //工单发料
                    if (type == "WOO")
                    {
                        command = IsTrue == "false"
                            ? ssh.RunCommand("/backup/qad/bat/client.test" + " " + filename + ",woo")
                            : ssh.RunCommand("/backup/qad/bat/client.auto" + " " + filename + ",woo");
                        if (sftp.Exists(FilePath + "out/woo-result.csv"))
                        {
                            stream = sftp.ReadAllText(FilePath + "out/woo-result.csv", Encoding.Default);
                        }
                    }
                    //计划外入库/计划外出库
                    if (type == "PNO" || type == "PNI")
                    {
                        command = IsTrue == "false"
                            ? ssh.RunCommand("/backup/qad/bat/client.test" + " " + filename + ",unp")
                            : ssh.RunCommand("/backup/qad/bat/client.auto" + " " + filename + ",unp");
                        if (sftp.Exists(FilePath + "out/unp-result.csv"))
                        {
                            stream = sftp.ReadAllText(FilePath + "out/unp-result.csv", Encoding.Default);
                        }
                    }
                    //调拨出入库
                    if (type == "ACI" || type == "ACO")
                    {
                        command = IsTrue == "false"
                            ? ssh.RunCommand("/backup/qad/bat/client.test" + " " + filename + ",trd")
                            : ssh.RunCommand("/backup/qad/bat/client.auto" + " " + filename + ",trd");
                        if (sftp.Exists(FilePath + "out/trd-result.csv"))
                        {
                            stream = sftp.ReadAllText(FilePath + "out/trd-result.csv", Encoding.Default);
                        }
                    }
                    ssh.RunCommand("rm " + FilePath + "in/" + filename);
                    if (command != null) logger.Info(command.Result);
                    //执行成功后，返回执行结果

                    if (!string.IsNullOrEmpty(stream))
                    {
                        var enginer = new FileHelperEngine<QADResponse>();
                        var res = enginer.ReadStringAsList(stream);
                        var result = new List<AffairItem>();
                        var dic = new Dictionary<int, int>();
                        foreach (var t in res.Where(t => !dic.ContainsKey(t.Id)))
                        {
                            dic.Add(t.Id, t.Status);
                        }
                        for (var i = 0; i < data.Count; i++)
                        {
                            var id = data[i].Id;
                            if (!dic.ContainsKey(id) || dic[id] != 1) continue;
                            data[i].Status = dic[id];
                            result.Add(data[i]);
                            dic.Remove(data[i].Id);
                        }
                        if (result.Count > 0)
                        {
                            // dao.Update("gds.wms.affairitem", result);
                        }
                        response.Data = data;
                        response.IsSuccess = true;
                        response.Count = data.Count;
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
                if (File.Exists(Path + filename))
                    try
                    {
                        File.Delete(Path + filename);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                logger.Error(ex.Message);
                return response;
            }
            return response;

        }

        public IList<AffairItem> GetAffairListByType(FileInfo fileInfo, string type)
        {
            IList<AffairItem> entities = null;
            var hashTable = new Hashtable { { "status", 0 } };
            var dao = new ServicesBase<AffairItem>(new Dao<AffairItem>());
            //采购单入库
            if (type == "POI")
            {
                entities = dao.FetchMany("gds.wms.affairitem.getpoi", hashTable);
                if (entities != null && entities.Count > 0)
                {
                    using (var sw = fileInfo.CreateText())
                    {
                        foreach (var entity in entities)
                        {
                            var id = entity.Id;
                            var qadNo = entity.QADNo.Trim();
                            var line = entity.SNID;
                            var partNo = entity.PartNo.Trim();
                            var qty = entity.AffairQty;
                            var loc = entity.Location;
                            var lotser = string.IsNullOrEmpty(entity.Lotser) ? " " : entity.Lotser;
                            var xh = string.IsNullOrEmpty(entity.Ref) ? " " : entity.Ref;
                            sw.WriteLine(id + " " + qadNo + " " + line + " " + qty);
                        }
                        sw.Flush();
                        sw.Close();
                        return entities;
                    }
                }
            }
            //工单发料
            if (type == "WOO")
            {
                entities = dao.FetchMany("gds.wms.affairitem.getwoo", hashTable);
            }
            //计划外入库
            if (type == "PNI")
            {
                entities = dao.FetchMany("gds.wms.affairitem.getpni", hashTable);
            }
            //计划外出库
            if (type == "PNO")
            {
                entities = dao.FetchMany("gds.wms.affairitem.getpno", hashTable);
            }
            //调拨入库
            if (type == "ACI")
            {
                entities = dao.FetchMany("gds.wms.affairitem.getaci", hashTable);
            }
            //调拨出库
            if (type == "ACO")
            {
                entities = dao.FetchMany("gds.wms.affairitem.getaco", hashTable);
            }
            //工单超领
            if (type == "SMO")
            {
                entities = dao.FetchMany("gds.wms.affairitem.getsmo", hashTable);
            }
            if (entities == null || entities.Count <= 0) return null;
            using (var sw = fileInfo.CreateText())
            {
                foreach (var entity in entities)
                {
                    var id = entity.Id;
                    var otype = entity.Type.Trim();
                    var qadNo = entity.QADNo.Trim();
                    var partNo = entity.PartNo.Trim();
                    var qty = entity.AffairQty;
                    var loc = entity.Location;
                    var lotser = string.IsNullOrEmpty(entity.Lotser) ? " " : entity.Lotser;
                    var xh = string.IsNullOrEmpty(entity.Ref) ? " " : entity.Ref;
                    sw.WriteLine(id + "," + qadNo + "," + otype + "," + partNo + "," + qty + "," + loc + "," + lotser + "," + xh);
                }
                //var sftp = new SftpClient("", "", "");
                sw.Flush();
                sw.Close();
                return entities;
            }
        }
    }
}
