<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VersionForm.aspx.cs" Inherits="SSO.Web.CSApp.VersionForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>版本管理</title>
    <link href="../Content/css/framework-font.css" rel="stylesheet" />
    <link href="../Content/css/framework-theme.css" rel="stylesheet" />
    <script src="../Content/js/jquery/jquery-2.1.1.min.js"></script>
    <script src="../Content/js/bootstrap/bootstrap.js"></script>
    <link href="../Content/js/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/css/framework-ui.css" rel="stylesheet" />
    <script src="../Content/js/framework-ui.js"></script>

    <link href="../Content/js/webuploader/webuploader.css" rel="stylesheet" type="text/css" />
    <link href="../Content/js/webuploader/style.css" rel="stylesheet" type="text/css" />

    <%-- <script src="../Content/js/webuploader/jquery.js" type="text/javascript"></script>--%>
    <script src="../Content/js/webuploader/webuploader.js" type="text/javascript"></script>
    <script src="../Content/js/webuploader/upload.js" type="text/javascript"></script>
    <style>
        .importField {
            color:red;
        }
    </style>
</head>
<body>
    <script>

        $(function () {
            InitData();
         
            $("#VersionCode").on("onblur", function () {
                tmp();
            });
        });
        var applicationId = $.request("applicationId");
        //初始化
        function InitData() {
            var applicationName = unescape($.request("applicationName"));

            //alert(applicationName);
            if (!!applicationId) {
                $("#ApplicationId").val(applicationId);
                $("#ApplicationName").val(applicationName);
                GetPreviVersion();
            }
        }

        //获取上个版本版本号
        function GetPreviVersion() {
            $.ajax({
                url: "../Handle/CSAppHandler.ashx?op=GetPreviVersion",
                data: { ApplicationId: applicationId },
                dataType: "text",
                async: false,
                success: function (data) {
                    $("#previVersion").text(data);
                }
            });
        }

        function tmp() {
            //当前上传版本
            var versionCode = $("#VersionCode").val() + '';
            //上个版本
            var previVersion = $("#previVersion").text() + '';

            if (versionCode <= previVersion) {
                             
                $.modalMsg("版本号" + versionCode + "必须大于上个版本" + previVersion + "", "success");
                $("#VersionCode").focus();
                return false;
            } else {
                            
            }           
        }
    </script>
    <form id="form1">
        <input type="hidden" id="ApplicationId" name="ApplicationId" runat="server" />
        <input type="hidden" id="AppVersionId" name="AppVersionId" runat="server" />
        <div style="margin-top: 10px; margin-left: 10px; margin-right: 10px;">
            <div style="padding-top: 20px; margin-right: 30px;">
                <table class="form">
                    <tr>
                        <th class="formTitle">应用名称</th>
                        <td class="formValue">
                            <input id="ApplicationName" name="ApplicationName" type="text" class="form-control" readonly="readonly" placeholder="应用名称" />
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">版本号<sapn class="importField">*</span></th>
                        <td class="formValue">
                            <input id="VersionCode" name="VersionCode" onblur="" type="text" class="form-control required" placeholder="请输入版本号" style="width: 60%; float: left;" /><span style="float: left; font-size: 13px; margin-top: 5px;">&nbsp;上个版本号：<span id="previVersion"></span></span>
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">备注</th>
                        <td class="formValue">
                            <input id="Remark" name="Remark" type="text" class="form-control" placeholder="请输入备注" />
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
                    <%--<tr>
                        <th class="formTitle">文件名称</th>
                        <td class="formValue">
                            <input id="AppFileName" name="AppFileName" type="text" class="form-control required" placeholder="" readonly="readonly" />
                        </td>
                    </tr>--%>
                    <tr>
                        <th class="formTitle">文件总大小</th>
                        <td class="formValue">
                            <input id="AppSize" name="AppSize" type="text" class="form-control" placeholder="" readonly="readonly" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
