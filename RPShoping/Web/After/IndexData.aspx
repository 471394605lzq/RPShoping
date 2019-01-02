<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexData.aspx.cs" Inherits="RPShoping.Web.After.IndexData" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>后台首页</title>
    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../jquery/hb.js"></script>
    <style type="text/css">
        #total .t_num {
            display: inline-block;
            line-height: 13px;
            margin: 2px 4px 0 4px;
        }

            #total .t_num i {
                width: 15px;
                height: 23px;
                display: inline-block;
                background: url("../../Image/number.png") no-repeat;
            }
    </style>
    <script type="text/javascript">
        $(window).load(show_num(1568239));
        $(function () {
            //getdata();
            //setInterval('getdata()', 3000);
        });
        //function getdata() {
        //    var num = $("#cur_num").val();
        //    $.ajax({
        //        url: 'data.php',
        //        type: 'POST',
        //        dataType: "json",
        //        data: { 'total': num },
        //        cache: false,
        //        timeout: 10000,
        //        error: function () { },
        //        success: function (data) {
        //            show_num(data.count);
        //        }
        //    });
        //}

        function show_num(n) {
            var it = $(".t_num i");
            var len = String(n).length;
            for (var i = 0; i < len; i++) {
                if (it.length <= i) {
                    $(".t_num").append("<i></i>");
                }
                var num = String(n).charAt(i);
                var y = -parseInt(num) * 30;
                var obj = $(".t_num i").eq(i);
                obj.animate({
                    backgroundPosition: '(0 ' + String(y) + 'px)'
                }, 'slow', 'swing', function () { }
                );
            }
            //$("#cur_num").val(n);
        }
        function pushinfo() {
            $.ajax({
                type: "post",
                contentType: "application/json",
                url: '/Contract/AfterService.asmx/JGPushInfo',
                data: '{"content":"默认推送消息"}',
                dataType: 'json',
                success: function (result) {
                    var data = result.d;
                    if (data != "0" || data != "") {

                    }
                }
            });
        }
    </script>
</head>
<body>
    <div>
        后台首页数据加载列表
        <div id="total">
            下载量：<span class="t_num">
                <i></i>
                <i></i>
                <i></i>
                <i></i>
                <i></i>
                <i></i>
                <i></i>
            </span>次
        </div>
        <input type="button" value="变数字" onclick="show_num(1568239)" />
        <input type="button" value="推送消息" onclick="pushinfo()" />
    </div>
</body>
</html>
