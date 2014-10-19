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
    /// ����IBatisNet�����ݷ��ʻ��� 
    /// </summary>
    public class BaseSQLMapDao
    {
     

        #region ����

        public BaseSQLMapDao()
        {
            
        }

        //public SqlMapper GetMapper()
        //{
        //    DomSqlMapBuilder d = new DomSqlMapBuilder();//��ʼ��һ��DomSqlMapBuilder
        //    sqlMap = d.Configure(fileName);//����Configure������ָ�������ļ�������,����һ��SqlMapper
        //    //SqlMapper sm=Mapper.Get();       
        //    // SqlMapper sm=Mapper.Instance();
        //    return sm;
        //}
        //Mapper.Get()������Mapper.Instance()���ƣ��÷���������Ĭ�ϵ�Sql.config�ļ�����SqlMapper
        //��ʹ��DomSqlMapBuilder��������ǣ�Mapper.Get(������SqlMapperÿ�ζ�Ҫ�ȷ���ӳ���XML�ļ���
        //���ܽ����Ľ���)����Ҫָ�������ļ������ƣ�����ʹ��Mapper.Get()����SqlMapper��
        //���ӳ���XMLû�д���Ļ����Ὣ��XML�ļ����浽�ڴ棬�´ε��õ�ʱ��Ͳ���Ҫ�ڼ��XML�ļ���
        //ֱ��SqlMap.config���ı�,��������������˳�������ܣ���ʹ��DomSqlMapBuilder

        #endregion


       

        /// <summary>
        /// ִ�����
        /// </summary>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
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
        /// ִ����ӣ������Զ�������
        /// </summary>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
        /// <returns>�����Զ�������</returns>
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
        /// ִ���޸�
        /// </summary>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��ɾ��
        /// </summary>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
        /// <returns>����Ӱ������</returns>
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
        /// �õ��б�
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="statementName">�������ƣ���Ӧxml�е�Statement��id</param>
        /// <param name="parameterObject">����</param>
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
        /// �õ�ָ�������ļ�¼��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="parameterObject">����</param>
        /// <param name="skipResults">�����ļ�¼��</param>
        /// <param name="maxResults">��󷵻صļ�¼��</param>
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
        /// ��ѯ�õ������һ��ʵ��
        /// </summary>
        /// <typeparam name="T">����type</typeparam>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
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
        /// ����һ��DataSet
        /// </summary>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
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
        /// �õ����������SQL
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
        /// ���ڷ�ҳ�ؼ�ʹ��
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="pageSize">ÿҳ��ʾ��Ŀ</param>
        /// <param name="curPage">��ǰҳ</param>
        /// <param name="recCount">��¼����</param>
        /// <returns>�õ���DataTable</returns>
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
        /// ��ȡ���ӳ��
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
