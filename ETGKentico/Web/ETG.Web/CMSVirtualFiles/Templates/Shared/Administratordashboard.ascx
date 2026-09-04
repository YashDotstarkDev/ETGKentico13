<%@ Control Language="C#" Inherits="CMS.PortalEngine.Web.UI.CMSAbstractLayout" %> 
<%@ Register TagPrefix="cc1" Namespace="CMS.DocumentEngine.Web.UI" Assembly="CMS.DocumentEngine.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="CMS.PortalEngine.Web.UI" Assembly="CMS.PortalEngine.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="CMS.DocumentEngine" Assembly="CMS.DocumentEngine" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td colspan="3">
      <cms:CMSWebPartZone ZoneID="zoneTop" runat="server" />      
    </td>
  </tr>
  <tr>
    <td colspan="3">
      <cms:CMSWebPartZone ZoneID="DashboardTop" runat="server" />      
    </td>
  </tr>
  <tr valign="top">
    <td style="width:33%">
      <cms:CMSWebPartZone ZoneID="zoneLeft" runat="server" />      
    </td>
    <td style="width:33%">
      <cms:CMSWebPartZone ZoneID="zoneCenter" runat="server" />      
    </td>
    <td style="width:33%">
      <cms:CMSWebPartZone ZoneID="zoneRight" runat="server" />      
    </td>
  </tr>
   <tr>
    <td colspan="3">
      <cms:CMSWebPartZone ZoneID="DashBoardBottom" runat="server" />      
    </td>
  </tr>
</table>
