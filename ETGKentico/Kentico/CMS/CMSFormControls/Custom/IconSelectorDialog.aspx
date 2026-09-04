<%@ Page Language="C#"  MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master" AutoEventWireup="true" CodeBehind="IconSelectorDialog.aspx.cs" Inherits="CMSApp.CMSFormControls.Custom.IconSelectorDialog" %>
<asp:Content ID="cntBeforeContent" ContentPlaceHolderID="plcBeforeContent" runat="server">
    <script src="http://code.jquery.com/jquery-3.3.1.min.js"
			  integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
			  crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://s3.amazonaws.com/icomoon.io/39161/RTHF2886RTHealthWebsite/style.css">
    <link rel="stylesheet" href="/custom/assets/styles/main.css">
    <style>
        .icon-selector{
            padding:40px;
        }
        .icon-list a{
            display:inline-block;
            margin-right:8px;
            padding:5px;
        }

        .icon-list a .icon{
            font-size:30px
        }

        .icon-list a.selected{
            border:2px solid #000
        }

        .dialog-footer{
            text-align:center
        }
        
        [role="button"],
        input[type="submit"],
        input[type="reset"],
        input[type="button"],
        button {
            -webkit-box-sizing: content-box;
               -moz-box-sizing: content-box;
                    box-sizing: content-box;
        }

        /* Reset `button` and button-style `input` default styles */
        input[type="submit"],
        input[type="reset"],
        input[type="button"],
        button {
            background: none;
            border: 0;
            color: inherit;
            /* cursor: default; */
            font: inherit;
            line-height: normal;
            overflow: visible;
            padding: 0;
            -webkit-appearance: button; /* for input */
            -webkit-user-select: none; /* for button */
               -moz-user-select: none;
                -ms-user-select: none;
        }
        input::-moz-focus-inner,
        button::-moz-focus-inner {
            border: 0;
            padding: 0;
        }

        /* Demo */
        [role="button"],
        input[type="submit"],
        input[type="reset"],
        input[type="button"],
        button {
            background-color: #f0f0f0;
            border: 1px solid rgb(0, 0, 0);
            border: 1px solid rgba(0, 0, 0, 0.1);
            border-radius: 0.25em;
            height: 2.5em;
            line-height: 2.5;
            margin: 0.25em;
            padding: 0 1em;
            width: 14em;
        }


       
    </style>
</asp:Content>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="icon-selector">
        <asp:Label runat="server" ID="lblError" CssClass="ErrorLabel" EnableViewState="false"
            Visible="false" />
        <div class="icon-list">        
        <asp:Repeater ID="repIcons" runat="server" >
            <ItemTemplate>
                <a href="#" data-icon="<%# Container.DataItem %>" class="<%# IsSelected(Container.DataItem.ToString()) %>" title="<%# Container.DataItem %>">
                    <div class="icon <%# Container.DataItem %>"></div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
        </div>

    </div>
    <asp:HiddenField ID="hidSelectedIcon" runat="server" ClientIDMode="Static" />

    <script>
        $('.icon-list a').click(function () {
            $('.icon-list a').removeClass('selected')
            $(this).addClass('selected')
            $("#hidSelectedIcon").val($(this).attr("data-icon"))
        });

    </script>
</asp:Content>
