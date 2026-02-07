<%@ Page Title="Societies"
    Language="C#"
    MasterPageFile="~/SuperAdmin/SuperAdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="AddSociety.aspx.cs"
    Inherits="LearningManagementSystem.SuperAdmin.AddSociety" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<div class="row mt-3">
    <div class="col-12">
        <div class="d-flex justify-content-between align-items-center">
            <h3 class="text-size-26">Societies</h3>

            <a href="#" class="btn bg-primary text-white"
               data-bs-toggle="modal"
               data-bs-target="#CreateModal">
                <i class="fa-solid fa-plus me-2"></i>Add Society
            </a>
        </div>
    </div>
</div>

<div class="card mt-4 shadow-sm border-0">
    <div class="card-body p-0">
        <div class="table-responsive">
            <asp:GridView ID="gvSociety"
                runat="server"
                AutoGenerateColumns="false"
                CssClass="table align-middle mb-0"
                OnRowCommand="gvSociety_RowCommand">

                <Columns>

                    <asp:BoundField DataField="SocietyName" HeaderText="Society Name" />
                    <asp:BoundField DataField="SocietyCode" HeaderText="Code" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# (bool)Eval("IsActive") ? "badge bg-success" : "badge bg-danger" %>'>
                                <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server"
                                CommandName="view"
                                CommandArgument='<%# Eval("SocietyId") %>'
                                CssClass="btn btn-sm btn-primary me-2">
                                <i class="fa-solid fa-eye"></i>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server"
                                CommandName="delete"
                                CommandArgument='<%# Eval("SocietyId") %>'
                                OnClientClick="return confirm('Delete this society?');"
                                CssClass="btn btn-sm btn-danger">
                                <i class="fa-solid fa-trash"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

<!-- CREATE MODAL -->
<div class="modal fade" id="CreateModal" tabindex="-1">
    <div class="modal-dialog modal-dialog-scrollable">
        <div class="modal-content rounded-0">
            <div class="modal-body p-4">

                <button type="button" class="btn position-absolute end-1"
                        data-bs-dismiss="modal">
                    <i class="fas fa-times"></i>
                </button>

                <h5 class="mb-3">Create Society</h5>
                <!-- Error message (NEW but non-breaking) -->
                <asp:Label ID="lblMsg" runat="server"
                    CssClass="text-danger small mb-3 d-block"></asp:Label>
                <asp:TextBox ID="txtName"
                    runat="server"
                    CssClass="form-control mb-3"
                    Placeholder="Society Name" />

                <asp:TextBox ID="txtCode"
                    runat="server"
                    CssClass="form-control mb-3"
                    Placeholder="Society Code" />

                <asp:DropDownList ID="ddlStatus"
                    runat="server"
                    CssClass="form-select mb-4">
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>

                <asp:Button ID="btnSave"
                    runat="server"
                    Text="Save"
                    CssClass="btn bg-primary text-white px-4"
                    OnClick="btnSave_Click" />
            </div>
        </div>
    </div>
</div>

</asp:Content>
