﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PMS.Data_Access
{
    internal class SqlFunctions
    {
        SqlConnection sqlcon = null;

        public SqlConnection GetConnection()
        {
            string strconstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            sqlcon = new SqlConnection(strconstr);
            try
            {
                sqlcon.Open();
            }
            catch (SqlException ex)
            {
                throw;
            }

            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return sqlcon;
        }

        public void CloseConnection()
        {
            try
            {
                if (sqlcon.State == ConnectionState.Open)
                {
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}
