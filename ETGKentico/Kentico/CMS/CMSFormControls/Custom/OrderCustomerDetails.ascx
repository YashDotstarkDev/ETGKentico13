<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderCustomerDetails.ascx.cs" Inherits="CMSApp.CMSFormControls.Custom.OrderCustomerDetails" %>
<style>
    .cms-bootstrap table td {
        padding:5px
    }
</style>
<table>
    <tr>
        <td>First name:</td>
        <td><%#OrderCustomer?.CustomerFirstName %></td>
    </tr>
    <tr>
        <td>Last name:</td>
        <td><%#OrderCustomer?.CustomerLastName %></td>
    </tr>
    <tr>
        <td>
            Email:
        </td>
        <td><%#OrderCustomer?.CustomerEmail %></td>
    </tr>
    <tr>
        <td>Phone:</td>
        <td><%#OrderCustomer?.CustomerPhone %></td>
    </tr>
    <tr>
        <td>State:</td>
        <td><%#OrderCustomer?.GetStringValue("CustomerState", "") %></td>
    </tr>
    <asp:PlaceHolder ID="plcAgent" runat="server" Visible="False">
        <tr>
            <td>Agency name:</td>
            <td><%#OrderCustomer?.GetStringValue("CustomerAgencyName", "") %></td>
        </tr>
        <tr>
            <td>Agency Consultant name:</td>
            <td><%#OrderCustomer?.GetStringValue("CustomerAgentName", "") %></td>
        </tr>
        <tr>
            <td>Agency Email address:</td>
            <td><%#OrderCustomer?.GetStringValue("CustomerAgentEmail", "") %></td>
        </tr>
        <tr>
            <td>Agency phone:</td>
            <td><%#OrderCustomer?.GetStringValue("CustomerAgentPhone", "") %></td>
        </tr>
        <tr>
            <td colspan="2">Agency comments:</td>
        </tr>
        <tr>    
            <td colspan="2">
                <%#OrderCustomer?.GetStringValue("CustomerAgentComment", "") %>
            </td>
        </tr>
    </asp:PlaceHolder>
    
</table>