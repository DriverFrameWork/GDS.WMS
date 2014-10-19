using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace EStudio.Framework.DbHelper
{
    public class SqlHelper
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand();
            using (var conn = new SqlConnection(ConnectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                var val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand();

            using (var conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                var val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand();
            var conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            var cmd = new SqlCommand();

            using (var connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
                var val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {

            var cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        public static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] parameters)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (parameters == null) return;
            foreach (var param in parameters)
                cmd.Parameters.Add(param);
        }

        public static DataTable GetPagingTable(IDataReader reader, int pageIndex, int pageSize)
        {
            var dataTable = new DataTable();
            var columnTotal = reader.FieldCount;
            for (var i = 0; i < columnTotal; i++)
            {
                dataTable.Columns.Add(!dataTable.Columns.Contains(reader.GetName(i))
                    ? new DataColumn(reader.GetName(i), reader.GetFieldType(i))
                    : new DataColumn(reader.GetName(i) + 1, reader.GetFieldType(i)));
            }
            // 读取数据。将DataReader中的数据读取到DataTable中
            object[] val = new object[columnTotal];
            int iCount = 0; // 临时记录变量
            while (reader.Read())
            {
                // 当前记录在当前页记录范围内

                if (iCount >= pageSize * (pageIndex - 1) && iCount < pageSize * pageIndex)
                {
                    for (int i = 0; i < columnTotal; i++)
                        val[i] = reader.GetValue(i);

                    dataTable.Rows.Add(val);
                }
                else if (iCount > pageSize * columnTotal)
                {
                    break;
                }
                iCount++; // 临时记录变量递增
            }

            if (reader.IsClosed) return dataTable;
            reader.Close();
            reader.Dispose();
            return dataTable;
        }

        public static DataTable Query(string sql, SqlParameter[] parameters)
        {
            var cmd = new SqlCommand(sql);
            using (var connection = new SqlConnection(ConnectionString))
            {
                PrepareCommand(cmd, connection, null, CommandType.Text, sql, parameters);
                var dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = cmd;
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "DATATABLE");
                cmd.Parameters.Clear();
                return dataSet.Tables["DATATABLE"];
            }
        }

        public static DataTable Query(string sql)
        {
            var cmd = new SqlCommand(sql);
            using (var connection = new SqlConnection(ConnectionString))
            {
                var dataAdapter = new SqlDataAdapter {SelectCommand = cmd};
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "DATATABLE");
                cmd.Parameters.Clear();
                return dataSet.Tables["DATATABLE"];
            }
        }
    }
}
