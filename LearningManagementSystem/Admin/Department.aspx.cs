using LearningManagementSystem.SuperAdmin;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LearningManagementSystem.Admin
{
    public partial class Department : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSocieties();
                LoadDepartments();
            }
        }

        void LoadSocieties()
        {
            DataTable dt = AdminDBHelper.GetTable(
                "SELECT SocietyId, SocietyName FROM Societies WHERE IsActive=1");

            ddlSociety.DataSource = ddlSocietyEdit.DataSource = dt;
            ddlSociety.DataTextField = ddlSocietyEdit.DataTextField = "SocietyName";
            ddlSociety.DataValueField = ddlSocietyEdit.DataValueField = "SocietyId";
            ddlSociety.DataBind();
            ddlSocietyEdit.DataBind();

            ddlSociety.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Society --", ""));
            ddlSocietyEdit.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Society --", ""));
        }

        void LoadInstitutes(DropDownList ddl, string societyId)
        {
            ddl.Items.Clear();
            ddl.DataSource = AdminDBHelper.GetTable(
                "SELECT InstituteId, InstituteName FROM Institutes WHERE IsActive=1 AND SocietyId=@Sid",
                new SqlParameter[] { new SqlParameter("@Sid", societyId) });

            ddl.DataTextField = "InstituteName";
            ddl.DataValueField = "InstituteId";
            ddl.DataBind();
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Institute --", ""));
        }

        void LoadDepartments()
        {
            gvDepartments.DataSource = AdminDBHelper.GetTable(
                @"SELECT D.DepartmentId, S.SocietyName, I.InstituteName,
                         D.DepartmentName, D.IsActive
                  FROM Departments D
                  JOIN Societies S ON S.SocietyId=D.SocietyId
                  JOIN Institutes I ON I.InstituteId=D.InstituteId");
            gvDepartments.DataBind();
        }

        protected void ddlSociety_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSociety.SelectedValue != "")
                LoadInstitutes(ddlInstitute, ddlSociety.SelectedValue);
        }

        protected void ddlSocietyEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSocietyEdit.SelectedValue != "")
                LoadInstitutes(ddlInstituteEdit, ddlSocietyEdit.SelectedValue);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlSociety.SelectedValue == "" ||
                ddlInstitute.SelectedValue == "" ||
                txtDepartmentName.Text.Trim() == "")
            {
                ShowMsg("All fields are required.", false);
                return;
            }

            int exists = AdminDBHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM Departments
                  WHERE SocietyId=@Sid AND InstituteId=@Iid AND DepartmentName=@Name",
                new SqlParameter[] {
                    new SqlParameter("@Sid", ddlSociety.SelectedValue),
                    new SqlParameter("@Iid", ddlInstitute.SelectedValue),
                    new SqlParameter("@Name", txtDepartmentName.Text.Trim())
                });

            if (exists > 0)
            {
                ShowMsg("Department already exists.", false);
                return;
            }

            AdminDBHelper.Execute(
                @"INSERT INTO Departments (SocietyId, InstituteId, DepartmentName)
                  VALUES (@Sid,@Iid,@Name)",
                new SqlParameter[] {
                    new SqlParameter("@Sid", ddlSociety.SelectedValue),
                    new SqlParameter("@Iid", ddlInstitute.SelectedValue),
                    new SqlParameter("@Name", txtDepartmentName.Text.Trim())
                });

            LoadDepartments();
            ShowMsg("Department added successfully.", true);
        }

        protected void gvDepartments_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                DataTable dt = AdminDBHelper.GetTable(
                    "SELECT * FROM Departments WHERE DepartmentId=@Id",
                    new SqlParameter[] { new SqlParameter("@Id", id) });

                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    hfDepartmentId.Value = id.ToString();
                    ddlSocietyEdit.SelectedValue = r["SocietyId"].ToString();
                    LoadInstitutes(ddlInstituteEdit, r["SocietyId"].ToString());
                    ddlInstituteEdit.SelectedValue = r["InstituteId"].ToString();
                    txtDepartmentNameEdit.Text = r["DepartmentName"].ToString();

                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "Edit",
                        "$('#EditModal').modal('show');", true);
                }
            }
            else if (e.CommandName == "Toggle")
            {
                AdminDBHelper.Execute(
                    "UPDATE Departments SET IsActive=1-IsActive WHERE DepartmentId=@Id",
                    new SqlParameter[] { new SqlParameter("@Id", id) });
                LoadDepartments();
            }
            else if (e.CommandName == "DeleteRow")
            {
                AdminDBHelper.Execute(
                    "DELETE FROM Departments WHERE DepartmentId=@Id",
                    new SqlParameter[] { new SqlParameter("@Id", id) });
                LoadDepartments();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (hfDepartmentId.Value == "") return;

            AdminDBHelper.Execute(
                @"UPDATE Departments
                  SET SocietyId=@Sid, InstituteId=@Iid, DepartmentName=@Name
                  WHERE DepartmentId=@Id",
                new SqlParameter[] {
                    new SqlParameter("@Sid", ddlSocietyEdit.SelectedValue),
                    new SqlParameter("@Iid", ddlInstituteEdit.SelectedValue),
                    new SqlParameter("@Name", txtDepartmentNameEdit.Text.Trim()),
                    new SqlParameter("@Id", hfDepartmentId.Value)
                });

            LoadDepartments();
            ShowMsg("Department updated successfully.", true);

            ScriptManager.RegisterStartupScript(
                this, GetType(), "Hide",
                "$('#EditModal').modal('hide');", true);
        }

        void ShowMsg(string msg, bool success)
        {
            lblMsg.Text = msg;
            lblMsg.CssClass = success
                ? "text-success fw-bold d-block mb-2"
                : "text-danger fw-bold d-block mb-2";
        }
    }
}
