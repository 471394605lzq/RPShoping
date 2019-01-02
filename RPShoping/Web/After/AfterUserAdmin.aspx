<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AfterUserAdmin.aspx.cs" Inherits="RPShoping.Web.After.AfterUserAdmin" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>后台用户管理</title>
    <link href="../../control/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../CSS/jquery-ui-1.9.2.custom.css" rel="stylesheet" />
    <link  href="../../CSS/ui.jqgrid.css" rel="stylesheet" />
    <link href="../../CSS/ui.jqgrid-bootstrap.css" rel="stylesheet" />
    <link href="../../control/layer/skin/layer.css" rel="stylesheet" />

    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../jquery/jquery.cookie.js"></script>
    <script src="../../jquery/jquery.query-2.1.7.js"></script>
    <script src="../../jquery/jquery.jqGrid.js"></script>
    <script src="../../jquery/grid.locale-cn.js"></script>
    <script src="../../jquery/json2.js"></script>
    <script src="../../control/layer/layer.js"></script>
    <script src="../../control/JS/JSCommonForAlter.js"></script>
    <script src="../../control/JS/JSCommon.js"></script>
    <script src="../../jquery/jquery-ui-1.9.2.custom.js"></script>
    <script src="../../jquery/JSAdvQuery.js"></script>
    <script src="../../control/JS/JSAfterUserAdmin.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 98%; height: 50px; border-bottom: 1px solid #e8e8e8; margin-bottom: 15px;">
            <div id="topmenu" style="width: auto; height: 30px; position: relative; top: 12px; left: 15px;">
            </div>
        </div>
        <table id="list5"></table>
        <div id="pager5"></div>
    </form>
</body>
</html>
