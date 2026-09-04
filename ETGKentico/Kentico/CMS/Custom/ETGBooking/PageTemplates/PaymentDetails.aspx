<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentDetails.aspx.cs" Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"  Inherits="CMSApp.CMSModules.ETGBooking.PageTemplates.PaymentDetails" %>
<%@ Import Namespace="ETG.Data.Extensions" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="plcBeforeContent">
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="PageContent">
        <div id="m_c_editOrderGeneral">
            <div class="form-horizontal">
                <div>
                    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
                </div>
                <div class="editing-form-category category">
                    <h3>Payment Details</h3>
                    <div class="editing-form-category-fields">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Payment ID:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litPaymentId" /></span>
                                    <asp:HiddenField runat="server" id="hidPaymentID" ClientIDMode="Static"/>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Merchant Unique Payment ID:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litMerchatID" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Payment Created:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litPaymentCreated" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">First Name:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litFirstName" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Last Name:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litLastName" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Email Address:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litEmail" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Contact number:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litContactNumber" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Invoice reference:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litInvoiceReference" /></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <span class="control-label editing-form-label">Amount:</span>
                            </div>
                            <div class="editing-form-value-cell">
                                <div class="EditingFormControlNestedControl editing-form-control-nested-control">
                                    <span class="LabelField form-control-text"><asp:Literal runat="server" ID="litAmount" /></span>
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