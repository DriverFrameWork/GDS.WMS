using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using FileHelpers;

namespace GDS.WMS.Model
{
    // ReSharper disable ConvertToAutoProperty
    [DelimitedRecord(",")]
    public class BusinessDet
    {
        [FieldOrder(0)]

        private string _type;
        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        /// <summary>
        /// QAD单据号
        /// </summary>
        [FieldOrder(1)]
        private string _qadNo;
        public string QADNo
        {
            get { return this._qadNo.ToUpper(); }
            set { this._qadNo = value; }
        }

        /// <summary>
        /// 项次
        /// </summary>
        [FieldOrder(2)]
        [FieldNullValue(0)]
        private int _line;
        public int Line
        {
            get { return this._line; }
            set { this._line = value; }
        }

        /// <summary>
        /// 库位
        /// </summary>
        [FieldOrder(3)]
        [FieldNullValue("")]
        public string _loc;
        public string Loc
        {
            get { return this._loc; }
            set { this._loc = value; }
        }
        /// <summary>
        /// 批序号
        /// </summary>
        [FieldOrder(4)]
        [FieldNullValue("")]
        public string _Lotser;
        public string Lotser
        {
            get { return this._Lotser; }
            set { this._Lotser = value; }
        }

        /// <summary>
        /// 批序号
        /// </summary>
        [FieldOrder(5)]
        [FieldNullValue("")]
        public string _ref;
        public string Ref
        {
            get { return this._ref; }
            set { this._ref = value; }
        }
        [FieldOrder(6)]
        [FieldNullValue("")]
        private string _partNo;
        public string PartNo
        {
            get { return this._partNo.ToUpper(); }
            set { this._partNo = value; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        [FieldOrder(37)]
        private decimal _qty;
        public decimal Qty
        {
            get { return this._qty; }
            set { this._qty = value; }
        }
        /// <summary>
        ///  实际累计入库数量
        /// </summary>
        [FieldOrder(8)]
        [FieldNotInFile]
        private decimal _storyageQty;
        public decimal StorageQty
        {
            get { return this._storyageQty; }
            set { this._storyageQty = value; }
        }
        /// <summary>
        /// 状态。0：未完全备料；1：完全备料
        /// </summary>
        [FieldOrder(9)]
        [FieldNotInFile]
        private int _status;
        public int Status
        {
            get { return this._status; }
            set { this._status = value; }
        }

        /// <summary>
        /// 更新人
        /// </summary>
        [FieldOrder(10)]
        [FieldNotInFile]
        private string _updateUser;
        public string UpdateUser
        {
            get { return this._updateUser; }
            set { this._updateUser = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [FieldOrder(11)]
        [FieldNotInFile]
        private string _updateTime;
        public string UpdateTime
        {
            get { return this._updateTime; }
            set { this._updateTime = value; }
        }
    }
    // ReSharper restore ConvertToAutoProperty
}
