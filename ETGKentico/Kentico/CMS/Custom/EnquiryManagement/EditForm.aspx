<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="CMSApp.Custom.EnquiryManagement.EditForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <cms:UIForm runat="server" ID="ctlEditForm" ObjectType="BizFormItem.BizForm.Enquire" RedirectUrlAfterCreate="" SetDefaultValuesToDisabledFields="false"
                        IsLiveSite="false" />
        </div>
    </form>
</body>
</html>
