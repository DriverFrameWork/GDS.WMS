using System;
using System.Data;
using System.IO;
using System.Reflection;
using EStudio.Framework.Configuration;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.SessionStore;

namespace GDS.WMS.Persistence.Dao
{
    public class DaoManager
    {
        protected static ISqlMapper _SqlMapper;
        protected static string SqlMapperFileName = "GDS.WMS.Persistence.Dao.sqlMap.dao.config";
        protected static object obj = new object();

        public static ISqlMapper GetSqlMapper()
        {

            try
            {
                lock (obj)
                {
                    return _SqlMapper ?? (_SqlMapper = GetMapperInstance(SqlMapperFileName));
                }

            }
            catch(Exception exception)
            {
                
            }
            return null;
        }

        private static ISqlMapper GetMapperInstance(string file)
        {
            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(file);
            ISqlMapper sqlMap = builder.Configure(stream);
            if (!string.IsNullOrEmpty(ApplicationSettingsFactory.GetApplicationSettings().ConnectionString))
            {
                sqlMap.DataSource.ConnectionString = ApplicationSettingsFactory.
                    GetApplicationSettings().ConnectionString;
            }
            CallContextSessionStore sessionStore = new CallContextSessionStore(sqlMap.Id);
            sqlMap.SessionStore = sessionStore;
            return sqlMap;
        }

        public static ISqlMapSession GetSession()
        {
            ISqlMapSession session;
            if (DaoManager.GetSqlMapper().LocalSession == null)
            {
                session = new SqlMapSession(DaoManager.GetSqlMapper());
                if (session.Connection == null || session.Connection.State == ConnectionState.Closed)
                {
                    session.OpenConnection();
                }
            }
            else
                session = DaoManager.GetSqlMapper().LocalSession;
            return session;
        }
    }
}
