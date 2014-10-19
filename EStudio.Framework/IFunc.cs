using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace EStudio.Framework
{
    public interface IFunc<T> where T : class 
    {
        #region 增，删，改操作

        /// <summary>
        /// 实体列表
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        bool Update(string statement,IList<T> modelList);

        /// <summary>
        /// 实体列表
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        bool Add(string statement,IList<T> modelList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        bool Delete(string statement, Hashtable hashtable);

        #endregion

        #region 查询操作

        IList<T> Fetch(string statement, Hashtable hashtable);

        DataTable Fetch(string cmd, params SqlParameter[] parameters);

        #endregion
    }
}
