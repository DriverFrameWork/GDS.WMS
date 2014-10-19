using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using FileHelpers;

namespace GDS.WMS.Model
{
    public class AffairItem
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// QAD单据号
        /// </summary>
        public string QADNo { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// 项目次
        /// </summary>
        public string SNID { get; set; }

        /// <summary>
        /// 事务数量
        /// </summary>
        public decimal AffairQty { get; set; }

        /// <summary>
        /// 事务开始时间
        /// </summary>
        public string AffairStartTime { get; set; }

        /// <summary>
        /// 事务结束时间
        /// </summary>
        public string AffairEndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 事务类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public string InputTime { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 批序号
        /// </summary>
        public string Lotser { get; set; }

        /// <summary>
        /// 参考
        /// </summary>
        public string Ref { get; set; }

    }
}
