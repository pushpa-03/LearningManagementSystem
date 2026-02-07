<%@ Page Title="Institutes"
    Language="C#"
    MasterPageFile="~/SuperAdmin/SuperAdminMaster.master"
    AutoEventWireup="true"
    CodeBehind="AddInstitute.aspx.cs"
    Inherits="LearningManagementSystem.SuperAdmin.AddInstitute" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hfInstituteId" runat="server" />
    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger fw-bold d-block mb-2" />

    <div class="d-flex align-items-lg-center flex-column flex-md-row mt-3">
        <div class="flex-grow-1">
            <h3 class="mb-2 text-size-26 text-color-2">Institutes</h3>
        </div>
        <div>
            <a href="#" data-bs-toggle="modal" data-bs-target="#CreateModal"
               class="bg-primary text-white d-flex align-items-center px-3 py-2 rounded-2 fw-bolder">
                <i class="fa-solid fa-plus me-2"></i> Add Institute
            </a>
        </div>
    </div>

    <div class="card shadow-sm border-0 mt-4">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvInstitutes" runat="server"
                    CssClass="table align-middle mb-0"
                    AutoGenerateColumns="false"
                    GridLines="None"
                    OnRowCommand="gvInstitutes_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="SocietyName" HeaderText="Society" />
                        <asp:BoundField DataField="InstituteName" HeaderText="Institute" />
                        <asp:BoundField DataField="InstituteCode" HeaderText="Code" />

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
                                    CommandArgument='<%# Eval("InstituteId") %>'>
                                    <i class="fa-regular fa-pen-to-square"></i>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-warning me-2"
                                    CommandName="Toggle"
                                    CommandArgument='<%# Eval("InstituteId") %>'
                                    OnClientClick="return confirm('Change status?');">
                                    <i class="fa-solid fa-toggle-on"></i>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server"
                                    CssClass="btn btn-sm btn-danger"
                                    CommandName="DeleteRow"
                                    CommandArgument='<%# Eval("InstituteId") %>'
                                    OnClientClick="return confirm('Delete institute?');">
                                    <i class="fa-solid fa-trash-can"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div class="modal fade" id="CreateModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add Institute</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <label>Select Society</label>
                    <asp:DropDownList ID="ddlSociety" runat="server" CssClass="form-select mb-2" />
                    <label>Institute Name</label>
                    <asp:TextBox ID="txtInstituteName" runat="server" CssClass="form-control mb-2" placeholder="Institute Name" />
                    <label>Institute Code</label>
                    <asp:TextBox ID="txtInstituteCode" runat="server" CssClass="form-control" placeholder="Institute Code" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="EditModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Edit Institute</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <label>Select Society</label>
                    <asp:DropDownList ID="ddlSocietyEdit" runat="server" CssClass="form-select mb-2" />
                    <label>Institute Name</label>
                    <asp:TextBox ID="txtInstituteNameEdit" runat="server" CssClass="form-control mb-2" />
                    <label>Institute Code</label>
                    <asp:TextBox ID="txtInstituteCodeEdit" runat="server" CssClass="form-control" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>