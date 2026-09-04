<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditQuote.aspx.cs"  Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"  Inherits="CMSApp.Custom.ETGBooking.PageTemplates.EditQuote" %>
<%@ Import Namespace="ETG.Data.Extensions" %>
<%@ Import Namespace="Castle.Core.Internal" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="plcBeforeContent">
    <style>
    .two-column-form{
    display:flex;
    }
        .two-column-form .column-left{
            width:500px;
        }
        
        .two-column-form .column-right{
            padding-left:40px;
            border-left:2px solid #ccc
        }
        .two-column-form .email-form{
            padding-top:20px;
        }
        .two-column-form .column-right .control-label{
            text-align: left;
        }
        .two-column-form .column-right textarea{
            width:500px;
            height:300px;
        }
        
        .two-column-form .editing-form-label-cell{
            width:200px;
        }
        .two-column-form .editing-form-value-cell{
            width:200px;
        }
        
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
                    <h3>Travel Agent Quote</h3>
                 <div class="editing-form-category-fields">
                    <div class="two-column-form">
                        <div class="form-column column-left">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Topdog Quote:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <asp:TextBox runat="server" id="txtTopDog" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Quote ID:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litQuoteId" /></span>
                                        <asp:HiddenField runat="server" id="hidQuoteID" ClientIDMode="Static"/>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Quote Created:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litQuoteCreated" /></span>
                                        <asp:HiddenField runat="server" id="HiddenField2" ClientIDMode="Static"/>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <span class="control-label editing-form-label">Valid days:</span>
                                </div>
                                <div class="editing-form-value-cell">
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litValidDays" /></span>
                                        <asp:HiddenField runat="server" id="HiddenField1" ClientIDMode="Static"/>
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
                                            <asp:TextBox runat="server" id="txtAdvisorEmail" CssClass="form-control"></asp:TextBox>
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
                            <asp:PlaceHolder runat="server" id="plcAgentCustomerDetails" Visible="False">
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
                                            <asp:TextBox runat="server" id="txtLeadPassengerEmail" CssClass="form-control"></asp:TextBox>
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
                                        <span class="control-label editing-form-label">Comments to customer:</span>
                                    </div>
                                    <div class="editing-form-value-cell">
                                        <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                            <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAgentCustomerComments" /></span>
                                        </div>
                                    </div>
                                </div>
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
                            
                           <%-- <asp:PlaceHolder runat="server" id="plcAddedServices" Visible="False">
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
                            </div>
                        </div>--%>
                        </div>
                    <asp:PlaceHolder ID="plcEmail" runat="server">
                        <div class="form-column column-right">
                            
                            <asp:Button id="btnEmail" runat="server" onclick="btnEmail_Click" Class="btn btn-primary" Text="Resend quote"></asp:Button>
                            <div>
                                <asp:Literal runat="server" ID="litEmailMessage"></asp:Literal>
                            </div>
                            <div class="email-form form-group">
                                <div>
                                    <span class="control-label editing-form-label">Email Comments:</span>
                                </div>
                                <div>
                                    <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                        <asp:TextBox runat="server" id="txtEmailComments" CssClass="form-control" TextMode="MultiLine" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                    </asp:PlaceHolder>
                    
                    </div>
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
                            <span class="control-label editing-form-label">Booking Summary</span>
                        </div>
                        <div class="editing-form-value-cell">
                            <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                <div class="summary-table"><asp:Literal runat="server" ID="litSummary" /></div>
                            </div>
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