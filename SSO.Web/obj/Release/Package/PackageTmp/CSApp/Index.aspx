<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SSO.Web.CSApp.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>桌面应用包版本管理</title>
    <script src="../Content/js/loading/pace.min.js"></script>
    <link href="../Content/css/framework-font.css" rel="stylesheet" />
    <link href="../Content/css/framework-theme.css" rel="stylesheet" />
    <link href="../Content/js/dialog/dialog.css" rel="stylesheet" />
    <script src="../Content/js/jquery/jquery-2.1.1.min.js"></script>
    <script src="../Content/js/dialog/dialog.js"></script>
    <script src="../Content/js/bootstrap/bootstrap.js"></script>
    <link href="../Content/js/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../Content/js/jqgrid/jqgrid.min.js"></script>
    <link href="../Content/js/jqgrid/jqgrid.css" rel="stylesheet" />
    <script src="../Content/js/jqgrid/grid.locale-cn.js"></script>
    <link href="../Content/css/framework-ui.css" rel="stylesheet" />
    <script src="../Content/js/framework-ui.js"></script>
    <style>
        #title {
            height: 40px;
            width: 100%;
            background-color: #1ABC9C;
            padding-top: 10px;
            padding-left: 20px;
        }

            #title span:first-child {
                color: white;
                font-size: 20px;
                font-weight: bold;
            }
    </style>
</head>
<body>
    <script>
        $(function () {
            gridList();
        });

        //获取列表
        function gridList() {
            var $gridList = $("#gridList");
            $gridList.dataGrid({
                url: "../Handle/CSAppHandler.ashx?op=GetApplicationList",
                height: $(window).height() - 178,
                colModel: [
                    { label: '主键', name: 'ApplicationId', hidden: true },
                    { label: '应用编码', name: 'ApplicationCode', width: 150, align: 'left' },
                    { label: '应用名', name: 'ApplicationName', width: 200, align: 'left' },
                    { label: '公司', name: 'Company', width: 200, align: 'left' },
                    {
                        label: '历史版本(个)', name: 'versionCount', width: 70, align: 'center', formatter: function (cellvalue, options, rowObject) {
                            if (cellvalue > 0) {
                                return "<span onclick=\"btn_versionHistory('" + rowObject.ApplicationId + "','" + rowObject.ApplicationName + "','" + rowObject.Company + "')\"  style='cursor:pointer;color:blue;'>" + cellvalue + "</span>";
                            } else {
                                return "无";
                            }
                        }
                    },
                    { label: '备注', name: 'Remark', width: 100, align: 'left' },
                    { label: '创建时间', name: 'CreateTime', width: 80, align: 'center' },
                ],
                pager: "#gridPager",
                sortname: 'CreateTime desc,ApplicationName asc',
                viewrecords: true
            });
            $("#btn_search").click(function () {
                $gridList.jqGrid('setGridParam', {
                    postData: { keywords: $("#txt_keyword").val() },
                }).trigger('reloadGrid');
            });
        }

        //新增
        function btn_add() {
            $.modalOpen({
                id: "Form",
                title: "新增应用",
                url: "Form.aspx",
                width: "500px",
                height: "400px",
                callBack: function (iframeId) {
                    top.frames[iframeId].submitForm();
                }
            });
        }


        //编辑
        function btn_edit() {
            var keyValue = $("#gridList").jqGridRowValue().ApplicationId;
            $.modalOpen({
                id: "Form",
                title: "修改应用",
                url: "Form.aspx?keyValue=" + keyValue,
                width: "500px",
                height: "400px",
                callBack: function (iframeId) {
                    top.frames[iframeId].submitForm();
                }
            });
        }

        //删除
        function btn_delete() {

            var applicationId = $("#gridList").jqGridRowValue().ApplicationId;
            if (!!applicationId) {
                $.deleteForm({
                    url: "../Handle/CSAppHandler.ashx?op=DeleteApp",
                    param: { ApplicationId: applicationId },
                    success: function () {
                        $("#gridList").trigger("reloadGrid");
                    }
                });
            } else {
                $.modalMsg("您没有选择任何行。", "warning");
            }
        }


        //明细
        function btn_details() {
            var keyValue = $("#gridList").jqGridRowValue().ApplicationId;

            if (!!keyValue) {
                $.modalOpen({
                    id: "Details",
                    title: "查看应用",
                    url: "Form.aspx?keyValue=" + keyValue,
                    width: "500px",
                    height: "400px",
                    btn: null,
                });
            } else {
                $.modalMsg("您没有选择任何行。", "warning");
            }
        }

        //上传更新包
        function btn_uploadVersion() {
            var keyValue = $("#gridList").jqGridRowValue().ApplicationId;
            var applicationName = $("#gridList").jqGridRowValue().ApplicationName;
            if (!!keyValue) {
                $.modalOpen({
                    id: "Details",
                    title: "上传更新包",
                    url: "VersionForm.aspx?applicationId=" + keyValue + "&applicationName=" + escape(applicationName),
                    width: "800px",
                    height: "600px",
                    btn: null,
                });
            } else {
                $.modalMsg("您没有选择任何行。", "warning");
            }
        }

        //历史记录
        function btn_versionHistory(keyValue, appName, company) {
            //console.log(rowObject);
            //var keyValue = rowObject.ApplicationId;
            //var appName = rowObject.ApplicationName;
            //var company = rowObject.Company;

            $.modalOpen({
                id: "Details",
                title: "【" + appName + "】更新包历史记录",
                url: "VersionHistory.aspx?ApplicationId=" + keyValue + "&AppName=" + escape(appName) + "&Company=" + escape(company),
                width: "1000px",
                height: "600px",
                btn: null,
            });
        }


        function btn_log() {
            $.ajax({
                url: "../Handle/CSAppHandler.ashx?op=GetCSAppLog",
                data: { ApplicationId: keyValue },
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#form1").formSerialize(data.data[0]);
                }
            });
        }
    </script>
    <div id="title">
        <span>桌面应用包版本管理</span><span style="float:right;margin-right:50px;"><a href="LogList.html" style="color:white;text-decoration-line:none;">日志</a></span>
    </div>
    <div class="topPanel">
        <div class="toolbar">
            <div class="btn-group">
                <a class="btn btn-primary" onclick="$.reload()"><span class="glyphicon glyphicon-refresh"></span></a>
            </div>
            <div class="btn-group">
                <a id="NF-add" authorize="yes" class="btn btn-primary dropdown-text" onclick="btn_add()"><i class="fa fa-plus"></i>新建</a>
            </div>
            <div class="operate">
                <ul class="nav nav-pills">
                    <li class="first">已选中<span>1</span>项</li>
                    <li><a id="NF-edit" onclick="btn_edit()"><i class="fa fa-pencil-square-o"></i>修改</a></li>
                    <li><a id="NF-delete" onclick="btn_delete()"><i class="fa fa-trash-o"></i>删除</a></li>
                    <li><a id="NF-Details" onclick="btn_details()"><i class="fa fa-search-plus"></i>查看</a></li>
                    <!--<li><a id="A1" onclick="btn_versionHistory()"><i class="fa fa-search-plus"></i>更新包</a></li>-->
                    <li><a id="NF-Upload" onclick="btn_uploadVersion()"><i class="fa fa-upload"></i>上传更新包</a></li>
                </ul>
                <a href="javascript:;" class="close"></a>
            </div>
            <script>$('.toolbar').authorizeButton()</script>
        </div>
        <div class="search">
            <table>
                <tr>
                    <td>
                        <div class="input-group">
                            <input id="txt_keyword" type="text" class="form-control" placeholder="名称/编码" style="width: 200px;" />
                            <span class="input-group-btn">
                                <button id="btn_search" type="button" class="btn  btn-primary"><i class="fa fa-search"></i></button>
                            </span>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="gridPanel">
        <table id="gridList"></table>
        <div id="gridPager"></div>
    </div>
</body>
</html>
