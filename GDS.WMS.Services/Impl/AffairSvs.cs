using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using EStudio.Framework;
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
        public BaseResponse Run(string type)
        {
            var response = new BaseResponse();
            try
            {
                //var scp = new ScpClient(HostName, UserName, Password);
                var ssh = new SshClient(HostName, UserName, Password);
                var sftp = new SftpClient(HostName, UserName, Password);
                //scp.Connect();
                ssh.Connect();
                sftp.Connect();
                var dao = new ServicesBase<AffairItem>(new Dao<AffairItem>());

                var filename = Guid.NewGuid() + ".csv";
                var file = new FileInfo(Path + filename);
                var data = GetAffairListByType(file, type);
                var stream = string.Empty;
                if (data != null && data.Count > 0)
                {
                    var fileStream = new FileStream(Path + filename, FileMode.Open);
                    sftp.UploadFile(fileStream, FilePath + "in/" + filename);
                    //采购入库
                    if (type == "POI")
                    {
                        sftp.UploadFile(fileStream, "/app/tomcat6/webapps/web/" + filename);
                        if (IsTrue == "false")
                        {
                            ssh.RunCommand("/backup/qad/bat/client.pointest" + " " + filename);
                        }
                        else
                        {
                            var termial = ssh.RunCommand("/backup/qad/bat/client.poin" + " " + filename);
                        }
                        foreach (var affairItem in data)
                        {
                            affairItem.Status = 1;
                        }
                        dao.Update("gds.wms.affairitem", data);
                        return response;
                        //stream = sftp.ReadAllText(FilePath + "out/woo-result.csv", Encoding.Default);
                    }
                    //工单发料
                    if (type == "WOO")
                    {
                        if (IsTrue == "false")
                        {
                            ssh.RunCommand("/backup/qad/bat/client.test" + " " + filename + ",woo");
                        }
                        else
                        {
                            var termial = ssh.RunCommand("/backup/qad/bat/client.auto" + " " + filename + ",woo");
                        }
                        stream = sftp.ReadAllText(FilePath + "out/woo-result.csv", Encoding.Default);
                    }
                    //计划外入库/计划外出库
                    if (type == "PNO" || type == "PNI")
                    {
                        if (IsTrue == "false")
                        {
                            ssh.RunCommand("/backup/qad/bat/client.test" + " " + filename + ",unp");
                        }
                        else
                        {
                            var termial = ssh.RunCommand("/backup/qad/bat/client.auto" + " " + filename + ",unp");
                        }
                        stream = sftp.ReadAllText(FilePath + "out/unp-result.csv", Encoding.Default);
                    }
                    //调拨出入库
                    if (type == "ACI" || type == "ACO")
                    {
                        if (IsTrue == "false")
                        {
                            ssh.RunCommand("/backup/qad/bat/client.test" + " " + filename + ",trd");
                        }
                        else
                        {
                            ssh.RunCommand("/backup/qad/bat/client.auto" + " " + filename + ",trd");
                        }
                        stream = sftp.ReadAllText(FilePath + "out/trd-result.csv", Encoding.Default);
                    }
                    ssh.RunCommand("rm " + FilePath + "in/" + filename);
                    //执行成功后，返回执行结果

                    if (!string.IsNullOrEmpty(stream))
                    {
                        var enginer = new FileHelperEngine<QADResponse>();
                        var res = enginer.ReadStringAsList(stream);
                        var result = new List<AffairItem>();
                        var dic = new Dictionary<int, int>();
                        foreach (var t in res)
                        {
                            if (!dic.ContainsKey(t.Id))
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
                            dao.Update("gds.wms.affairitem", result);
                        }
                    }
                    response.Data = data;
                    response.IsSuccess = true;
                    response.Count = data.Count;
                    ssh.Disconnect();
                    sftp.Disconnect();
                }
                else
                {
                    response.IsSuccess = false;
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
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
                sw.Flush();
                sw.Close();
                return entities;
            }
        }
    }
}
