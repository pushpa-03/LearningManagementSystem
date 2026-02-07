<%@ Page Title="Branches"
    Language="C#"
    MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="Branches.aspx.cs"
    Inherits="LearningManagementSystem.Admin.Branches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <asp:HiddenField ID="hfBranchId" runat="server" />

    <asp:Label ID="lblMsg" runat="server" CssClass="fw-bold d-block mb-2" />


    <!-- HEADER -->
    <div class="d-flex justify-content-between align-items-center mb-3">

        <h3 class="mb-0">Branches</h3>
         <div class="d-flex gap-2">
            <button class="btn btn-light border">
                <i class="fa-solid fa-filter"></i> Filter by
            </button>
            <a href="#" data-bs-toggle="modal" data-bs-target="#CreateModal"
                class="btn btn-success">
                <i class="fa-solid fa-plus"></i> Add Branch
            </a>
         </div>
    </div>

    <!-- GRID -->
    <div class="card shadow-sm border-0 mt-4">
        <div class="card-body p-0">
            <div class="table-responsive">
            <asp:GridView ID="gvBranches" runat="server"
                CssClass="table align-middle mb-0"
                AutoGenerateColumns="false"
                GridLines="None"
                OnRowCommand="gvBranches_RowCommand">
                <Columns>
                    <asp:BoundField DataField="SocietyName" HeaderText="Society" />
                    <asp:BoundField DataField="InstituteName" HeaderText="Institute" />
                    <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                    <asp:BoundField DataField="BranchName" HeaderText="Branch" />
                    <asp:BoundField DataField="BranchCode" HeaderText="Code" />

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
                                CommandArgument='<%# Eval("BranchId") %>'>
                                <i class="fa-regular fa-pen-to-square"></i>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-warning me-2"
                                    CommandName="Toggle"
                                    CommandArgument='<%# Eval("BranchId") %>'
                                    OnClientClick="return confirm('Change status?');">
                                    <i class="fa-solid fa-toggle-on"></i>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-danger"
                                    CommandName="DeleteRow"
                                    CommandArgument='<%# Eval("BranchId") %>'
                                    OnClientClick="return confirm('Delete branch?');">
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
                    <h5>Add Branch</h5>
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
                        CssClass="form-select mb-2"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlInstitute_SelectedIndexChanged" />

                    <label>Department</label>
                    <asp:DropDownList ID="ddlDepartment" runat="server"
                        CssClass="form-select mb-2" />

                    <label>Branch Name</label>
                    <asp:TextBox ID="txtBranchName" runat="server"
                        CssClass="form-control mb-2"
                        placeholder="Branch Name" />

                    <label>Branch Code</label>
                    <asp:TextBox ID="txtBranchCode" runat="server"
                        CssClass="form-control"
                         MaxLength="6"
                        placeholder="Branch Code" />
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
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5>Edit Branch</h5>
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
                        CssClass="form-select mb-2"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlInstituteEdit_SelectedIndexChanged" />

                     <label>Department</label>
                    <asp:DropDownList ID="ddlDepartmentEdit" runat="server"
                        CssClass="form-select mb-2" />

                    <label>Branch Name</label>
                    <asp:TextBox ID="txtBranchNameEdit" runat="server"
                        CssClass="form-control mb-2" />

                    <label>Branch Code</label>
                    <asp:TextBox ID="txtBranchCodeEdit" runat="server"
                        CssClass="form-control"
                        MaxLength ="6"/>
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

    <script>
        setTimeout(function () {
            var lbl = document.getElementById('<%= lblMsg.ClientID %>');
            if (lbl) lbl.style.display = 'none';
        }, 5000);
    </script>

</asp:Content>
