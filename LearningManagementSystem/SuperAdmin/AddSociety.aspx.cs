using System;
using System.Data;
using System.Data.SqlClient;

namespace LearningManagementSystem.SuperAdmin
{
    public partial class AddSociety : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SuperAdmin")
                Response.Redirect("~/Default.aspx");

            if (!IsPostBack)
                LoadSocieties();
        }

        void LoadSocieties()
        {
            gvSociety.DataSource =
                AdminDBHelper.GetTable("SELECT * FROM Societies");
            gvSociety.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtCode.Text))
                return;
            if (txtCode.Text.Length > 20)
            {
                // show message
                lblMsg .Text= "Society Code must be of lenght 10 or within 10";
                return;
            }

            string q = @"INSERT INTO Societies
                (SocietyName, SocietyCode, IsActive)
                VALUES (@n,@c,@a)";

            SqlParameter[] p =
            {
        new SqlParameter("@n", txtName.Text.Trim()),
        new SqlParameter("@c", txtCode.Text.Trim()),
        new SqlParameter("@a", ddlStatus.SelectedValue)
    };

            AdminDBHelper.Execute(q, p);
            LoadSocieties();
        }


        protected void gvSociety_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int societyId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "delete")
            {
                AdminDBHelper.Execute(
                    "DELETE FROM Societies WHERE SocietyId=@id",
                    new[] { new SqlParameter("@id", societyId) }
                );
                LoadSocieties();
            }

            if (e.CommandName == "view")
            {
                Session["SocietyId"] = societyId;
                Response.Redirect("~/Admin/Dashboard.aspx");
            }
        }
    }
}
