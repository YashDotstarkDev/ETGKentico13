<%@ Control Language="C#" Inherits="CMS.FormEngine.Web.UI.CMSAbstractFormLayout" %> 
<%@ Register TagPrefix="cms" Namespace="CMS.FormEngine.Web.UI" Assembly="CMS.FormEngine.Web.UI" %>
<cms:FormCategory ID="fPageOptions" runat="server" CategoryTitleResourceString="content.metadata.pagesettings">
    <div class="form-group">
        <div class="editing-form-label-cell">
            <cms:FormLabel CssClass="control-label" runat="server" ID="lDocumentPageTitle" Field="DocumentPageTitle" ResourceString="PageProperties.Title" />
        </div>
        <div class="editing-form-value-cell">
            <cms:FormControl runat="server" ID="fDocumentPageTitleInherit" FormControlName="checkboxcontrol" Field="DocumentPageTitleInherit">
                <Properties>
                    <cms:Property Name="Text" Value="metadata.inherit"/>
                </Properties>
            </cms:FormControl>
            <cms:FormControl runat="server" ID="iDocumentPageTitle" Field="DocumentPageTitle" FormControlName="textboxcontrol" />
        </div>
    </div>
    <div class="form-group">
        <div class="editing-form-label-cell">
            <cms:FormLabel CssClass="control-label" runat="server" ID="lDocumentPageDescription" Field="DocumentPageDescription" ResourceString="PageProperties.Decription" />
        </div>
        <div class="editing-form-value-cell">
            <cms:FormControl runat="server" ID="iDocumentPageDescriptionInherit" FormControlName="checkboxcontrol" Field="DocumentPageDescriptionInherit">
                <Properties>
                    <cms:Property Name="Text" Value="metadata.inherit"/>
                </Properties>
            </cms:FormControl>
            <cms:FormControl runat="server" ID="iDocumentPageDescription" Field="DocumentPageDescription" FormControlName="textareacontrol" />
        </div>
    </div>
    <div class="form-group">
        <div class="editing-form-label-cell">
            <cms:FormLabel CssClass="control-label" runat="server" ID="lDocumentPageKeyWords" Field="DocumentPageKeyWords" ResourceString="PageProperties.KeyWords" />
        </div>
        <div class="editing-form-value-cell">
            <cms:FormControl runat="server" ID="iDocumentPageKeyWordsInherit" Field="DocumentPageKeyWordsInherit" FormControlName="checkboxcontrol">
                <Properties>
                    <cms:Property Name="Text" Value="metadata.inherit"/>
                </Properties>
            </cms:FormControl>
            <cms:FormControl runat="server" ID="iDocumentPageKeyWords" Field="DocumentPageKeyWords" FormControlName="textareacontrol" />
        </div>
    </div>
</cms:FormCategory>
