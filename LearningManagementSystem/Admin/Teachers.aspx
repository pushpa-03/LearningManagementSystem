<%@ Page Title="Teachers"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="Teachers.aspx.cs"
    Inherits="LearningManagementSystem.Admin.Teachers" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" />

<asp:Content ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <asp:HiddenField ID="hfUserId" runat="server" />
    <asp:Label ID="lblMsg" runat="server" CssClass="fw-bold d-block mb-2" />

    <!-- HEADER -->
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h3 class="mb-0">Teachers</h3>
        <a href="#" class="btn btn-success"
            data-bs-toggle="modal" data-bs-target="#CreateModal">
            <i class="fa-solid fa-plus"></i> Add Teacher
        </a>
    </div>

    <!-- GRID -->
    <div class="card shadow-sm border-0">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvTeachers" runat="server"
                    CssClass="table align-middle mb-0"
                    AutoGenerateColumns="false"
                    GridLines="None"
                    AllowPaging="true"
                    PageSize="10"
                    OnPageIndexChanging="gvTeachers_PageIndexChanging"
                    OnRowCommand="gvTeachers_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="FullName" HeaderText="Name" />
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="InstituteName" HeaderText="Institute" />
                        <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                        <asp:BoundField DataField="EmployeeId" HeaderText="Emp ID" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='badge <%# (bool)Eval("IsActive") ? "bg-success" : "bg-danger" %>'>
                                    <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-info me-1"
                                    CommandName="View"
                                    CommandArgument='<%# Eval("UserId") %>'>
                                    <i class="fa-solid fa-eye"></i>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-primary me-1"
                                    CommandName="Edit"
                                    CommandArgument='<%# Eval("UserId") %>'>
                                    <i class="fa-regular fa-pen-to-square"></i>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-warning me-1"
                                    CommandName="Toggle"
                                    CommandArgument='<%# Eval("UserId") %>'>
                                    <i class="fa-solid fa-toggle-on"></i>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-danger"
                                    CommandName="Delete"
                                    CommandArgument='<%# Eval("UserId") %>'
                                    OnClientClick="return confirm('Delete teacher?');">
                                    <i class="fa-solid fa-trash"></i>
                                </asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- ADD / EDIT MODAL -->
    <div class="modal fade" id="CreateModal" tabindex="-1">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">

                <div class="modal-header">
                    <h5>Teacher Registration</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">

                    <div class="row g-2">
                        <div class="col-md-6">
                            <label>Full Name *</label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label>DOB *</label>
                            <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label>Username *</label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label>Contact *</label>
                            <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-6">
                            <label>Society *</label>
                            <asp:DropDownList ID="ddlSociety" runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlSociety_SelectedIndexChanged" />
                        </div>

                        <div class="col-md-6">
                            <label>Institute *</label>
                            <asp:DropDownList ID="ddlInstitute" runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlInstitute_SelectedIndexChanged" />
                        </div>

                        <div class="col-md-6">
                            <label>Department (Optional)</label>
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select" />
                        </div>

                        <div class="col-md-6">
                            <label>Employee ID *</label>
                            <asp:TextBox ID="txtEmployeeId" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                </div>

                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save"
                        CssClass="btn btn-primary"
                        OnClick="btnSave_Click" />
                </div>

            </div>
        </div>
    </div>

    <script>
        setTimeout(function () {
            var lbl = document.getElementById('<%= lblMsg.ClientID %>');
            if (lbl) lbl.style.display = 'none';
        }, 5000);
    </script>

</asp:Content>
