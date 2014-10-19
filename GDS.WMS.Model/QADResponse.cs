using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using FileHelpers;

namespace GDS.WMS.Model
{
    [DelimitedRecord(",")]
    public class QADResponse
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public int Id { get; set; }
        public int Status { get; set; }

    }
}
