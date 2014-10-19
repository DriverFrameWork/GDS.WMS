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

        public BaseResponse Run(string type)
        {
            var response = new BaseResponse();
            var dao = new ServicesBase<BusinessMstr>(new Dao<BusinessMstr>());
            var sftp = new SftpClient(HostName, UserName, Password);
            sftp.Connect();
            var master = string.Empty;
            var detail = string.Empty;

            //工单发料
            if (type == "WOO")
            {
                if (sftp.Exists(FilePath + "out/wms-woo.csv"))
                {
                    master = sftp.ReadAllText(FilePath + "out/wms-woo.csv", Encoding.Default);
                    sftp.DeleteFile(FilePath + "out/wms-woo.csv");
                }
                if (sftp.Exists(FilePath + "out/wms-woo.csv"))
                {
                    detail = sftp.ReadAllText(FilePath + "out/wms-wood.csv", Encoding.Default);
                    sftp.DeleteFile(FilePath + "out/wms-wood.csv");
                }

            }
            //计划外入库||计划外出库
            if (type == "PNI" || type == "PNO")
            {
                master = sftp.ReadAllText(FilePath + "out/wms-unp.csv", Encoding.Default);
                detail = sftp.ReadAllText(FilePath + "out/wms-unpd.csv", Encoding.Default);

            }
            //调拨入库/出库
            if (type == "ACI" || type == "ACO" || type == "SMO")
            {
                if (sftp.Exists(FilePath + "out/wms-tr.csv"))
                {
                    master = sftp.ReadAllText(FilePath + "out/wms-tr.csv", Encoding.Default);
                    sftp.DeleteFile(FilePath + "out/wms-tr.csv");
                }
                if (sftp.Exists(FilePath + "out/wms-trd.csv"))
                {
                    detail = sftp.ReadAllText(FilePath + "out/wms-trd.csv", Encoding.Default);

                    sftp.DeleteFile(FilePath + "out/wms-trd.csv");
                }
            }
            //部门领用
            if (type == "DPO")
            {

            }
            if (string.IsNullOrEmpty(master))
            {
                response.ErrorMessage = "事务主档没有内容";
                return response;
            }
            if (string.IsNullOrEmpty(detail))
            {
                response.ErrorMessage = "事务明细没有内容";
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
                var hashTable = new Hashtable { { "qadno", entity.QADNo }, { "type", entity.Type } };
                var item = dao.FetchOne("gds.wms.businessmstr.get", hashTable);
                //新增事务主表数据
                if (string.IsNullOrEmpty(item.QADNo))
                {
                    addm.Add(entity);
                    datam.Add(entity);
                }
                if (addm.Count == 50)
                {
                    dao.Add("gds.wms.businessmstr", addm);
                    addm.Clear();
                }
                if (index != entities.Count - 1) continue;
                dao.Add("gds.wms.businessmstr", addm);
            }
            #endregion

            #region 保存事务从表信息

            var daodet = new ServicesBase<BusinessDet>(new Dao<BusinessDet>());
            var detEnginer = new FileHelperEngine<BusinessDet>();

            var items = detEnginer.ReadStringAsList(detail);
            var addd = new List<BusinessDet>();
            var datad = new List<BusinessDet>();
            for (int i = 0; i < items.Count; i++)
            {
                var det = items[i];
                var hashTable = new Hashtable { { "qadno", det.QADNo }, { "part", det.PartNo } };
                var item = daodet.FetchOne("gds.wms.businessdet.get", hashTable);
                //新增事务从表数据
                if (string.IsNullOrEmpty(item.QADNo))
                {
                    addd.Add(det);
                    datad.Add(det);
                }
                if (addd.Count == 50)
                {
                    daodet.Add("gds.wms.businessdet", addd);
                    addd.Clear();
                }
                if (i != items.Count - 1) continue;
                daodet.Add("gds.wms.businessdet", addd);
            }
            #endregion
            sftp.Disconnect();

            response.IsSuccess = true;
            response.Count = datam.Count;
            response.Data = datam;
            return response;
        }
    }
}
