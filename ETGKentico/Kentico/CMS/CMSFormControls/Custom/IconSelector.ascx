<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IconSelector.ascx.cs" Inherits="CMSApp.CMSFormControls.Custom.IconSelector" %>
<cms:CMSUpdatePanel RenderMode="Block" ID="pnlIconSelector" runat="server">
    <ContentTemplate>
        <div class="control-group-inline">
            <cms:CMSTextBox runat="server" ID="txtIconClass" ReadOnly="true" />
            <cms:LocalizedButton runat="server" ID="btnChangeIcon" ResourceString="general.select" ButtonStyle="Default" EnableViewState="false" />
            <cms:LocalizedButton runat="server" ID="btnClearIcon" ResourceString="general.clear" ButtonStyle="Default"
                OnClick="btnClearIcon_Click" EnableViewState="false" />
            <asp:HiddenField runat="server" ID="hfValue" />
        </div>
    </ContentTemplate>
</cms:CMSUpdatePanel>