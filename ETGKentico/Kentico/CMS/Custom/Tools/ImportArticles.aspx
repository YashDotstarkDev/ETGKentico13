<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportArticles.aspx.cs" Inherits="CMSApp.Custom.Tools.ImportArticles" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div> 
            <asp:DropdownList ID="ddlDest" runat="server">
                <asp:ListItem Value="">Barge</asp:ListItem>
                <asp:ListItem Value="A957464C-3705-4037-8817-FC5BD03CEFDE">Italy</asp:ListItem>
                <asp:ListItem Value="492ED09D-5429-4AB4-8F22-DBB81B1C21D8">France</asp:ListItem>
                <asp:ListItem Value="A0FC2FEB-F876-40F7-86E8-E56AE8917E94">Spain</asp:ListItem>
                <asp:ListItem Value="60C5B616-654D-4078-91EE-939DA3E35ED5">Portugal</asp:ListItem>
                <asp:ListItem Value="D8AE4068-503E-43FB-B238-D01B33EA2B07">Switzerland</asp:ListItem>
                <asp:ListItem Value="5D5B828F-4776-4C1B-98BF-117FC7DEB023">Canada</asp:ListItem>
                <asp:ListItem Value="1788A775-F873-4ECB-911F-3CACA3784510">Alaska</asp:ListItem>
                <asp:ListItem Value="07987524-186E-4437-A498-7EBE0346BAAB">New Caledonia</asp:ListItem>
                <asp:ListItem Value="BBAB54DF-500E-4FBE-BEBA-4D4DA9F829F3">Maldives</asp:ListItem>
                <asp:ListItem Value="6BF80C58-3C66-48D8-8532-5105ACA28189">Monaco</asp:ListItem>
                <asp:ListItem Value="5B075623-20B3-44DC-AE4D-37FEA5AD32EA">Tahiti</asp:ListItem>
                <asp:ListItem Value="DCE54CCB-15CA-434F-B99B-2E69D6A75070">Malta</asp:ListItem>
            </asp:DropdownList>
            <asp:TextBox runat="server" id="txt" TextMode="MultiLine"></asp:TextBox>
            <asp:Button runat="server" id="btn" OnClick="btn_OnClick"/>
        </div>
    </form>
</body>
</html>
