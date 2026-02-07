<%@ Page Title="Departments"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="Department.aspx.cs"
    Inherits="LearningManagementSystem.Admin.Department" %>
<asp:Content ID="c1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

 <asp:HiddenField ID="hfDepartmentId" runat="server" />
    <asp:Label ID="lblMsg" runat="server"
        CssClass="fw-bold d-block mb-2" />

    <div class="d-flex justify-content-between align-items-center mb-3">
    <h3 class="mb-0">Department</h3>

    <div class="d-flex gap-2">
        <button class="btn btn-light border">
            <i class="fa-solid fa-filter"></i> Filter by
        </button>

        <a href="#" data-bs-toggle="modal" data-bs-target="#CreateModal"
           class="btn btn-success">
            <i class="fa-solid fa-plus"></i> Add Departments
        </a>
    </div>
</div>


    <div class="card shadow-sm border-0 mt-4">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvDepartments" runat="server"
                    CssClass="table align-middle mb-0"
                    AutoGenerateColumns="false"
                    GridLines="None"
                    OnRowCommand="gvDepartments_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="SocietyName" HeaderText="Society" />
                        <asp:BoundField DataField="InstituteName" HeaderText="Institute" />
                        <asp:BoundField DataField="DepartmentName" HeaderText="Department" />

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
                                    CssClass="btn btn-sm btn-primary me-2"
                                    CommandName="EditRow"
                                    CommandArgument='<%# Eval("DepartmentId") %>'>
                                    <i class="fa-regular fa-pen-to-square"></i>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-warning me-2"
                                    CommandName="Toggle"
                                    CommandArgument='<%# Eval("DepartmentId") %>'
                                    OnClientClick="return confirm('Change status?');">
                                    <i class="fa-solid fa-toggle-on"></i>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-danger"
                                    CommandName="DeleteRow"
                                    CommandArgument='<%# Eval("DepartmentId") %>'
                                    OnClientClick="return confirm('Delete department?');">
                                    <i class="fa-solid fa-trash-can"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- ADD MODAL -->
    <div class="modal fade" id="CreateModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5>Add Department</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <label>Society</label>
                    <asp:DropDownList ID="ddlSociety" runat="server"
                        CssClass="form-select mb-2"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSociety_SelectedIndexChanged" />

                    <label>Institute</label>
                    <asp:DropDownList ID="ddlInstitute" runat="server"
                        CssClass="form-select mb-2" />

                    <label>Department Name</label>
                    <asp:TextBox ID="txtDepartmentName" runat="server"
                        CssClass="form-control"
                        placeholder="Department Name" />
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

    <!-- EDIT MODAL -->
    <div class="modal fade" id="EditModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5>Edit Department</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <label>Society</label>
                    <asp:DropDownList ID="ddlSocietyEdit" runat="server"
                        CssClass="form-select mb-2"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSocietyEdit_SelectedIndexChanged" />

                    <label>Institute</label>
                    <asp:DropDownList ID="ddlInstituteEdit" runat="server"
                        CssClass="form-select mb-2" />

                    <label>Department Name</label>
                    <asp:TextBox ID="txtDepartmentNameEdit" runat="server"
                        CssClass="form-control" />
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnUpdate" runat="server"
                        Text="Update"
                        CssClass="btn btn-primary"
                        OnClick="btnUpdate_Click" />
                </div>
            </div>
        </div>
    </div>
    <div class="d-flex justify-content-between align-items-center p-3">
    <div>
        <ul class="pagination mb-0">
            <li class="page-item active"><a class="page-link" href="#">1</a></li>
            <li class="page-item"><a class="page-link" href="#">2</a></li>
            <li class="page-item"><a class="page-link" href="#">›</a></li>
        </ul>
    </div>
    <div class="text-muted">
        PAGE 1 OF 102
    </div>
</div>

    <script>
        setTimeout(function () {
            var lbl = document.getElementById('<%= lblMsg.ClientID %>');
            if (lbl) lbl.style.display = 'none';
        }, 5000);
    </script>

</asp:Content>
