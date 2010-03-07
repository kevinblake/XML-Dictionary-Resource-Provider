<%@ page language="C#" autoeventwireup="true" codebehind="default.aspx.cs" inherits="Web.Sample.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="_form1" runat="server">
    <div>
        <asp:dropdownlist runat="server" id="_changeLocation" autopostback="true" onselectedindexchanged="SelectLanguage_SelectedIndexChanged">
            <asp:listitem text="English" value="en-GB" />
            <asp:listitem text="French" value="fr-FR" />
        </asp:dropdownlist>
        
        <asp:localize id="_localize1" runat="server" text="<%$Resources:intro, intro_central_country %>" />
    </div>
    </form>
</body>
</html>
