using System.Configuration;

namespace EStudio.Framework.Configuration
{
    public class WebConfigApplicationSettings : IApplicationSettings
    {
        public string LoggerName
        {
            get
            {
                var loggerName = ConfigurationManager.AppSettings["LoggerName"];
                return loggerName;
            }
        }
        public string ConnectionString
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"];
                return connectionString != null ? connectionString.ConnectionString : null;
            }
        }

        public string MapperConnectionStrng
        {
            get
            {
                var connection = string.Empty;
                return connection = ConfigurationManager.ConnectionStrings["ConnectionString"] != null ?
                ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString : connection;
            }
        }
    }
}
