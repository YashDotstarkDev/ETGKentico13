<%@ Control Language="C#" Inherits="CMS.FormEngine.Web.UI.CMSAbstractFormLayout" %> 
<%@ Register TagPrefix="cms" Namespace="CMS.FormEngine.Web.UI" Assembly="CMS.FormEngine.Web.UI" %>
<%@ Register Src="~/CMSAdminControls/UI/SmartTip.ascx" TagPrefix="cms" TagName="SmartTip" %>

<div class="content-block-50 remove-default-space">
  <cms:FormControl runat="server" ID="iTemplateCode" Field="TemplateCode" FormControlName="MacroEditor" />
</div>

<cms:SmartTip runat="server" ID="stHint" EnableViewState="false" Content="{$newsletter.templatecode.smarttip.content$}" 
  ExpandedHeader="{$newsletter.templatecode.smarttip.header$}" CollapsedHeader="{$newsletter.templatecode.smarttip.header$}" /> 
