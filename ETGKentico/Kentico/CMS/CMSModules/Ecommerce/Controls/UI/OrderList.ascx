<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_UI_OrderList"
     Codebehind="OrderList.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<cms:CMSUpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <cms:UniGrid runat="server" ID="gridElem" IsLiveSite="false" OrderBy="OrderDate DESC" FilterLimit="0"
            DisplayFilter="true" RememberStateByParam="customerId" RememberDefaultState="true"
            Columns="OrderID,OrderInvoiceNumber,OrderCustomerID,OrderDate,OrderGrandTotal,OrderGrandTotalInMainCurrency,OrderPaymentOptionID,OrderIsPaid,OrderShippingOptionID,
            OrderCurrencyID,OrderTrackingNumber,OrderNote,OrderStatusID,OrderAmountPaid,OrderTourCode,OrderTopdogNumber,OrderTourDepartureDate,OrderUtmCampaign,OrderUtmContent,
            OrderUtmMedium,OrderUtmSource,OrderUtmTerm"
            ObjectType="ecommerce.order">
            <GridActions>
                <ug:Action Name="edit" Caption="$General.Edit$" FontIconClass="icon-edit" FontIconStyle="Allow" />
                <ug:Action Name="#delete" Caption="$General.Delete$" FontIconClass="icon-bin" FontIconStyle="Critical"
                    Confirmation="$General.ConfirmDelete$" ExternalSourceName="delete" />
                <ug:Action Name="cancel" Caption="Cancel Order" FontIconClass="icon-cancel" FontIconStyle="Critical"
                    Confirmation="Are you sure you want to cancel" ExternalSourceName="cancel" />
                <%--<ug:Action Name="previous" Caption="$Unigrid.Order.Actions.PreviousStatus$" FontIconClass="icon-chevron-left" />
                <ug:Action Name="next" Caption="$Unigrid.Order.Actions.NextStatus$" FontIconClass="icon-chevron-right" />--%>
            </GridActions>
            <GridColumns>
                <ug:Column Name="IDAndInvoice" Source="##ALL##" ExternalSourceName="IDAndInvoice"
                    Caption="$Unigrid.Order.Columns.OrderID$" Sort="OrderID" Wrap="false" />
                <ug:Column Name="TopDogNumber" Source="OrderTopDogNumber" Caption="Topdog Number" AllowSorting="false" Wrap="false">
                    <Tooltip Encode="true" Source="OrderCustomerID" ExternalSourceName="#transform: ecommerce.customer : {% CustomerEmail %}" />
                </ug:Column>
                <ug:Column Name="Customer" Source="OrderCustomerID" ExternalSourceName="#transform: ecommerce.customer : {% CustomerInfoName %}" Caption="$Unigrid.Order.Columns.OrderCustomerFullName$"
                    AllowSorting="false" Wrap="false">
                    <Tooltip Encode="true" Source="OrderCustomerID" ExternalSourceName="#transform: ecommerce.customer : {% CustomerEmail %}" />
                </ug:Column>
                <ug:Column Name="Date" Source="OrderDate" ExternalSourceName="#userdatetimegmt" Caption="$Unigrid.Order.Columns.OrderDate$"
                    Wrap="false" />
                <ug:Column Name="TourCode" Source="OrderTourCode" Caption="Tour code"
                           Wrap="false" />
                <ug:Column Name="OrderDepartureDate" Source="OrderTourDepartureDate"  ExternalSourceName="departureDate" Caption="Departure date"
                           Wrap="false" />
                <ug:Column Name="MainCurrencyPrice" Source="##ALL##" ExternalSourceName="GrandTotalInMainCurrency"
                    Caption="$Unigrid.Order.Columns.OrderTotalPrice$" Sort="OrderGrandTotalInMainCurrency"
                    Wrap="false" CssClass="TextRight" />
                <ug:Column Name="OrderPrice" Source="##ALL##" ExternalSourceName="GrandTotalInOrderCurrency"
                    Caption="$com.orderlist.ordercurrencycaption$" Wrap="false"
                    CssClass="TextRight" />
                <ug:Column Name="OrderStatus" Source="OrderStatusID" ExternalSourceName="statusName"
                    Caption="$Unigrid.Order.Columns.OrderStatusID$" AllowSorting="false" Wrap="false" />
                <ug:Column Name="IsPaid" Source="OrderIsPaid" ExternalSourceName="#yesno" Caption="$Unigrid.Order.Columns.OrderIsPaid$"
                    Wrap="false" />
                <ug:Column Name="OrderAmountPaid" Source="OrderAmountPaid" Caption="Amount paid" ExternalSourceName="amountpaid"
                           Wrap="false" />
                <ug:Column Name="OrderAmountDue" Source="OrderAmountPaid" ExternalSourceName="amountdue" Caption="Amount due"
                           Wrap="false" />
                <ug:Column Name="Note" Source="OrderNote" ExternalSourceName="Note" Caption="$com.orderlist.notecaption$"
                    Wrap="false">
                    <Tooltip Encode="true" Source="OrderNote" />
                </ug:Column>
                <ug:Column Name="PGCutOff" Source="##ALL##" ExternalSourceName="PGCutOff" Caption="PG cut off"  Wrap="false" />
                
                <%-- UTM fields --%>
                <ug:Column Name="UtmCampaign" Source="OrderUtmCampaign" Caption="UTM campaign" Wrap="false" />
                <ug:Column Name="UtmContent" Source="OrderUtmContent" Caption="UTM content" Wrap="false" />
                <ug:Column Name="UtmMedium" Source="OrderUtmMedium" Caption="UTM medium" Wrap="false" />
                <ug:Column Name="UtmSource" Source="OrderUtmSource" Caption="UTM source" Wrap="false" />
                <ug:Column Name="UtmTerm" Source="OrderUtmTerm" Caption="UTM term" Wrap="false" />
                
                <ug:Column Source="OrderID" Visible="false" />
                <ug:Column CssClass="filling-column" />
            </GridColumns>
            <GridOptions DisplayFilter="true" FilterPath="~/CMSModules/Ecommerce/Controls/Filters/OrderFilter.ascx" />
        </cms:UniGrid>
    </ContentTemplate>
</cms:CMSUpdatePanel>
