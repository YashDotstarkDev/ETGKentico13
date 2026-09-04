<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportProducts.aspx.cs" Inherits="CMSApp.Custom.Tools.ImportProducts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox runat="server" id="txt" TextMode="MultiLine"></asp:TextBox>
            <asp:Button runat="server" id="btn" OnClick="btn_OnClick"/>
        </div>
    </form>
</body>
</html>
