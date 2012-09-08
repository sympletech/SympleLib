<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quickstart: Downloading from the filesystem</title>
    <style type="text/css">
        body {font-family:Calibri, Arial, sans-serif}
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <h1>Quickstart: Downloading from the filesystem</h1>
        <h2>Files</h2>
        <asp:Repeater ID="fileRepeater" runat="Server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li><a href="Download.ashx?file=<%# Container.DataItem %>"><%# Container.DataItem %></a></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
