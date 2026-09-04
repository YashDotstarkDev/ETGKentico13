<%@ Control Language="C#" Inherits="CMS.FormEngine.Web.UI.CMSAbstractFormLayout" %> 
<%@ Register TagPrefix="cms" Namespace="CMS.FormEngine.Web.UI" Assembly="CMS.FormEngine.Web.UI" %>
<cms:LocalizedHeading ID="fTags" runat="server" ResourceString="content.metadata.tags" Level="4"></cms:LocalizedHeading>
<cms:LocalizedLabel ID="lblNoTags" runat="server" ResourceString="content.tags.nodata" EnableViewState="False" CssClass="form-control-text InfoLabel" Visible="false" />
<div class="form-group">
  <div class="editing-form-label-cell">
    <cms:FormLabel CssClass="control-label" runat="server" ID="lDocumentTagGroupID" Field="DocumentTagGroupID" ResourceString="PageProperties.TagGroup" />
  </div>
  <div class="editing-form-value-cell">
    <cms:FormControl runat="server" ID="iDocumentTagGroupIDInherit" Field="DocumentTagGroupIDInherit" FormControlName="checkboxcontrol">
      <Properties>
        <cms:Property Name="Text" Value="metadata.inherit"/>
      </Properties>
    </cms:FormControl>
    <cms:FormControl runat="server" ID="iDocumentTagGroupID" Field="DocumentTagGroupID" FormControlName="uni_selector" />
  </div>
</div>
<div class="form-group">
  <div class="editing-form-label-cell">
    <cms:FormLabel CssClass="control-label" runat="server" ID="lDocumentTags" Field="DocumentTags" ResourceString="PageProperties.Tags" />
  </div>
  <div class="editing-form-value-cell">
    <cms:FormControl runat="server" ID="iDocumentTags" Field="DocumentTags" FormControlName="tagselector" />
  </div>
</div>
