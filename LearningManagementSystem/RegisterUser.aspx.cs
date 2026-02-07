using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Web.UI.WebControls;

namespace LMS
{
    public partial class RegisterUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSocieties();
            }
        }

        private void LoadSocieties()
        {
            DataTable dt = AdminDBHelper.GetTable(
                "SELECT SocietyId, SocietyName FROM Societies WHERE IsActive = 1 ORDER BY SocietyName");

            ddlSociety.DataSource = dt;
            ddlSociety.DataTextField = "SocietyName";
            ddlSociety.DataValueField = "SocietyId";
            ddlSociety.DataBind();

            ddlSociety.Items.Insert(0, new ListItem("-- Select Society --", "0"));
        }

        protected void ddlSociety_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlInstitute.Items.Clear();

            if (ddlSociety.SelectedValue == "0")
            {
                ddlInstitute.Items.Insert(0, new ListItem("-- Select Society First --", "0"));
                return;
            }

            SqlParameter[] p =
            {
                new SqlParameter("@sid", ddlSociety.SelectedValue)
            };

            DataTable dt = AdminDBHelper.GetTable(
                "SELECT InstituteId, InstituteName FROM Institutes WHERE SocietyId=@sid AND IsActive=1 ORDER BY InstituteName",
                p);

            ddlInstitute.DataSource = dt;
            ddlInstitute.DataTextField = "InstituteName";
            ddlInstitute.DataValueField = "InstituteId";
            ddlInstitute.DataBind();

            ddlInstitute.Items.Insert(0, new ListItem("-- Select Institute --", "0"));
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblMsg.CssClass = "text-danger small mb-3 d-block";
            lblMsg.Text = "";

            // ================= VALIDATIONS =================
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMsg.Text = "All fields are required.";
                return;
            }

            if (txtPassword.Text != txtConfirm.Text)
            {
                lblMsg.Text = "Passwords do not match.";
                return;
            }

            if (ddlSociety.SelectedValue == "0" || ddlInstitute.SelectedValue == "0")
            {
                lblMsg.Text = "Please select a valid Society and Institute.";
                return;
            }

            try
            {
                // ================= ROLE RESOLUTION =================
                // Signup page allows ONLY Admin & SuperAdmin
                SqlParameter[] pRole =
                {
                    new SqlParameter("@r1", "Admin"),
                    new SqlParameter("@r2", "SuperAdmin")
                };

                DataTable roleDt = AdminDBHelper.GetTable(
                    "SELECT RoleId FROM Roles WHERE RoleName IN (@r1, @r2)",
                    pRole);

                if (roleDt.Rows.Count == 0)
                {
                    lblMsg.Text = "Required roles not found. Contact system administrator.";
                    return;
                }

                // Default role for signup = Admin
                int roleId = Convert.ToInt32(
                    AdminDBHelper.ExecuteScalar(
                        "SELECT RoleId FROM Roles WHERE RoleName = 'Admin'"));

                // ================= USERNAME CHECK =================
                SqlParameter[] pCheck =
                {
                    new SqlParameter("@u", txtUsername.Text.Trim())
                };

                int exists = AdminDBHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM Users WHERE Username=@u",
                    pCheck);

                if (exists > 0)
                {
                    lblMsg.Text = "Username already exists.";
                    return;
                }

                // ================= INSERT USER =================
                SqlParameter[] pInsert =
                {
                    new SqlParameter("@Username", txtUsername.Text.Trim()),
                    new SqlParameter("@PasswordHash", Hash(txtPassword.Text)),
                    new SqlParameter("@Email", txtEmail.Text.Trim()),
                    new SqlParameter("@RoleId", roleId),
                    new SqlParameter("@SocietyId", ddlSociety.SelectedValue),
                    new SqlParameter("@InstituteId", ddlInstitute.SelectedValue)
                };

                AdminDBHelper.Execute(
                    @"INSERT INTO Users
                      (Username, PasswordHash, Email, RoleId, SocietyId, InstituteId, IsActive, IsFirstLogin)
                      VALUES
                      (@Username, @PasswordHash, @Email, @RoleId, @SocietyId, @InstituteId, 1, 1)",
                    pInsert);

                lblMsg.CssClass = "text-success small mb-3 d-block";
                lblMsg.Text = "Registration successful. Please login.";

                ClearForm();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Registration failed. Please try again.";
            }
        }

        private byte[] Hash(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirm.Text = "";
            ddlSociety.SelectedIndex = 0;
            ddlInstitute.Items.Clear();
            ddlInstitute.Items.Insert(0, new ListItem("-- Select Society First --", "0"));
        }
    }
}
