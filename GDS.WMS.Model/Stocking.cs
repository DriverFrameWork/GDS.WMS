using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDS.WMS.Model
{
    public class Stocking
    {
        /// <summary>
        /// Id主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// 盘点数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string Lotser { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// 入库时间(年月）
        /// </summary>
        public string InputTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
