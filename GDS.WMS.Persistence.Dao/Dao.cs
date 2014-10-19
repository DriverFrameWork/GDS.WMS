using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using EStudio.Framework;
using EStudio.Framework.DbHelper;

namespace GDS.WMS.Persistence.Dao 
{
    public class Dao<T> : BaseSQLMapDao, IFunc<T> where T : class , new()
    {
        public static string POSTFIX_INSERT = ".insert";

        public static string POSTFIX_UPDATE = ".update";

        public static string POSTFIX_DELETE = ".delete";

        #region 私有方法
    
        #endregion

        #region 接口实现

        public bool Update(string statament,IList<T> modelList)
        {
            if (modelList == null)
                return false;
            else if (modelList.Count == 0)
                return true;
            return ExecuteUpdate(statament + POSTFIX_UPDATE, modelList) > 0;
        }

        public bool Add(string statement,IList<T> modelList)
        {
            if (modelList == null)
                return false;
            else if (modelList.Count == 0)
                return true;
            return ExecuteInsertForInt(statement + POSTFIX_INSERT, modelList) > 0;
        }

        public bool Delete(string statement, Hashtable hashtable)
        {
            return ExecuteDelete(statement, hashtable) > 0;
        }

        public IList<T> Fetch(string statement, Hashtable hashtable)
        {
            return ExecuteQueryForList<T>(statement, hashtable);
        }
      
        #endregion

        public System.Data.DataTable Fetch(string cmd, params  SqlParameter[] parameters)
        {
            return SqlHelper.Query(cmd, parameters);
        }
    }
}
