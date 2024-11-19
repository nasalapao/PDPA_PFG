using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;

public class dbConnection
{
    private string sqlConnectionString;
    private string sqlCyberConnectionString;
    private string db2ConnectionString;

    public bool isError = false ;
    public bool isHasRow = false;

    public dbConnection()
    {
        // ดึง connection string จาก web.config
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConnectSQLSERVER"].ConnectionString;
        sqlCyberConnectionString = ConfigurationManager.ConnectionStrings["ConnectSQLCyber"].ConnectionString;
        db2ConnectionString = ConfigurationManager.ConnectionStrings["ConnectDB2"].ConnectionString;
    }

    // Method สำหรับเลือก connection string ของ SQL Server ตาม serverType
    private string GetSqlConnectionString(string serverType)
    {
        switch (serverType)
        {
            case "Cyber":
                return sqlCyberConnectionString;
            case "Default":
            default:
                return sqlConnectionString;
        }
    }

    // Method สำหรับ SQL Server
    public DataTable ExecuteSqlQuery(string query, string serverType = "Default")
    {
        using (SqlConnection conn = new SqlConnection(GetSqlConnectionString(serverType)))
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    isError = false;
                    isHasRow = false;
                    return null;
                }


                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
              
                if (dt.Rows.Count > 0)
                {
                    isHasRow = true;
                    return dt;
                }
                else
                {
                    isHasRow = false;
                    return null;
                }


            }
            catch (Exception ex)
            {
                isError = true;
                throw new Exception("SQL Server Error: " + ex.Message);
                return null;
            }
        }
    }

    // Method สำหรับ execute SQL Server with parameters
    public DataTable ExecuteSqlQueryWithParams(string query, SqlParameter[] parameters, string serverType = "Default")
    {
        using (SqlConnection conn = new SqlConnection(GetSqlConnectionString(serverType)))
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(parameters);
                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL Server Error: " + ex.Message);
            }
        }
    }

    // Method สำหรับ Execute NonQuery (Insert, Update, Delete) สำหรับ SQL Server
    public int ExecuteSqlNonQuery(string query, SqlParameter[] parameters = null, string serverType = "Default")
    {
        using (SqlConnection conn = new SqlConnection(GetSqlConnectionString(serverType)))
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("SQL Server Error: " + ex.Message);
            }
        }
    }

    ///**********************************************************************************************////
    // Method สำหรับ DB2
    public DataTable ExecuteDb2Query(string query)
    {
        isError = false;
        isHasRow = false;
        using (OleDbConnection conn = new OleDbConnection(db2ConnectionString))
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    isError = false;
                    isHasRow = false;
                    return null;
                }


                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                DataTable dt = new DataTable();
                new OleDbDataAdapter(cmd).Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    isHasRow = true;
                    return dt;
                }else
                {
                    isHasRow = false;
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                isError = true;
                throw new Exception("DB2 Error: " + ex.Message);
                return null;
            }
        }
    }

    // Method สำหรับ execute DB2 with parameters
    public DataTable ExecuteDb2QueryWithParams(string query, OleDbParameter[] parameters)
    {
        using (OleDbConnection conn = new OleDbConnection(db2ConnectionString))
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddRange(parameters);
                DataTable dt = new DataTable();
                new OleDbDataAdapter(cmd).Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("DB2 Error: " + ex.Message);
            }
        }
    } // end 

    // Method สำหรับ Execute NonQuery (Insert, Update, Delete) สำหรับ DB2
    public int ExecuteDb2NonQuery(string query, OleDbParameter[] parameters)
    {
        using (OleDbConnection conn = new OleDbConnection(db2ConnectionString))
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("DB2 Error: " + ex.Message);
            }
        }
    } // end 

    public string SqlDateYYYYMMDD(DateTime sender)
    {
        var sender2 = sender;
        var sReturn = string.Format("{0}{1:00}{2:00}", sender2.Year, sender2.Month, sender2.Day);
        return sReturn;
    }//end 
    public string SqlDateYYYYMMDDHHMMSS(DateTime sender)
    {
        var sender2 = sender;
        var sReturn = string.Format("{0}{1:00}{2:00}-{3:00}{4:00}{5:00}", sender2.Year, sender2.Month, sender2.Day, sender2.Hour, sender2.Minute, sender2.Second);
        return sReturn;
    }//end

}
