using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using EStudio.Framework;
using FileHelpers;

namespace GDS.WMS.Model
{
    [DelimitedRecord(";")]

    public class WorkItem
    {
        /// <summary>
        /// 物料号
        /// </summary>
        [FieldOrder(0)]
        private string _partNo;

        public string PartNo
        {
            get { return this._partNo; }
            set { this._partNo = value; }
        }
        /// <summary>
        /// 品名
        /// </summary>
        [FieldOrder(1)]
        [FieldNullValue("")]
        private string _partDesc1;

        public string PartDesc1
        {
            get { return this._partDesc1; }
            set { this._partDesc1 = value; }
        }
        /// <summary>
        /// 规格
        /// </summary>
        [FieldOrder(2)]
        [FieldNullValue("")]
        private string _partDesc2;
        public string PartDesc2
        {
            get { return this._partDesc2; }
            set { this._partDesc2 = value; }
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        [FieldOrder(3)]
        private string _partUm;
        public string PartUm
        {
            get { return this._partUm; }
            set { this._partUm = value; }
        }

        [FieldOrder(4)]
        [FieldNotInFile]
        private int _isAdd;
        public int IsAdd
        {
            get { return this._isAdd; }
            set { this._isAdd = value; }
        }
    }
}
