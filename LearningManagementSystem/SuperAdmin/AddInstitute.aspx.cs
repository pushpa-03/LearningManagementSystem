using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace LearningManagementSystem.SuperAdmin
{
    public partial class AddInstitute : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSocieties();
                LoadInstitutes();
            }
        }

        void LoadSocieties()
        {
            DataTable dt = AdminDBHelper.GetTable("SELECT SocietyId, SocietyName FROM Societies WHERE IsActive=1");
            
            ddlSociety.DataSource = dt;
            ddlSociety.DataTextField = "SocietyName";
            ddlSociety.DataValueField = "SocietyId";
            ddlSociety.DataBind();
            ddlSociety.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Society --", ""));

            ddlSocietyEdit.DataSource = dt;
            ddlSocietyEdit.DataTextField = "SocietyName";
            ddlSocietyEdit.DataValueField = "SocietyId";
            ddlSocietyEdit.DataBind();
            ddlSocietyEdit.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Society --", ""));
        }

        void LoadInstitutes()
        {
            gvInstitutes.DataSource = AdminDBHelper.GetTable(
                @"SELECT I.InstituteId, S.SocietyName, I.InstituteName, I.InstituteCode, I.IsActive 
                  FROM Institutes I 
                  JOIN Societies S ON S.SocietyId = I.SocietyId");
            gvInstitutes.DataBind();
        }

        private void ClearFields()
        {
            txtInstituteName.Text = txtInstituteCode.Text = "";
            txtInstituteNameEdit.Text = txtInstituteCodeEdit.Text = "";
            ddlSociety.SelectedIndex = 0;
            hfInstituteId.Value = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlSociety.SelectedValue == "" || string.IsNullOrWhiteSpace(txtInstituteName.Text))
            {
                lblMsg.Text = "Please fill all fields.";
                return;
            }

            AdminDBHelper.Execute(
                "INSERT INTO Institutes (SocietyId, InstituteName, InstituteCode, IsActive) VALUES (@Sid, @Name, @Code, 1)",
                new SqlParameter[] {
                    new SqlParameter("@Sid", ddlSociety.SelectedValue),
                    new SqlParameter("@Name", txtInstituteName.Text.Trim()),
                    new SqlParameter("@Code", txtInstituteCode.Text.Trim())
                });

            ClearFields();
            LoadInstitutes();
            lblMsg.Text = "Institute added successfully!";
        }

        protected void gvInstitutes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null) return;
            int instituteId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                DataTable dt = AdminDBHelper.GetTable("SELECT * FROM Institutes WHERE InstituteId=@Id", 
                    new SqlParameter[] { new SqlParameter("@Id", instituteId) });

                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    hfInstituteId.Value = r["InstituteId"].ToString();
                    ddlSocietyEdit.SelectedValue = r["SocietyId"].ToString();
                    txtInstituteNameEdit.Text = r["InstituteName"].ToString();
                    txtInstituteCodeEdit.Text = r["InstituteCode"].ToString();

                    // Show Modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowEdit", "$('#EditModal').modal('show');", true);
                }
            }
            else if (e.CommandName == "Toggle")
            {
                AdminDBHelper.Execute("UPDATE Institutes SET IsActive = 1 - IsActive WHERE InstituteId=@Id", 
                    new SqlParameter[] { new SqlParameter("@Id", instituteId) });
                LoadInstitutes();
            }
            else if (e.CommandName == "DeleteRow")
            {
                AdminDBHelper.Execute("DELETE FROM Institutes WHERE InstituteId=@Id", 
                    new SqlParameter[] { new SqlParameter("@Id", instituteId) });
                LoadInstitutes();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfInstituteId.Value)) return;

            AdminDBHelper.Execute(
                @"UPDATE Institutes SET SocietyId=@Sid, InstituteName=@Name, InstituteCode=@Code WHERE InstituteId=@Id",
                new SqlParameter[] {
                    new SqlParameter("@Sid", ddlSocietyEdit.SelectedValue),
                    new SqlParameter("@Name", txtInstituteNameEdit.Text.Trim()),
                    new SqlParameter("@Code", txtInstituteCodeEdit.Text.Trim()),
                    new SqlParameter("@Id", hfInstituteId.Value)
                });

            LoadInstitutes();
            ClearFields();
            
            // Hide Modal after update
            ScriptManager.RegisterStartupScript(this, GetType(), "HideEdit", "$('#EditModal').modal('hide');", true);
        }
    }
}