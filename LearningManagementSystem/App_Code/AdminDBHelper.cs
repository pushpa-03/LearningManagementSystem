using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class AdminDBHelper
{

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(
            ConfigurationManager.ConnectionStrings["LMS_DB"].ConnectionString
        );
    }
    private static readonly string conStr =
           ConfigurationManager.ConnectionStrings["LMS_DB"].ConnectionString;

    //private static string conStr;

    static AdminDBHelper()
    {
        var cs = ConfigurationManager.ConnectionStrings["LMS_DB"];
        if (cs == null)
            throw new Exception("Connection string 'LMS_DB' not found in Web.config");

        conStr = cs.ConnectionString;
    }
    public static DataTable ExecuteSelect(string query, SqlParameter[] prm)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            if (prm != null)
                cmd.Parameters.AddRange(prm);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    public static int ExecuteQuery(string query, SqlParameter[] prm)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            if (prm != null)
                cmd.Parameters.AddRange(prm);

            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }

    // ✅ REQUIRED
    //public static int ExecuteScalar(string query, SqlParameter[] prm)
    //{
    //    using (SqlConnection con = new SqlConnection(conStr))
    //    using (SqlCommand cmd = new SqlCommand(query, con))
    //    {
    //        if (prm != null)
    //            cmd.Parameters.AddRange(prm);

    //        con.Open();
    //        return Convert.ToInt32(cmd.ExecuteScalar());
    //    }
    //}
    public static int ExecuteScalar(string query, SqlParameter[] parameters = null)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            con.Open();
            object result = cmd.ExecuteScalar();
            return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }
    }

    public static DataTable GetTable(string q, SqlParameter[] p = null)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        using (SqlCommand cmd = new SqlCommand(q, con))
        {
            if (p != null) cmd.Parameters.AddRange(p);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    public static int Execute(string q, SqlParameter[] p = null)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        using (SqlCommand cmd = new SqlCommand(q, con))
        {
            if (p != null) cmd.Parameters.AddRange(p);
            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}


