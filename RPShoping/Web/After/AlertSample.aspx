<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlertSample.aspx.cs" Inherits="RPShoping.Web.After.AlertSample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>弹出层示例</title>
    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../control/JS/JSCommonForAlter.js"></script>
    <script src="../../control/layer/layer.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#testbtn").click(function () {
                AlertInfo("你不是傻逼，你是二笔");
                //layer.msg("你妹");
                //layer.open({
                //    type: 0, 
                //    title: false,
                //    content: '这个是提示内容，是这样的吗！？'
                //});

            });
            $("#testbtn2").click(function () {
                AlertConfirm("你是个傻逼吗！？", function (c) {
                    if (c == "1") {
                        Alertsuccess("你确实是一个傻逼");
                    }
                    else {
                        AlertText("你不是傻逼，你是二笔");
                        return;
                    }
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="button" value="提示" id="testbtn" />
            <input type="button" value="改变样式提示" id="testbtn2" />
        </div>
    </form>
</body>
</html>
