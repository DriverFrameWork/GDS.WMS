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
    public class MasterSvs : IMaster
    {
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"] ?? "mfg";
        private static readonly string Password = ConfigurationManager.AppSettings["Password"] ?? "mfg123";
        private static readonly string HostName = ConfigurationManager.AppSettings["HostName"] ?? "192.168.90.90";
        private static readonly string FilePath = ConfigurationManager.AppSettings["Path"];
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BaseResponse Run(string type)
        {
            var response = new BaseResponse();
            var dao = new ServicesBase<BusinessMstr>(new Dao<BusinessMstr>());
            var sftp = new SftpClient(HostName, UserName, Password);
            sftp.Connect();
            try
            {
                var master = string.Empty;
                var detail = string.Empty;
                if (type == "WOO")
                {
                    if (sftp.Exists(FilePath + "out/wms-woo.csv"))
                    {
                        master = sftp.ReadAllText(FilePath + "out/wms-woo.csv", Encoding.Default);
                    }
                    if (sftp.Exists(FilePath + "out/wms-wood.csv"))
                    {
                        detail = sftp.ReadAllText(FilePath + "out/wms-wood.csv", Encoding.Default);
                    }
                }
                //计划外入库||计划外出库
                if (type == "PNI" || type == "PNO")
                {

                    if (sftp.Exists(FilePath + "out/wms-unp.csv"))
                    {
                        master = sftp.ReadAllText(FilePath + "out/wms-unp.csv", Encoding.Default);
                    }
                    if (sftp.Exists(FilePath + "out/wms-unpd.csv"))
                    {
                        detail = sftp.ReadAllText(FilePath + "out/wms-unpd.csv", Encoding.Default);
                    }
                }
                //调拨入库/出库
                if (type == "ACI" || type == "ACO" || type == "SMO")
                {
                    if (sftp.Exists(FilePath + "out/wms-tr.csv"))
                    {
                        master = sftp.ReadAllText(FilePath + "out/wms-tr.csv", Encoding.Default);
                    }
                    if (sftp.Exists(FilePath + "out/wms-trd.csv"))
                    {
                        detail = sftp.ReadAllText(FilePath + "out/wms-trd.csv", Encoding.Default);
                    }
                }
                //部门领用
                if (type == "DPO")
                {

                }
                if (string.IsNullOrEmpty(master))
                {
                    logger.Info("事务主档没有内容");
                    return response;
                }
                if (string.IsNullOrEmpty(detail))
                {
                    logger.Info("事务明细没有内容");
                    return response;
                }
                var masterEnginer = new FileHelperEngine<BusinessMstr>();
                var entities = masterEnginer.ReadStringAsList(master);
                #region 保存事务主表信息
                var addm = new List<BusinessMstr>();
                var datam = new List<BusinessMstr>();
                for (var index = 0; index < entities.Count; index++)
                {
                    var entity = entities[index];
                    var hashTable = new Hashtable { { "qadno", entity.QADNo.Trim() }, { "type", entity.Type.Trim() } };
                    var item = dao.FetchOne("gds.wms.businessmstr.get", hashTable);
                    //新增事务主表数据
                    if (item == null)
                    {
                        addm.Add(entity);
                        datam.Add(entity);
                    }
                    if (addm.Count != 50) continue;
                    dao.Add("gds.wms.businessmstr", addm);
                    addm.Clear();
                }
                dao.Add("gds.wms.businessmstr", addm);
                addm.Clear();
                #endregion

                #region 保存事务从表信息

                var daodet = new ServicesBase<BusinessDet>(new Dao<BusinessDet>());
                var detEnginer = new FileHelperEngine<BusinessDet>();

                var items = detEnginer.ReadStringAsList(detail);
                var addd = new List<BusinessDet>();
                var datad = new List<BusinessDet>();
                for (var i = 0; i < items.Count; i++)
                {
                    var det = items[i];
                    if (string.IsNullOrEmpty(det.QADNo) || string.IsNullOrEmpty(det.PartNo)) continue;
                    var hashTable = new Hashtable();
                    hashTable.Clear();
                    hashTable.Add("qadno", det.QADNo);
                    hashTable.Add("part", det.PartNo);
                    var item = daodet.FetchOne("gds.wms.businessdet.get", hashTable);
                    //新增事务从表数据
                    if (item == null)
                    {
                        addd.Add(det);
                        datad.Add(det);
                    }
                    if (addd.Count != 50) continue;
                    daodet.Add("gds.wms.businessdet", addd);
                    addd.Clear();
                }
                daodet.Add("gds.wms.businessdet", addd);
                addd.Clear();
                response.Count = datam.Count;
                response.Data = datam;
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return response;
            }
            if (type == "WOO")
            {
                if (sftp.Exists(FilePath + "out/wms-woo.csv"))
                {
                    sftp.DeleteFile(FilePath + "out/wms-woo.csv");
                }
                if (sftp.Exists(FilePath + "out/wms-wood.csv"))
                {
                    sftp.DeleteFile(FilePath + "out/wms-wood.csv");
                }
            }
            if (type == "ACI" || type == "ACO" || type == "SMO")
            {
                if (sftp.Exists(FilePath + "out/wms-tr.csv"))
                {
                    sftp.DeleteFile(FilePath + "out/wms-tr.csv");
                }
                if (sftp.Exists(FilePath + "out/wms-trd.csv"))
                {
                    sftp.DeleteFile(FilePath + "out/wms-trd.csv");
                }
            }
            //计划外入库||计划外出库
            if (type == "PNI" || type == "PNO")
            {
                if (sftp.Exists(FilePath + "out/wms-unp.csv"))
                {
                    sftp.DeleteFile(FilePath + "out/wms-unp.csv");
                }
                if (sftp.Exists(FilePath + "out/wms-unpd.csv"))
                {
                    sftp.DeleteFile(FilePath + "out/wms-unpd.csv");
                }
            }
            sftp.Disconnect();
            response.IsSuccess = true;
            return response;
        }
    }
}
