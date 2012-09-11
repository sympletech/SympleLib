<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quickstart: Uploading to the filesystem</title>
    <style type="text/css">
        body {font-family:Calibri, Arial, sans-serif}
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <h1>Quickstart: Uploading to the filesystem</h1>
        <p><input id="uploadFile" type="file" runat="server" /></p>
        <p><asp:Button ID="uploadButton" runat="server" Text="Upload" OnClick="uploadButton_Click" /></p>
    </form>
</body>
</html>
