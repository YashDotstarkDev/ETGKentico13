<%@ Control Language="C#" Inherits="CMS.FormEngine.Web.UI.CMSAbstractFormLayout" %> 
<%@ Register TagPrefix="cms" Namespace="CMS.FormEngine.Web.UI" Assembly="CMS.FormEngine.Web.UI" %>
<cms:FormCategory ID="pnlGeneral2" runat="server" CategoryTitleResourceString="general.general">
  <cms:FormField runat="server" ID="fScopePath" Field="ScopePath" ShowRequiredMark="true" />
  <cms:FormField runat="server" ID="fScopeIncludeChildren" Field="ScopeIncludeChildren" />
</cms:FormCategory>
<cms:FormCategory ID="pnlGeneral" runat="server" CategoryTitleResourceString="development.doctypes">
  <cms:FormField runat="server" ID="fScopeAllowAllTypes" Field="ScopeAllowAllTypes" />
  <cms:FormField runat="server" ID="fScopeClasses" Field="ScopeClasses" UseFFI="false">
    <cms:FormControl runat="server" ID="iScopeClasses" Field="ScopeClasses" />
  </cms:FormField>
</cms:FormCategory>
<cms:FormCategory ID="pnlClasses" runat="server" CategoryTitleResourceString="general.advanced">
  <cms:FormField runat="server" ID="fScopeAllowLinks" Field="ScopeAllowLinks" />  
  <cms:FormField runat="server" ID="fScopeMacroCondition" Field="ScopeMacroCondition" />
</cms:FormCategory>
<cms:FormSubmit runat="server" ID="fSubmit" />