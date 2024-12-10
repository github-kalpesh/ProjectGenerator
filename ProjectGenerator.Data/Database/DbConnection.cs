using BLToolkit.Data;
using ProjectGenerator.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectGenerator.Template;
using Npgsql;

namespace ProjectGenerator.Data.Database
{
    public class DbConnection
    {
        public readonly string ConnectionString;
        public DbConnection(string Conn) 
        {
            ConnectionString = Conn;
        }

        public List<TableColumnInfo> GetTableInfo(string tableName)
        {
            var sqlQuery = "SELECT  \r\nTABLE_NAME AS TableName,\r\nCOLUMN_NAME AS ColumnName,\r\nORDINAL_POSITION AS OrdinalPosition,\r\nCOLUMN_DEFAULT AS DefaultValue,\r\n(CASE WHEN IS_NULLABLE = 'NO' THEN 0 ELSE 1 END) AS IsAllowNull,\r\nDATA_TYPE AS DataType,\r\nCHARACTER_MAXIMUM_LENGTH AS MaxLength,\r\nCHARACTER_OCTET_LENGTH,\r\nNUMERIC_PRECISION,\r\nNUMERIC_PRECISION_RADIX,\r\nNUMERIC_SCALE,\r\nDATETIME_PRECISION,\r\nCHARACTER_SET_NAME,\r\nCOLLATION_CATALOG,\r\nCOLLATION_SCHEMA,\r\nCOLLATION_NAME,\r\nDOMAIN_CATALOG,\r\nDOMAIN_SCHEMA,\r\nDOMAIN_NAME\r\nFROM  INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + tableName + "'";
            return ExecuteList<TableColumnInfo>(sqlQuery);
        }
        public List<TableInfo> GetTables() 
        {
            var sqlQuery = "SELECT TABLE_NAME AS TableName, REPLACE(TABLE_NAME,'t_','') AS DisplayTableName FROM  INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'";
            return ExecuteList<TableInfo>(sqlQuery);
        }

        public List<T> ExecuteList<T>(string SqlQuery)
        {
            List<T> returnObj = null;
            DataTable dataTable = null;
            dataTable = ExecuteDatatable(SqlQuery);
            if (dataTable != null &&  dataTable.Rows.Count > 0)
            {
                returnObj = Utill.ConvertDataTable<T>(dataTable);
            }
            return returnObj;
        }
        public T ExecuteItem<T>(string SqlQuery)
        {
            List<T> returnObj = null;
            DataTable dataTable = null;
            dataTable = ExecuteDatatable(SqlQuery);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                returnObj = Utill.ConvertDataTable<T>(dataTable);
            }
            return returnObj.FirstOrDefault();
        }

        public DataTable ExecuteDatatable(string SqlQuery)
        {
            using (SqlConnection _con = new SqlConnection(ConnectionString))
            {
                string queryStatement = SqlQuery;
                using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                {
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    _con.Open();
                    _dap.Fill(dataTable);
                    _con.Close();
                    return dataTable;
                }
            }
        }
        public DataTable ExecuteDatatableNpgsql(string SqlQuery)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = SqlQuery;
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        // Process the results of the query
                        while (reader.Read())
                        {
                            DataRow dataRow = dataTable.NewRow();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dataRow[i] = reader[i];
                            }

                            dataTable.Rows.Add(dataRow);
                            // Access data from each row (e.g., reader.GetInt32(0) for the first column)
                        }
                        return dataTable;
                    }
                }
            }
        }
    }
}
