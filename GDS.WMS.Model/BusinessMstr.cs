using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using FileHelpers;

namespace GDS.WMS.Model
{
    // ReSharper disable ConvertToAutoProperty
    [DelimitedRecord(",")]
    public class BusinessMstr
    {
        /// <summary>
        /// QAD单据号
        /// </summary>
        [FieldOrder(1)]
        private string _qadNo;
        public string QADNo
        {
            get { return this._qadNo; }
            set { this._qadNo = value; }
        }

        /// <summary>
        /// 事务类型(POI：采购入库；PNI：计划外入库；ACI：调拨入；WOO：工单发料；PNO：计划外出库；ACO：调拨出；DPO:部门领用；SMO超领发料)
        /// </summary>
        [FieldOrder(0)]
        private string _type;
        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        [FieldOrder(2)]
        [FieldNotInFile]
        private string _userId;
        public string UserId
        {
            get { return this._userId; }
            set { this._userId = value; }
        }

        /// <summary>
        /// 操作时间
        /// </summary>
        [FieldOrder(3)]
        [FieldNotInFile]
        private string _inputTime;
        public string InputTime
        {
            get { return this._inputTime; }
            set { this._inputTime = value; }
        }

        /// <summary>
        /// 状态(0：未启动；1：启动)
        /// </summary>
        [FieldOrder(4)]
        [FieldNotInFile]
        private int _status;
        public int Status
        {
            get { return this._status; }
            set { this._status = value; }
        }
    }
}
