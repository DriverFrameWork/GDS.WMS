using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EStudio.Framework
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            this.IsSuccess = false;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回的条数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
