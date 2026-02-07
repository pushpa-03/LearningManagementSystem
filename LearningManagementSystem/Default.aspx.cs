using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LMS
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.Cookies["LMS_User"] != null)
            {
                txtUsername.Text = Request.Cookies["LMS_User"].Value;
                chkRemember.Checked = true;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                lblMsg.Text = "Username and password required.";
                return;
            }

            byte[] passwordHash = HashPassword(password);

            using (SqlConnection con = AdminDBHelper.GetConnection())
            {
                string sql = @"
                    SELECT 
                        U.UserId,
                        U.Username,
                        R.RoleName,
                        U.IsActive,
                        U.IsFirstLogin
                    FROM Users U
                    INNER JOIN Roles R ON U.RoleId = R.RoleId
                    WHERE (U.Username=@u OR U.Email=@u)
                      AND U.PasswordHash=@p";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", passwordHash);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (!dr.Read())
                {
                    lblMsg.Text = "Invalid username or password.";
                    return;
                }

                if (!(bool)dr["IsActive"])
                {
                    lblMsg.Text = "Your account is inactive. Contact administrator.";
                    return;
                }

                string role = dr["RoleName"].ToString();

                // 🚫 BLOCK Teacher & Student for now
                if (role == "Teacher" || role == "Student")
                {
                    lblMsg.Text = "Access denied. Please contact Admin.";
                    return;
                }

                // SESSION
                Session["UserId"] = dr["UserId"];
                Session["Username"] = dr["Username"];
                Session["Role"] = role;

                // COOKIE
                if (chkRemember.Checked)
                {
                    HttpCookie ck = new HttpCookie("LMS_User", username);
                    ck.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(ck);
                }
                else
                {
                    if (Request.Cookies["LMS_User"] != null)
                        Response.Cookies["LMS_User"].Expires = DateTime.Now.AddDays(-1);
                }

                dr.Close();

                // Update login audit
                SqlCommand up = new SqlCommand(
                    @"UPDATE Users 
                      SET LastLogin=GETDATE(), IsFirstLogin=0 
                      WHERE Username=@u",
                    con);
                up.Parameters.AddWithValue("@u", username);
                up.ExecuteNonQuery();

                // ✅ SAFE REDIRECTS (ONLY EXISTING PAGES)
                if (role == "SuperAdmin")
                {
                    Response.Redirect("~/SuperAdmin/AddSociety.aspx", false);
                }
                else if (role == "Admin")
                {
                    Response.Redirect("~/Admin/Dashboard.aspx", false);
                }
            }
        }

        private byte[] HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
