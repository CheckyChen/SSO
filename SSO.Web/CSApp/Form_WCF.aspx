<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form_WCF.aspx.cs" Inherits="SSO.Web.CSApp.Form_WCF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>本地服务WCF更新包管理</title>

    <link href="../Content/css/framework-font.css" rel="stylesheet" />
    <link href="../Content/css/framework-theme.css" rel="stylesheet" />
    <script src="../Content/js/jquery/jquery-2.1.1.min.js"></script>
    <script src="../Content/js/bootstrap/bootstrap.js"></script>
    <link href="../Content/js/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/css/framework-ui.css" rel="stylesheet" />
    <script src="../Content/js/framework-ui.js"></script>

    <link href="../Content/js/webuploader/webuploader.css" rel="stylesheet" type="text/css" />
    <link href="../Content/js/webuploader/style.css" rel="stylesheet" type="text/css" />
    <script src="../Content/js/webuploader/webuploader.js" type="text/javascript"></script>
    <script src="../Content/js/webuploader/upload_wcf.js" type="text/javascript"></script>
</head>
<body>
    <script>
    </script>
    <form id="form1">
        <input type="hidden" runat="server" id="WCFVersionInfoId" />
        <div style="margin-top: 10px; margin-left: 10px; margin-right: 10px;">
            <div style="padding-top: 20px; margin-right: 30px;">
                <table class="form">
                    <tr>
                        <th class="formTitle">版本号</th>
                        <td class="formValue">
                            <input id="VersionCode" name="VersionCode" onblur="" type="text" class="form-control required" placeholder="请输入版本号" style="width: 60%; float: left;" /><span style="float: left; font-size: 13px; margin-top: 5px;">&nbsp;上个版本号：<span runat="server" id="previVersion"></span></span>
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">备注</th>
                        <td class="formValue">
                            <input id="Remark" name="Remark" type="text" class="form-control required" placeholder="备注" />
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle"></th>
                        <td class="formValue">
                            <!-- <div id="wrapper">-->
                            <div id="container">
                                <!--头部，相册选择和格式选择-->
                                <div id="uploader">
                                    <div class="queueList">
                                        <div id="dndArea" class="placeholder">
                                            <div id="filePicker"></div>
                                        </div>
                                    </div>
                                    <div class="statusBar" style="display: none;">
                                        <div class="progress">
                                            <span class="text">0%</span>
                                            <span class="percentage"></span>
                                        </div>
                                        <div class="info"></div>
                                        <div class="btns">
                                            <div id="filePicker2"></div>
                                            <div class="uploadBtn">开始上传</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--</div>-->
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">文件大小</th>
                        <td class="formValue">
                            <input id="FileSize" name="FileSize" type="text" class="form-control" placeholder="文件大小" readonly="readonly" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
