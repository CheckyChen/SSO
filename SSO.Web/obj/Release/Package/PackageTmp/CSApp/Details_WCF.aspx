<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details_WCF.aspx.cs" Inherits="SSO.Web.CSApp.Details_WCF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        table {
            border: 1px solid #ccc;
            text-align: left;
            width:90%;
            margin:10px;
        }
        td,th {
            border:1px solid #ccc;
        }
    </style>
</head>
<body>
    <div>
         <p>版  本  号：<label id="VersionCode" runat="server"></label> &nbsp;&nbsp;&nbsp;创 建  人：<label id="CreateUserId" runat="server"></label></p>
         <p>上传时间：<label id="CreateTime" runat="server"></label></p>         
         <div id="FileTable" runat="server">
        </div>
    </div>
</body>
</html>
