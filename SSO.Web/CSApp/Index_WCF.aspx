<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index_WCF.aspx.cs" Inherits="SSO.Web.CSApp.Index_WCF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>本地服务版本管理</title>
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
                url: "../Handle/CSAppHandler.ashx?op=GetWCFVersionList",
                height: $(window).height() - 178,
                colModel: [
                    { label: '主键', name: 'WCFVersionInfoId', hidden: true },
                    { label: '版本编号', name: 'VersionCode', width: 150, align: 'left' },
                    { label: '文件总大小', name: 'FileSize', width: 100, align: 'left' },
                    { label: '创建人', name: 'CreateUserId', width: 100, align: 'left' },
                    { label: '备注', name: 'Remark', width: 100, align: 'left' },
                    { label: '创建时间', name: 'CreateTime', width: 80, align: 'center' },
                ],
                pager: "#gridPager",
                sortname: 'VersionCode',
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
                title: "新增版本",
                url: "Form_WCF.aspx",
                width: "700px",
                height: "500px",
                btn: null,
                callBack: function (iframeId) {
                    top.frames[iframeId].submitForm();
                }
            });
        }

        //删除
        function btn_delete() {

            var wcfVersionInfoId = $("#gridList").jqGridRowValue().WCFVersionInfoId;

            if (!!wcfVersionInfoId) {
                $.deleteForm({
                    url: "../Handle/CSAppHandler.ashx?op=DeleteWCFVersion",
                    param: { WCFVersionInfoId: wcfVersionInfoId },
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
            var keyValue = $("#gridList").jqGridRowValue().WCFVersionInfoId;

            if (!!keyValue) {
                $.modalOpen({
                    id: "Details",
                    title: "版本详情",
                    url: "Details_WCF.aspx?WCFVersionInfoId=" + keyValue,
                    width: "500px",
                    height: "450px",
                    btn: null,
                });
            } else {
                $.modalMsg("您没有选择任何行。", "warning");
            }
        }

    </script>
    <div id="title">
        <span>本地服务版本管理</span>
    </div>
    <div class="topPanel">
        <div class="toolbar">
            <div class="btn-group">
                <a class="btn btn-primary" onclick="$.reload()"><span class="glyphicon glyphicon-refresh"></span></a>
            </div>
            <div class="btn-group">
                <a id="NF-add" class="btn btn-primary dropdown-text" onclick="btn_add()"><i class="fa fa-plus"></i>新增</a>
            </div>
            <div class="operate">
                <ul class="nav nav-pills">
                    <li class="first">已选中<span>1</span>项</li>
                    <%-- <li><a id="NF-edit" onclick="btn_edit()"><i class="fa fa-pencil-square-o"></i>修改</a></li>--%>
                    <li><a id="NF-delete" onclick="btn_delete()"><i class="fa fa-trash-o"></i>删除</a></li>
                    <li><a id="NF-Details" onclick="btn_details()"><i class="fa fa-search-plus"></i>查看</a></li>
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
                            <input id="txt_keyword" type="text" class="form-control" placeholder="版本编码、备注" style="width: 200px;" />
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
