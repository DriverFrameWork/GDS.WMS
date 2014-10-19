using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace EStudio.Framework
{
    public class EntityBase
    {
        
        public virtual string nameSpace { get;set; }
        /// <summary>
        ///  获取自身命名空间
        /// </summary>
        public virtual string GetSelfNameSpace()
        {
            return this.nameSpace;
        }
    }
}
