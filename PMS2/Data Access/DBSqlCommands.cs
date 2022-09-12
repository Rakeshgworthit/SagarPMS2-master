using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace PMS.Data_Access
{
    public class DBSqlCommand : IDisposable
    {
        private SqlCommand _sqlCommand = null;
        private SqlFunctions _sqlFunctions = null;

        public DBSqlCommand(CommandType commandType = CommandType.StoredProcedure)
        {
            _sqlFunctions = new SqlFunctions();
            _sqlCommand = new SqlCommand();
            _sqlCommand.CommandTimeout = 1200; //Default is 30 Secs.
            _sqlCommand.Connection = _sqlFunctions.GetConnection();
            _sqlCommand.CommandType = commandType;
        }

        public void AddParameters(object value, string parameter_name, SqlDbType _type, ParameterDirection paramDirection = ParameterDirection.Input, int size = 0)
        {
            _sqlCommand.Parameters.Add(parameter_name, _type);
            _sqlCommand.Parameters[parameter_name].Value = value;
            _sqlCommand.Parameters[parameter_name].Direction = paramDirection;
            if (size > 0)
                _sqlCommand.Parameters[parameter_name].Size = size;
        }

        public SqlCommand sqlCommand
        {
            get
            {
                return _sqlCommand;
            }
        }

        public DataTable ExecuteDataTable(string commandText)
        {
            DataTable dTable = null;
            _sqlCommand.CommandText = commandText;
            using (SqlDataAdapter adapter = new SqlDataAdapter(_sqlCommand))
            {
                dTable = new DataTable();
                adapter.Fill(dTable);
            }
            return dTable;
        }

        public DataSet ExecuteDataSet(string commandText)
        {
            DataSet Ds = null;
            _sqlCommand.CommandText = commandText;
            using (SqlDataAdapter adapter = new SqlDataAdapter(_sqlCommand))
            {
                Ds = new DataSet();
                adapter.Fill(Ds);
            }
            return Ds;
        }

        public bool ExecuteNonQuery(string commandText)
        {
            _sqlCommand.CommandText = commandText;
            int _count = _sqlCommand.ExecuteNonQuery();
            if (_count > 0)
                return true;
            return false;
        }

        public object ExecuteScalar(string commandText)
        {
            _sqlCommand.CommandText = commandText;
            return _sqlCommand.ExecuteScalar();
        }

        public IDataReader ExecuteDataReader(string commandText)
        {
            _sqlCommand.CommandText = commandText;
            return _sqlCommand.ExecuteReader();
        }

        public XmlReader ExecuteXmlReader(string commandText)
        {
            _sqlCommand.CommandText = commandText;
            return _sqlCommand.ExecuteXmlReader();
        }

        public void Dispose()
        {
            _sqlFunctions.CloseConnection();
            GC.SuppressFinalize(this);
        }
    }
}