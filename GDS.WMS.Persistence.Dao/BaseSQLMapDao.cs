using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using EStudio.Framework.DbHelper;
using IBatisNet.Common;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;

namespace GDS.WMS.Persistence.Dao
{
    /// <summary>
    /// 基于IBatisNet的数据访问基类 
    /// </summary>
    public class BaseSQLMapDao
    {
     

        #region 构造

        public BaseSQLMapDao()
        {
            
        }

        //public SqlMapper GetMapper()
        //{
        //    DomSqlMapBuilder d = new DomSqlMapBuilder();//初始化一个DomSqlMapBuilder
        //    sqlMap = d.Configure(fileName);//调用Configure方法并指定配置文件的名称,返回一个SqlMapper
        //    //SqlMapper sm=Mapper.Get();       
        //    // SqlMapper sm=Mapper.Instance();
        //    return sm;
        //}
        //Mapper.Get()方法与Mapper.Instance()类似，该方法将调用默认的Sql.config文件建立SqlMapper
        //与使用DomSqlMapBuilder类的区别是，Mapper.Get(建立的SqlMapper每次都要先分析映射的XML文件，
        //性能将大大的降低)不需要指定配置文件的名称，并且使用Mapper.Get()返回SqlMapper后
        //如果映射的XML没有错误的话，会将该XML文件缓存到内存，下次调用的时候就不需要在检查XML文件，
        //直到SqlMap.config被改变,这样将大大的提高了程序的性能，而使用DomSqlMapBuilder

        #endregion


       

        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        protected object ExecuteInsert(string statementName, object parameterObject)
        {
            try
            {
                return DaoManager.GetSqlMapper().Insert(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for insert.  Cause: " + e.Message, e);
            }
        }
        /// <summary>
        /// 执行添加，返回自动增长列
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        /// <returns>返回自动增长列</returns>
        protected int ExecuteInsertForInt(string statementName, object parameterObject)
        {
            try
            {
                object obj = DaoManager.GetSqlMapper().Insert(statementName, parameterObject);

                return 1;
             
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for insert.  Cause: " + e.Message, e);
            }
        }
        /// <summary>
        /// 执行修改
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        /// <returns>返回影响行数</returns>
        protected int ExecuteUpdate(string statementName, object parameterObject)
        {
            try
            {
                return DaoManager.GetSqlMapper().Update(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for update.  Cause: " + e.Message, e);
            }
        }


        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        /// <returns>返回影响行数</returns>
        protected int ExecuteDelete(string statementName, object parameterObject)
        {
            try
            {
                DaoManager.GetSqlMapper().Delete(statementName, parameterObject);
                return 1;
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for delete.  Cause: " + e.Message, e);
            }
        }


        /// <summary>
        /// 得到列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="statementName">操作名称，对应xml中的Statement的id</param>
        /// <param name="parameterObject">参数</param>
        /// <returns></returns>
        protected IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject)
        {
            try
            {
                return DaoManager.GetSqlMapper().QueryForList<T>(statementName, parameterObject);
            }
            catch (Exception e)
             {
                throw new DataMapperException("Error executing query '" + statementName + "' for list.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        /// 得到指定数量的记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="parameterObject">参数</param>
        /// <param name="skipResults">跳过的记录数</param>
        /// <param name="maxResults">最大返回的记录数</param>
        /// <returns></returns>
        protected IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject, int skipResults, int maxResults)
        {
            try
            {
                return DaoManager.GetSqlMapper().QueryForList<T>(statementName, parameterObject, skipResults, maxResults);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for list.  Cause: " + e.Message, e);
            }
        }


        /// <summary>
        /// 查询得到对象的一个实例
        /// </summary>
        /// <typeparam name="T">对象type</typeparam>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        /// <returns></returns>
        protected T ExecuteQueryForObject<T>(string statementName, object parameterObject)
        {
            try
            {
                return DaoManager.GetSqlMapper().QueryForObject<T>(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for object.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        /// 返回一个DataSet
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        /// <returns></returns>
        protected DataSet ExecuteQueryForDataSet(string statementName, object parameterObject)
        {
            DataSet ds = new DataSet();
            IMappedStatement statement = DaoManager.GetSqlMapper().GetMappedStatement(statementName);
            if (!DaoManager.GetSqlMapper().IsSessionStarted)
            {
                DaoManager.GetSqlMapper().OpenConnection();
            }
            RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, parameterObject, DaoManager.GetSqlMapper().LocalSession);
            statement.PreparedCommand.Create(scope, DaoManager.GetSqlMapper().LocalSession, statement.Statement, parameterObject);
            IDbCommand cmdCount = GetDbCommand(statementName, parameterObject);
            cmdCount.Connection = DaoManager.GetSqlMapper().LocalSession.Connection;
            if (cmdCount.Connection.State == ConnectionState.Closed)
            {
                cmdCount.Connection.Open();
            }
            DataTable dt = new DataTable(statementName);
            dt.Load(cmdCount.ExecuteReader());

            ds.Tables.Add(dt);
            cmdCount.Connection.Close();
            return ds;
        }

        /// <summary>
        /// 得到参数化后的SQL
        /// </summary>
        protected string GetSql(string statementName, object paramObject)
        {
            IStatement statement = DaoManager.GetSqlMapper().GetMappedStatement(statementName).Statement;

            IMappedStatement mapStatement = DaoManager.GetSqlMapper().GetMappedStatement(statementName);

            ISqlMapSession session = DaoManager.GetSession();

            RequestScope request = statement.Sql.GetRequestScope(mapStatement, paramObject, session);

            return request.PreparedStatement.PreparedSql;

        }


     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statementName"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        protected IDbCommand GetDbCommand(string statementName, object paramObject)
        {
            IStatement statement = DaoManager.GetSqlMapper().GetMappedStatement(statementName).Statement;

            IMappedStatement mapStatement = DaoManager.GetSqlMapper().GetMappedStatement(statementName);

            ISqlMapSession session = DaoManager.GetSession();

            RequestScope request = statement.Sql.GetRequestScope(mapStatement, paramObject, session);

            mapStatement.PreparedCommand.Create(request, session, statement, paramObject);

            return request.IDbCommand;

        }

        /**/
        /// <summary>
        /// 用于分页控件使用
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pageSize">每页显示数目</param>
        /// <param name="curPage">当前页</param>
        /// <param name="recCount">记录总数</param>
        /// <returns>得到的DataTable</returns>
        protected IList<T> ExecuteQueryForList<T>(string statementName, object paramObject, int pageSize, int curPage, ref int recCount)
        {
            IDataReader dr = null;
            bool isSessionLocal = false;
            string sql = GetSql(statementName, paramObject);
            string strCount = "select count(*) " + sql.Substring(sql.ToLower().IndexOf("from"));

            IDalSession session = DaoManager.GetSession();
            var dataTable = new DataTable();
            if (session == null)
            {
                session = new SqlMapSession(DaoManager.GetSqlMapper());
                session.OpenConnection();
                isSessionLocal = true;
            }
            try
            {
                if (session.Connection == null || session.Connection.State == ConnectionState.Closed)
                {
                    session.OpenConnection();
                    isSessionLocal = true;
                }
                IDbCommand cmdCount = GetDbCommand(statementName, paramObject);
                cmdCount.Connection = session.Connection;
                cmdCount.CommandText = strCount;
                if (recCount == 0)
                {
                    object count = cmdCount.ExecuteScalar();
                    recCount = Convert.ToInt32(count);
                }
                IDbCommand cmd = GetDbCommand(statementName, paramObject);
                cmd.Connection = session.Connection;
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataTable = SqlHelper.GetPagingTable(dr, pageSize, curPage);
                dr.Close();
            }
            finally
            {
                if (isSessionLocal)
                {
                    if (session.Connection != null && session.Connection.State == ConnectionState.Open)
                        session.CloseConnection();
                }
            }

            IList<T> list = new List<T>();
            int statementCount = DaoManager.GetSqlMapper().GetMappedStatement(statementName).Statement.ResultsMap.Count;
            if (statementCount > 0)
            {
                dict.Clear();
                GetResultMap(DaoManager.GetSqlMapper().GetMappedStatement(statementName).Statement.ResultsMap[0]);
            }
            foreach (DataRow row in dataTable.Rows)
            {
                T t = Activator.CreateInstance<T>();
                Type type = t.GetType();
                list.Add(t);
                foreach (DataColumn cell in row.Table.Columns)
                {
                    PropertyInfo propertyInfo = type.GetProperty(dict[cell.Caption.ToUpper()]);
                    if (propertyInfo != null)
                    {
                        if (!string.IsNullOrEmpty(row[cell.Caption].ToString()))
                        {
                            if (propertyInfo.PropertyType != typeof(System.Byte[]))
                                propertyInfo.SetValue(t, Convert.ChangeType(row[cell.Caption].ToString(), propertyInfo.PropertyType), null);
                            else
                                propertyInfo.SetValue(t, Convert.ChangeType(row[cell.Caption], propertyInfo.PropertyType), null);
                        }
                    }
                }
            }

            return list;
        }



        IDictionary<string, string> dict = new Dictionary<string, string>();
        /// <summary>
        /// 获取结果映射
        /// </summary>
        /// <param name="resultMap"></param>
        /// <returns></returns>
        private void GetResultMap(IResultMap resultMap)
        {
            for (int i = 0; i < resultMap.Properties.Count; i++)
            {
                if (resultMap.Properties[i].NestedResultMap != null)
                {
                    GetResultMap(resultMap.Properties[i].NestedResultMap);
                }
                else
                {
                    dict.Add(resultMap.Properties[i].ColumnName.ToUpper(), resultMap.Properties[i].PropertyName);
                }
            }
        }
    }
}
