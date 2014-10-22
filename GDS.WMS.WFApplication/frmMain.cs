using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EStudio.Framework;
using GDS.WMS.Persistence.Dao;
using GDS.WMS.Services;
using GDS.WMS.Services.Interface;
using GDS.WMS.Model;
using System.Data.SqlClient;
using FileHelpers;
using Renci.SshNet;
namespace GDS.WMS.WFApplication
{
    public partial class frmMain : Form
    {
        private static readonly string Domain = ConfigurationManager.AppSettings["Domain"] ?? "GDS";
        private static readonly string UserName = ConfigurationManager.AppSettings["UserName"] ?? "mfg";
        private static readonly string Password = ConfigurationManager.AppSettings["Password"] ?? "mfg123";
        private static readonly string HostName = ConfigurationManager.AppSettings["HostName"] ?? "192.168.90.90";
        private static readonly string DbName = ConfigurationManager.AppSettings["dbName"] ?? "mfggds.db";
        private static readonly string FilePath = ConfigurationManager.AppSettings["Path"];
        public readonly string Path = ConfigurationManager.AppSettings["DownloadPath"];
        public frmMain()
        {
            InitializeComponent();
            BootStrapper.ServicesRegistry();
            RunMaster();
        }

        private void btnSyncWorkItem_Click(object sender, EventArgs e)
        {
            var dao = new ServicesBase<WorkItem>(new Dao<WorkItem>());
            var day = string.IsNullOrEmpty(txt.Text) ? 0 : Convert.ToInt16(txt.Text);
            var date = DateTime.Now.AddDays(day * -1).ToString("yyyy-MM-dd").Split('-');
            var time = date[1] + "/" + date[2] + "/" + date[0];
            var engine = new FileHelperEngine<WorkItem>();
            var filename = Guid.NewGuid().ToString();
            var cmd = "/app/progress/102b/bin/mpro -b -db /app/mfgpro/qad2011/db/" + DbName + " -p /app/mfgpro/qad2011/xxsrc/xxout-pptmstr.p -param {0},{1},{2}";
            cmd = string.Format(cmd, filename, Domain, time);
            try
            {
                var ssh = new SshClient(HostName, UserName, Password);
                ssh.Connect();
                ssh.RunCommand(cmd);
                ssh.RunCommand("sed -i 's/  //g' " + FilePath + filename + ".csv ");
                var sftp = new SftpClient(HostName, UserName, Password);
                sftp.Connect();
                var stream = sftp.ReadAllText(FilePath + filename + ".csv", Encoding.Default);
                var response = engine.ReadStringAsList(stream);
                var add = new List<WorkItem>();
                var data = new List<WorkItem>();
                for (var index = 0; index < response.Count; index++)
                {
                    var t1 = response[index];
                    var hashTable = new Hashtable { { "part", t1.PartNo } };
                    var item = dao.FetchOne("gds.wms.workitem.get", hashTable);
                    //新增物料数据
                    if (string.IsNullOrEmpty(item.PartNo))
                    {
                        add.Add(t1);
                        data.Add(t1);
                    }
                    if (add.Count == 30)
                    {
                        dao.Add("gds.wms.workitem", add);
                        add.Clear();
                    }
                    if (index != response.Count - 1) continue;
                    dao.Add("gds.wms.workitem", add);
                }
                ssh.RunCommand("rm " + FilePath + filename + ".csv");
                dgvData.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }

        public BaseResponse RunAffair()
        {
            var service = ServicesFactory.GetInstance<IAffair>();
            var response =   service.Run("ACI");
            service.Run("ACO");
            return response;
        }


        public BaseResponse RunMaster()
        {
            var service = ServicesFactory.GetInstance<IMaster>();
            var response = service.Run("WOO");
             //service.Run("ACI");
            return response;
        }
    }
}
