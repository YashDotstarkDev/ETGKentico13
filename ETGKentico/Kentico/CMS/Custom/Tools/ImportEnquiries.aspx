<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportEnquiries.aspx.cs" Inherits="CMSApp.Custom.Tools.ImportEnquiries" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div> 
            <asp:FileUpload runat="server" id="upload"/>
            <asp:Button runat="server" id="btn" OnClick="btn_OnClick" Text="Import" />
        </div>
    </form>
</body>
</html>
