using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using EStudio.Framework;

namespace GDS.WMS.Services
{
    public class ServicesBase<T>
        where T : class , new()
    {
        private readonly IFunc<T> _func;
        public ServicesBase() { }

        public ServicesBase(IFunc<T> func)
        {
            _func = func;
        }
        #region 新增 删除 更新


        public bool Add(string statament, IList<T> model)
        {
            return _func.Add(statament, model);
        }

        public bool Update(string statament, IList<T> model)
        {
            return _func.Update(statament, model);
        }
        public bool Delete(string statement, Hashtable hashtable)
        {
            return _func.Delete(statement, hashtable);
        }
        #endregion

        public T FetchOne(string statement, Hashtable hashtable)
        {
            var entities = this._func.Fetch(statement, hashtable);
            return entities.Count > 0 ? entities[0] : null;
        }
        public IList<T> FetchMany(string statement, Hashtable hashtable)
        {
            return this._func.Fetch(statement, hashtable);
        }

        public DataTable FetchToDataTable(string cmd, params SqlParameter[] parameters)
        {
            return this._func.Fetch(cmd, parameters);
        }
    }
}
