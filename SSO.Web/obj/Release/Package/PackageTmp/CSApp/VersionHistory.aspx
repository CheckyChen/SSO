<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VersionHistory.aspx.cs" Inherits="SSO.Web.CSApp.VersionHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>   
    <script src="../Content/js/webuploader/jquery-1.4.1.min.js"></script>
</head>
<body>
    <script>
        function deleteVersion(obj) {

            var versionId = $(obj).attr("data-id");
            
            if (confirm("您确定删除该版本吗？")) {
                $.ajax({
                    url: "../Handle/CSAppHandler.ashx?op=DeleleVersion&AppVersionId=" + versionId,
                    dataType: "json",
                    success: function (data) {
                        if (data.state == "success") {                           
                            window.location.reload();
                            parent.$("#gridList").trigger("reloadGrid");
                        }
                    }
                });
            }
        }
    </script>
    <h2 id="AppName" runat="server"></h2>
    <p runat="server" id="p_Company"></p>
    <ul id='timeline' runat="server">
    </ul>

    <link href="../Content/css/timeline.css" rel="stylesheet" />
</body>

</html>
