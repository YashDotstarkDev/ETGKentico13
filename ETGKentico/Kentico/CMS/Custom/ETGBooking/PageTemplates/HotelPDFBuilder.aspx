<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotelPDFBuilder.aspx.cs" Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"  Inherits="CMSApp.CMSModules.ETGBooking.PageTemplates.HotelPDFBuilder" %>
<%@ Import Namespace="CMS.SiteProvider" %>
<asp:Content ContentPlaceHolderID="plcBeforeBody" runat="server">
    <style>
        .cms-bootstrap .editing-form-value-cell .table-hotels {
            word-spacing:0;
        }
        .table-hotels td{
            padding:3px
        }

    </style>
    <link  href="<%=SiteContext.CurrentSite.SitePresentationURL %>/styles/global.css" rel="stylesheet">
    <noscript><link rel="stylesheet" href="<%=SiteContext.CurrentSite.SitePresentationURL %>/styles/global.css"></noscript>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.13.1/themes/base/jquery-ui.css">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="plcBeforeContent">
    <div id="m_pnlActions" class="cms-edit-menu">
        <div id="m_actionsElem_pnlMenu" class="object-edit-panel">
            <div id="m_actionsElem_editMenuElem_pC" class="header-actions-container">
                <div id="m_actionsElem_editMenuElem_menu_pnlUp">
                    <div id="m_actionsElem_editMenuElem_menu_pnlActions" class="header-actions-main">
                        <asp:Button runat="server" ID="btnCreate" Text="Create" OnClick="btnCreate_Click" CssClass="btn btn-primary"/>
                    </div>
                </div>
                <div class="Clear">
                </div>
                <asp:Literal ID="litCreateMessage" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="no-loader"></div>
    <div class="PageContent">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label runat="server" CssClass="control-label" EnableViewState="false">Package code:</asp:Label>
                </div>
                <div class="editing-form-value-cell">
                    <asp:TextBox ID="txtTourCode" runat="server" CssClass="form-control" /><br />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTourCode" ErrorMessage="Please enter package code." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label runat="server" CssClass="control-label" EnableViewState="false">Booking number:</asp:Label>
                </div>
                <div class="editing-form-value-cell">
                    <asp:TextBox ID="txtBookingNumber" runat="server" CssClass="form-control" /><br />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBookingNumber" ErrorMessage="Please enter booking number." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label runat="server" CssClass="control-label" EnableViewState="false">Travel start date (dd/mm/yyyy):</asp:Label>
                </div>
                <div class="editing-form-value-cell">
                    <asp:TextBox ID="txtTravelStartDate" runat="server" CssClass="datepicker form-control" /><br />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTravelStartDate" ErrorMessage="Please enter travel start date." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    </div>
                <div class="editing-form-value-cell">
                    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-secondary" OnClick="btnNext_OnClick" />
                    </div>
            </div>
            <div class="form-group" <%=HotelSelectStyle%>>
                <div class="editing-form-label-cell">
                    <asp:Label runat="server" CssClass="control-label" EnableViewState="false">Hotels:</asp:Label>
                </div>
                <div class="editing-form-value-cell">
                    <div class="hotel-builder" data-endpoint="<%=SiteContext.CurrentSite.SitePresentationURL %>/api/hotel/search" data-method="GET">
                            <div class="ui form">
                                <div class="step-one">
                                    <div class="label">
                                        Hotel search
                                    </div>
                                    <input type="text" class="search-field" placeholder="Please enter keyword or country"/>

                                    <div class="checkboxes">
                                        <div class="label">Hotels</div>
                                        <div class="inner"></div>
                                        <div class="selected-hotels">
                                            <div class="label">Selected Hotels</div>
                                            <div class="inner-container"></div>
                                        </div>
                                    </div>
                                    <div class="buttons">
                                        <a href="#" class="btn btn-secondary clear">Reset</a>
                                        <a href="#" class="btn btn-secondary show-dropdowns">Next</a>
                                    </div>
                                </div>
                                <div class="step-two">
                                    <div class="dropdowns"></div>
                                    <asp:HiddenField ID="hidHotelIds" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hidNights" runat="server" ClientIDMode="Static" />
                                </div>
                            </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div>
            <asp:Literal runat="server" ID="litMessage"></asp:Literal>
        </div>
    </div>
    <script src="https://code.jquery.com/jquery-3.6.0.js"></script>
      <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/fuse.js/dist/fuse.js"></script>
    <script src="<%=SiteContext.CurrentSite.SitePresentationURL %>/scripts/runtime.js"></script>
    <script src="<%=SiteContext.CurrentSite.SitePresentationURL %>/scripts/global.js"></script>
    <script src="<%=SiteContext.CurrentSite.SitePresentationURL %>/scripts/hotelbuilder.js"></script>

</asp:Content>