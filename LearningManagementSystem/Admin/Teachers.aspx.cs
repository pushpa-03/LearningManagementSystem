using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LearningManagementSystem.Admin
{
    public partial class Teachers : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSocieties();
                LoadTeachers();
            }
        }

        void LoadSocieties()
        {
            ddlSociety.DataSource = AdminDBHelper.GetTable(
                "SELECT SocietyId, SocietyName FROM Societies WHERE IsActive=1");

            ddlSociety.DataTextField = "SocietyName";
            ddlSociety.DataValueField = "SocietyId";
            ddlSociety.DataBind();
            ddlSociety.Items.Insert(0, new ListItem("-- Select --", ""));
        }

        void LoadInstitutes(string societyId)
        {
            ddlInstitute.DataSource = AdminDBHelper.GetTable(
                "SELECT InstituteId, InstituteName FROM Institutes WHERE SocietyId=@S",
                new SqlParameter[] { new SqlParameter("@S", societyId) });

            ddlInstitute.DataTextField = "InstituteName";
            ddlInstitute.DataValueField = "InstituteId";
            ddlInstitute.DataBind();
            ddlInstitute.Items.Insert(0, new ListItem("-- Select --", ""));
        }

        void LoadDepartments(string instituteId)
        {
            ddlDepartment.DataSource = AdminDBHelper.GetTable(
                "SELECT DepartmentId, DepartmentName FROM Departments WHERE InstituteId=@I",
                new SqlParameter[] { new SqlParameter("@I", instituteId) });

            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("-- Optional --", ""));
        }

        void LoadTeachers()
        {
            gvTeachers.DataSource = AdminDBHelper.GetTable(@"
                SELECT U.UserId, U.Username, U.IsActive,
                       UP.FullName,
                       I.InstituteName,
                       ISNULL(D.DepartmentName,'-') DepartmentName,
                       T.EmployeeId
                FROM Users U
                JOIN UserProfile UP ON UP.UserId=U.UserId
                JOIN TeacherDetails T ON T.UserId=U.UserId
                JOIN Institutes I ON I.InstituteId=U.InstituteId
                LEFT JOIN Departments D ON D.DepartmentId=T.DepartmentId
                WHERE U.RoleId = 2");

            gvTeachers.DataBind();
        }

        protected void ddlSociety_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSociety.SelectedValue != "")
                LoadInstitutes(ddlSociety.SelectedValue);
        }

        protected void ddlInstitute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInstitute.SelectedValue != "")
                LoadDepartments(ddlInstitute.SelectedValue);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFullName.Text == "" || txtUsername.Text == "" ||
                txtDOB.Text == "" || ddlInstitute.SelectedValue == "")
            {
                ShowMsg("Required fields missing", false);
                return;
            }

            int exists = AdminDBHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Users WHERE Username=@U",
                new SqlParameter[] { new SqlParameter("@U", txtUsername.Text) });

            if (exists > 0)
            {
                ShowMsg("Username already exists", false);
                return;
            }

            AdminDBHelper.Execute(@"
                INSERT INTO Users (Username, PasswordHash, RoleId, SocietyId, InstituteId)
                VALUES (@U, HASHBYTES('SHA2_256','12345'), 2, @S, @I)",

                new SqlParameter[] {
                    new SqlParameter("@U", txtUsername.Text),
                    new SqlParameter("@S", ddlSociety.SelectedValue),
                    new SqlParameter("@I", ddlInstitute.SelectedValue)
                });

            int userId = AdminDBHelper.ExecuteScalar(
                "SELECT UserId FROM Users WHERE Username=@U",
                new SqlParameter[] { new SqlParameter("@U", txtUsername.Text) });

            AdminDBHelper.Execute(@"
                INSERT INTO UserProfile
                (UserId, FullName, Gender, DOB, ContactNo, EmergencyContactName,
                 EmergencyContactNo, Address, JoinedDate)
                VALUES (@Uid,@N,'NA',@DOB,@C,'NA','NA','NA',GETDATE())",

                new SqlParameter[] {
                    new SqlParameter("@Uid", userId),
                    new SqlParameter("@N", txtFullName.Text),
                    new SqlParameter("@DOB", txtDOB.Text),
                    new SqlParameter("@C", txtContact.Text)
                });

            AdminDBHelper.Execute(@"
                INSERT INTO TeacherDetails
                (UserId, EmployeeId, DepartmentId, ExperienceYears)
                VALUES (@Uid,@E,@D,0)",

                new SqlParameter[] {
                    new SqlParameter("@Uid", userId),
                    new SqlParameter("@E", txtEmployeeId.Text),
                    new SqlParameter("@D",
                        ddlDepartment.SelectedValue == ""
                        ? (object)DBNull.Value
                        : ddlDepartment.SelectedValue)
                });

            LoadTeachers();
            ShowMsg("Teacher added successfully", true);
        }

        protected void gvTeachers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Toggle")
            {
                AdminDBHelper.Execute(
                    "UPDATE Users SET IsActive=1-IsActive WHERE UserId=@Id",
                    new SqlParameter[] { new SqlParameter("@Id", id) });
                LoadTeachers();
            }
            else if (e.CommandName == "Delete")
            {
                AdminDBHelper.Execute(
                    "DELETE FROM TeacherDetails WHERE UserId=@Id",
                    new SqlParameter[] { new SqlParameter("@Id", id) });
                AdminDBHelper.Execute(
                    "DELETE FROM UserProfile WHERE UserId=@Id",
                    new SqlParameter[] { new SqlParameter("@Id", id) });
                AdminDBHelper.Execute(
                    "DELETE FROM Users WHERE UserId=@Id",
                    new SqlParameter[] { new SqlParameter("@Id", id) });

                LoadTeachers();
            }
        }

        protected void gvTeachers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTeachers.PageIndex = e.NewPageIndex;
            LoadTeachers();
        }

        void ShowMsg(string msg, bool success)
        {
            lblMsg.Text = msg;
            lblMsg.CssClass = success
                ? "text-success fw-bold"
                : "text-danger fw-bold";
        }
    }
}
