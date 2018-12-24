<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="SSO.Web.CSApp.Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>应用新增</title>
    <link href="../Content/css/framework-font.css" rel="stylesheet" />
    <link href="../Content/css/framework-theme.css" rel="stylesheet" />
    <script src="../Content/js/jquery/jquery-2.1.1.min.js"></script>
    <script src="../Content/js/bootstrap/bootstrap.js"></script>
    <link href="../Content/js/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../Content/js/validate/jquery.validate.min.js"></script>
    <link href="../Content/css/framework-ui.css" rel="stylesheet" />
    <script src="../Content/js/framework-ui.js"></script>
        <style>
        .importField {
            color:red;
        }
    </style>
</head>
<body>
    <script>

        var keyValue = $.request("keyValue");

        $(function () {

            InitData();

        });
        //初始化
        function InitData() {
            if (!!keyValue) {
                $.ajax({
                    url: "../Handle/CSAppHandler.ashx?op=GetApplicationEntity",
                    data: { ApplicationId: keyValue },
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        $("#form1").formSerialize(data.data[0]);
                    }
                });
            }
        }

        //提交
        function submitForm() {

            if (!$('#form1').formValid()) {
                return false;
            }

            $.ajax({
                url: "../Handle/CSAppHandler.ashx?op=SubmitForm&ApplicationId=" + keyValue,
                data: { postData: JSON.stringify($("#form1").formSerialize()) },
                dataType: "json",
                async: false,
                success: function (data) {
                    $.modalMsg(data.message, data.state);
                    if (data.state == "success") {
                        parent.$("#gridList").trigger("reloadGrid");
                        $.modalClose();
                    }
                }
            });
        }
    </script>
    <form id="form1">
        <input type="hidden" id="ApplicationId" name="ApplicationId" runat="server" />
        <div style="margin-top: 10px; margin-left: 10px; margin-right: 10px;">
            <div style="padding-top: 20px; margin-right: 30px;">
                <table class="form">
                    <tr>
                        <th class="formTitle">应用名称<sapn class="importField">*</span></th>
                        <td class="formValue">
                            <input id="ApplicationName" name="ApplicationName" type="text" class="form-control required" placeholder="请输入应用名称" />
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">应用编码<sapn class="importField">*</span></th>
                        <td class="formValue">
                            <input id="ApplicationCode" name="ApplicationCode" type="text" class="form-control required" placeholder="请输入应用编码" />
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">启动文件<sapn class="importField">*</span></th>
                        <td class="formValue">
                            <input id="StartFileName" name="StartFileName" type="text" class="form-control required" placeholder="启动文件（文件名.后缀）" />
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">所属厂商</th>
                        <td class="formValue">
                            <input id="Company" name="Company" type="text" class="form-control" placeholder="请输入所属厂商" />
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">排序码</th>
                        <td class="formValue">
                            <input id="SortCode" name="SortCode" type="text" class="form-control" placeholder="请输入排序码" />
                        </td>
                    </tr>
                    <tr>
                        <th class="formTitle">备注</th>
                        <td class="formValue">
                            <input id="Remark" name="Remark" type="text" class="form-control" placeholder="请输入备注" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
