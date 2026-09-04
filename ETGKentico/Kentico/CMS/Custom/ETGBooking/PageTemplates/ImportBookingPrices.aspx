<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportBookingPrices.aspx.cs" Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"  Inherits="CMSApp.CMSModules.ETGBooking.PageTemplates.ImportBookingPrices" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="plcBeforeContent">
    <div id="m_pnlActions" class="cms-edit-menu">
        <div id="m_actionsElem_pnlMenu" class="object-edit-panel">
            <div id="m_actionsElem_editMenuElem_pC" class="header-actions-container">
                <div id="m_actionsElem_editMenuElem_menu_pnlUp">
                    <div id="m_actionsElem_editMenuElem_menu_pnlActions" class="header-actions-main">
                        <asp:Button runat="server" ID="btnImport" Text="Import" OnClick="btnImport_Click" CssClass="btn btn-primary"/>
                        <asp:Button runat="server" ID="btnValidate" Text="Validate CSV File" OnClick="btnValidate_Click" CssClass="btn btn-primary"/>
                    </div>
                </div>
                <div class="Clear">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    
    <div class="PageContent">
        <div class="editing-form-value-cell" style="padding-bottom:20px;display: block">
            <asp:FileUpload runat="server" ID="upload" />
        </div>
        <asp:Panel ID="pnlMessage" runat="server">
            <asp:Literal runat="server" ID="litMessage"></asp:Literal>
        </asp:Panel>
    </div>
</asp:Content>