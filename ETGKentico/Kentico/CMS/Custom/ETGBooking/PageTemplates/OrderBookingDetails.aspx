<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderBookingDetails.aspx.cs" Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"  Inherits="CMSApp.CMSModules.ETGBooking.PageTemplates.OrderBookingDetails" %>
<%@ Import Namespace="ETG.Data.Extensions" %>
<%@ Import Namespace="Castle.Core.Internal" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="plcBeforeContent">
    <style>
        .price-display{
            display:inline-block;
            width:90px;
        }
    </style>
    <div id="m_pnlActions" class="cms-edit-menu">
        <div id="m_actionsElem_pnlMenu" class="object-edit-panel">
            <div id="m_actionsElem_editMenuElem_pC" class="header-actions-container">
                <div id="m_actionsElem_editMenuElem_menu_pnlUp">
                    <div id="m_actionsElem_editMenuElem_menu_pnlActions" class="header-actions-main">
                        <asp:Button id="btnSave" runat="server" onclick="btnSave_Click" Class="btn btn-primary" Text="Save"></asp:Button>
                        <asp:Button id="btnSendRefund" runat="server" onclick="btnSendRefund_Click" Class="btn btn-primary" Text="Send Apply Refund Email"></asp:Button>
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
        <div id="m_c_editOrderGeneral">
            <div class="form-horizontal">
                <div>
                    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
                </div>
                <div class="editing-form-category category">
                    <h3><asp:Literal runat="server" ID="litHeading">Direct Client Booking</asp:Literal></h3>
                    <div class="editing-form-category-fields">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Topdog Number:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <asp:TextBox runat="server" id="txtTopDog" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <asp:PlaceHolder runat="server" id="plcQuote" Visible="false">
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Quote ID:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litQuoteID" /></span>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Order ID:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litOrderId" /></span>
                                    <asp:HiddenField runat="server" id="hidOrderID" ClientIDMode="Static"/>
                                </div>
                            </div>
                        </div>
                        <asp:PlaceHolder runat="server" id="plcAgent" Visible="False">
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Travel Agency Trading Name:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentTradingName" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Travel Agency Post Code:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentPostcode" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Travel Agency Phone Number:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentPhone" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Travel Advisor Name:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentAdvisorName" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Travel Advisor Email:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentEmail" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Agency Comments:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgencyComments" /></span>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" id="plcCustomer" Visible="False">
                          <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Title:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerTitle" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - First Name:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerFirstName" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Middle Name:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerMiddleName" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Last Name:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerLastName" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Date of Birth:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerDateOfBirth" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Phone number:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerPhone" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Email Address:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerEmail" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - State:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerState" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Comments:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCustomerComments" /></span>
                                    </div>
                                </div>
                            </div>
                             <asp:PlaceHolder runat="server" id="plcSecondPassenger" Visible="False">
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - Title:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litSecondPassengerTitle" /></span>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - First Name:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litSecondPassengerFirstName" /></span>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - Middle Name:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litSecondPassengerMiddleName" /></span>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - Last Name:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litSecondPassengerLastName" /></span>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - Date of Birth:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litSecondPassengerDateOfBirth" /></span>
                                         </div>
                                     </div>
                                 </div>
                             </asp:PlaceHolder>    
                        </asp:PlaceHolder>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Tour code:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litTourCode" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Country:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litCountry" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Tour name:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litTourName" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Tour Date:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litDate" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">On Sale Now:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litOnSaleNow" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Days from order to departure date:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litDaysFromDepartureDate" /> days</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Number of twin/double rooms:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litDoubleRooms" /></span>
                                    <div>
                                        <asp:Literal ID="litTwinShareRoomsType" runat="server"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Number of single rooms:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litSingleRooms" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Room Upgrade:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <div class="form-control-text">
                                        <asp:Repeater runat="server" id="repRoomOptions" ItemType="ETG.Module.Booking.Models.Cart.RoomOptionCartItem">
                                            <ItemTemplate>
                                                <div>Room <%#Container.ItemIndex + 1 %>: <%# Item.OptionDescription.IsNullOrEmpty() ? "No upgrade" : GetRoomTypeLabel(Container.ItemIndex + 1, Item.OptionType) + " - " + Item.OptionDescription + "(" + Item.OptionPricePerPerson.FormatPrice() + ")" %></div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Optional Extra:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <div class="form-control-text">
                                        <asp:Literal runat="server" id="litExtraSelectNow"></asp:Literal>
                                    <asp:Repeater runat="server" id="repExtras" ItemType="ETG.Module.Booking.Models.Cart.RoomOptionCartItem">
                                        <ItemTemplate>
                                            <div><%# Item.OptionDescription %> (<%#Item.OptionPricePerPerson.FormatPrice() %>)</div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Freedom of choice:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <div class="form-control-text">
                                    <asp:Literal runat="server" id="litFocSelectNow"></asp:Literal>
                                    <asp:Repeater runat="server" id="repFreedomOfChoice" ItemType="ETG.Module.Booking.Models.FreedomOfChoiceItem">
                                        <ItemTemplate>
                                            <div><%# Item.DayCaption %>: <%#Item.OptionLabel %></div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--<div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Entire Flex:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litEntireFlex" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Entire Flex FOC:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litFOCEntireFlex" /></span>
                                </div>
                            </div>
                        </div>--%>
                    <asp:PlaceHolder runat="server" id="plcAgentCustomerDetails" Visible="False">
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Title:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerTitle" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - First Name:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerFirstName" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Middle Name:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerMiddleName" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Last Name:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerLastName" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                 <div class="editing-form-label-cell">
                                     <span class="control-label editing-form-label">Lead Passenger Details - Date of Birth:</span>
                                 </div>
                                 <div class="editing-form-value-cell">
                                     <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                         <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerDateOfBirth" /></span>
                                     </div>
                                 </div>
                             </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Phone number:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerPhone" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Email Address:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerEmail" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - State:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerState" /></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Lead Passenger Details - Comments:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerComments" /></span>
                                    </div>
                                </div>
                            </div>
                            <asp:PlaceHolder runat="server" id="plcAgentSecondPassenger" Visible="False">
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - Title:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentSecondPassengerTitle" /></span>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - First Name:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentSecondPassengerFirstName" /></span>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - Middle Name:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentSecondPassengerMiddleName" /></span>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - Last Name:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentSecondPassengerLastName" /></span>
                                         </div>
                                     </div>
                                 </div>
                                <div class="form-group">
                                     <div class="editing-form-label-cell">
                                         <span class="control-label editing-form-label">Second Passenger Details - Date of Birth:</span>
                                     </div>
                                     <div class="editing-form-value-cell">
                                         <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                             <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentSecondPassengerDateOfBirth" /></span>
                                         </div>
                                     </div>
                                 </div>
                             </asp:PlaceHolder>    
                        </asp:PlaceHolder>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Require international fare assistance:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litFareAssistance" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Require travel insurance assistance:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litInsuranceAssistance" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">TravelPay Payment reference:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litTravelPaymentReference" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Funds to Merchant:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litFundsToMerchant" /></span>
                                </div>
                            </div>
                        </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <span class="control-label editing-form-label">Second Instalment date:</span>
                        </div>
                        <div class="editing-form-value-cell">
                            <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litSecondInstallmentDate" /></span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <span class="control-label editing-form-label">Second Instalment amount:</span>
                        </div>
                        <div class="editing-form-value-cell">
                            <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litSecondInstallmentAmount" /></span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <span class="control-label editing-form-label">Final Balance Due date:</span>
                        </div>
                        <div class="editing-form-value-cell">
                            <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litBalanceDueDate" /></span>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlExchangeRate" runat="server" CssClass="form-group" Visible="False">
                        <div class="editing-form-label-cell">
                            <span class="control-label editing-form-label">Exchange rate:</span>
                        </div>
                        <div class="editing-form-value-cell">
                            <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litExchangeRate" /></span>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <span class="control-label editing-form-label">Pre Nights:</span>
                        </div>
                        <div class="editing-form-value-cell">
                            <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litPreNights" /></span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <span class="control-label editing-form-label">Post Nights:</span>
                        </div>
                        <div class="editing-form-value-cell">
                            <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litPostNights" /></span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Promo code:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litPromoCode" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Booking Summary</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <div class="summary-table"><asp:Literal runat="server" ID="litSummary" /></div>
                                </div>
                            </div>
                        </div>
                    <%--<asp:PlaceHolder runat="server" id="plcAddedServices" Visible="False">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Added Services</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <div class="summary-table"><asp:Literal runat="server" ID="litAddedServices" /></div>
                                </div>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <span class="control-label editing-form-label">Total</span>
                        </div>
                        <div class="editing-form-value-cell">
                            <span class="EditingFormControlNestedControl editing-form-control-nested-control">
                            <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litGrandTotal" /></span>
                        </div>
                    </div>--%>
                    </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="plcBeforeBody">
    <style>
        .summary-table table td {
            padding: 5px;
        }

        .summary-table table td.price-cell {
            text-align: right
        }
    </style>
</asp:Content>