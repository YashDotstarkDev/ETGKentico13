<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotelBuilder.aspx.cs" Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"  Inherits="CMSApp.CMSModules.ETGBooking.PageTemplates.HotelBuilder" %>
<asp:Content ContentPlaceHolderID="plcBeforeBody" runat="server">
    <style>
        .cms-bootstrap .editing-form-value-cell .table-hotels {
            word-spacing:0;
        }

        .table-hotels td{
            padding:3px
        }
    </style>
    <script>
        function RefreshHiddenValue() {

            var value = '';
            $cmsj('.chkHotel').each(function (e) {

                if ($cmsj(this).is(":checked")) {
                    if (value == '') {
                        value = $cmsj(this).val();
                    } else {
                        value += ',' + $cmsj(this).val();
                    }
                }
            });

            $cmsj('#hidHotelIds').val(value);
        }


        $cmsj(document).ready(function (e) {
            $cmsj(".table-hotels input[type='checkbox']").click(function (el) {
                RefreshHiddenValue();
            })
        })
    </script>
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
                    <asp:TextBox ID="txtTourCode" runat="server" /><br />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTourCode" ErrorMessage="Please enter package code." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label runat="server" CssClass="control-label" EnableViewState="false">Booking number:</asp:Label>
                </div>
                <div class="editing-form-value-cell">
                    <asp:TextBox ID="txtBookingNumber" runat="server" /><br />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBookingNumber" ErrorMessage="Please enter booking number." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label runat="server" CssClass="control-label" EnableViewState="false">Hotels:</asp:Label>
                </div>
                <div class="editing-form-value-cell">
                    <asp:Literal ID="litHotels" runat="server" />
                    <asp:Repeater ID="repHotels" runat="server">
                        <HeaderTemplate><table class="table-hotels"></HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><input class="chkHotel" type="checkbox" value="<%# Eval("HotelID")  %>" /></td>
                                <td><%#Eval("HotelName") %></td>
                                <td><%#Eval("ParentName") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate></table></FooterTemplate>
                    </asp:Repeater>
                    <asp:HiddenField ID="hidHotelIds" runat="server" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        
        <div>
            <asp:Literal runat="server" ID="litMessage"></asp:Literal>
        </div>
    </div>
</asp:Content>