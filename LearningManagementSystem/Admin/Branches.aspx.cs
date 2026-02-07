using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LearningManagementSystem.Admin
{
    public partial class Branches : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSocieties();
                LoadBranches();
            }
        }

        void LoadSocieties()
        {
            DataTable dt = AdminDBHelper.GetTable(
                "SELECT SocietyId, SocietyName FROM Societies");

            ddlSociety.DataSource = ddlSocietyEdit.DataSource = dt;
            ddlSociety.DataTextField = ddlSocietyEdit.DataTextField = "SocietyName";
            ddlSociety.DataValueField = ddlSocietyEdit.DataValueField = "SocietyId";

            ddlSociety.DataBind();
            ddlSocietyEdit.DataBind();

            ddlSociety.Items.Insert(0, new ListItem("-- Select Society --", ""));
            ddlSocietyEdit.Items.Insert(0, new ListItem("-- Select Society --", ""));
        }

        void LoadInstitutes(DropDownList ddl, string societyId)
        {
            ddl.DataSource = AdminDBHelper.GetTable(
                "SELECT InstituteId, InstituteName FROM Institutes WHERE SocietyId=@S",
                new SqlParameter[] { new SqlParameter("@S", societyId) });

            ddl.DataTextField = "InstituteName";
            ddl.DataValueField = "InstituteId";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Select Institute --", ""));
        }

        void LoadDepartments(DropDownList ddl, string instituteId)
        {
            ddl.DataSource = AdminDBHelper.GetTable(
                "SELECT DepartmentId, DepartmentName FROM Departments WHERE InstituteId=@I",
                new SqlParameter[] { new SqlParameter("@I", instituteId) });

            ddl.DataTextField = "DepartmentName";
            ddl.DataValueField = "DepartmentId";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Select Department --", ""));
        }


        void LoadBranches()
        {
            gvBranches.DataSource = AdminDBHelper.GetTable(@"
                SELECT B.BranchId, S.SocietyName, I.InstituteName,
                       D.DepartmentName, B.BranchName, B.BranchCode, B.IsActive
                FROM Branches B
                JOIN Societies S ON S.SocietyId=B.SocietyId
                JOIN Institutes I ON I.InstituteId=B.InstituteId
                JOIN Departments D ON D.DepartmentId=B.DepartmentId");
            gvBranches.DataBind();
        }

        // ---------- Create Modal Dropdowns ----------
        protected void ddlSociety_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSociety.SelectedValue != "")
                LoadInstitutes(ddlInstitute, ddlSociety.SelectedValue);
        }

        protected void ddlInstitute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInstitute.SelectedValue != "")
                LoadDepartments(ddlDepartment, ddlInstitute.SelectedValue);
        }

        // ---------- Edit Modal Dropdowns (FIXED) ----------
        protected void ddlSocietyEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSocietyEdit.SelectedValue != "")
                LoadInstitutes(ddlInstituteEdit, ddlSocietyEdit.SelectedValue);
        }

        protected void ddlInstituteEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInstituteEdit.SelectedValue != "")
                LoadDepartments(ddlDepartmentEdit, ddlInstituteEdit.SelectedValue);
        }

        // ---------- Save ----------
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlSociety.SelectedValue == "" ||
                ddlInstitute.SelectedValue == "" ||
                ddlDepartment.SelectedValue == "" ||
                txtBranchName.Text.Trim() == "")
            {
                ShowMsg("All fields are required.", false);
                return;
            }

            int exists = AdminDBHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM Branches
                  WHERE SocietyId=@Sid AND InstituteId=@Iid
                  AND DepartmentId=@Did AND BranchName=@Name",
                new SqlParameter[] {
                    new SqlParameter("@Sid", ddlSociety.SelectedValue),
                    new SqlParameter("@Iid", ddlInstitute.SelectedValue),
                    new SqlParameter("@Did", ddlDepartment.SelectedValue),
                    new SqlParameter("@Name", txtBranchName.Text.Trim())
                });

            if (exists > 0)
            {
                ShowMsg("Branch already exists.", false);
                return;
            }

            AdminDBHelper.Execute(
                @"INSERT INTO Branches
                  (SocietyId, InstituteId, DepartmentId, BranchName, BranchCode, IsActive)
                  VALUES (@S,@I,@D,@N,@C,1)",
                new SqlParameter[] {
                    new SqlParameter("@S", ddlSociety.SelectedValue),
                    new SqlParameter("@I", ddlInstitute.SelectedValue),
                    new SqlParameter("@D", ddlDepartment.SelectedValue),
                    new SqlParameter("@N", txtBranchName.Text),
                    new SqlParameter("@C", txtBranchCode.Text)
                });

            LoadBranches();
            ShowMsg("Branch added successfully.", true);
        }

        // ---------- GridView Row Command ----------
        protected void gvBranches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                hfBranchId.Value = id.ToString();

                DataTable dt = AdminDBHelper.GetTable(
                 "SELECT * FROM Branches WHERE BranchId=@Id",
                 new SqlParameter[] { new SqlParameter("@Id", id) });

                ddlSocietyEdit.SelectedValue = dt.Rows[0]["SocietyId"].ToString();
                LoadInstitutes(ddlInstituteEdit, ddlSocietyEdit.SelectedValue);

                ddlInstituteEdit.SelectedValue = dt.Rows[0]["InstituteId"].ToString();
                LoadDepartments(ddlDepartmentEdit, ddlInstituteEdit.SelectedValue);

                ddlDepartmentEdit.SelectedValue = dt.Rows[0]["DepartmentId"].ToString();
                txtBranchNameEdit.Text = dt.Rows[0]["BranchName"].ToString();
                txtBranchCodeEdit.Text = dt.Rows[0]["BranchCode"].ToString();

                ScriptManager.RegisterStartupScript(this, GetType(),
                    "edit", "var m=new bootstrap.Modal(document.getElementById('EditModal'));m.show();", true);
            }
            else if (e.CommandName == "Toggle")
            {
                AdminDBHelper.Execute(
                "UPDATE Branches SET IsActive=1-IsActive WHERE BranchId=@Id",
                new SqlParameter[] { new SqlParameter("@Id", id) });

                LoadBranches();
            }
            else if (e.CommandName == "DeleteRow")
            {
                AdminDBHelper.Execute(
                "DELETE FROM Branches WHERE BranchId=@Id",
                new SqlParameter[] { new SqlParameter("@Id", id) });
                LoadBranches();
            }
        }

        // ---------- Update ----------
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ddlSocietyEdit.SelectedValue == "" ||
                ddlInstituteEdit.SelectedValue == "" ||
                ddlDepartmentEdit.SelectedValue == "" ||
                txtBranchNameEdit.Text.Trim() == "")
            {
                ShowMsg("All fields are required.", false);
                return;
            }

            AdminDBHelper.Execute(
                @"UPDATE Branches SET SocietyId=@Sid, InstituteId=@Iid,
                  DepartmentId=@Did, BranchName=@N, BranchCode=@C
                  WHERE BranchId=@Id",
                new SqlParameter[] {
                    new SqlParameter("@Sid", ddlSocietyEdit.SelectedValue),
                    new SqlParameter("@Iid", ddlInstituteEdit.SelectedValue),
                    new SqlParameter("@Did", ddlDepartmentEdit.SelectedValue),
                    new SqlParameter("@N", txtBranchNameEdit.Text),
                    new SqlParameter("@C", txtBranchCodeEdit.Text),
                    new SqlParameter("@Id", hfBranchId.Value)
                });

            LoadBranches();
            ShowMsg("Branch updated successfully.", true);
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
