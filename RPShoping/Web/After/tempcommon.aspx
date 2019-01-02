<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tempcommon.aspx.cs" Inherits="RPShoping.Web.After.tempcommon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>弹出层公用页面</title>
    <script type="text/javascript">
        //移动div方法开始
        var clicked = "Nope.";
        var mausx = "0";
        var mausy = "0";
        var winx = "0";
        var winy = "0";
        var difx = mausx - winx;
        var dify = mausy - winy;
        $("html").mousemove(function (event) {
            mausx = event.pageX;
            mausy = event.pageY;
            winx = $("#showdiv").offset().left;
            winy = $("#showdiv").offset().top;
            if (clicked == "Nope.") {
                difx = mausx - winx;
                dify = mausy - winy;
            }
            var newx = event.pageX - difx - $("#showdiv").css("marginLeft").replace('px', '');
            var newy = event.pageY - dify - $("#showdiv").css("marginTop").replace('px', '');
            $("#showdiv").css({ top: newy, left: newx });
            //$(".container").html("Mouse Cords: " + mausx + " , " + mausy + "<br />" + "Window Cords:" + winx + " , " + winy + "<br />Draggin'?: " + clicked + "<br />Difference: " + difx + " , " + dify + "");
        });
        $("#showdiv").mousedown(function (event) {
            clicked = "Yeah.";
        });
        $("html").mouseup(function (event) {

            clicked = "Nope.";
        });
        //移动div方法结束


        $("#addmodule").click(function () {
            $("#showdiv").css("display", "block");
        });
        $("#closediv").click(function () {
            $("#showdiv").css("display", "none");
        });
        $("#showdivclosebtn").click(function () {
            $("#showdiv").css("display", "none");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="showdiv" class="showdiv">
                <div class="showdiv_topall">
                    <span class="showdiv_toptitle">新增模块</span>
                    <i id="closediv" class="fa fa-times" style=" float: right;color: red;cursor: pointer;font-size: 18px;margin-right: 15px;height: 50px;line-height: 50px;" aria-hidden="true"></i>
                </div>
                <div class="showul_div">
                    <ul>
                        <li>
                            <span class="s_left">模块key：</span>
                            <span class="s_right">
                                <input type="text" id="module_key" class="input_text" /><span class="showdivrequiredstr">*</span>
                            </span>
                            <span class="s_left">模块名称：</span>
                            <span  class="s_right">
                                <input type="text" id="module_name" class="input_text" /><span class="showdivrequiredstr">*</span>
                            </span>
                        </li>
                        <li>
                            <span class="s_left">模块编号：</span>
                            <span class="s_right">
                                <input type="text" id="module_code" class="input_text" /><span class="showdivrequiredstr">*</span>
                            </span>
                            <span class="s_left">是否显示：</span>
                            <span class="s_right">
                                <input id="yes" type="radio" checked="checked" name="showstate" value="0" class="input_radio" /><label class="radio_lable" for="mb_female">否</label>
                                <input id="no" type="radio" name="showstate" value="1"  class="input_radio radio_radio_right" /><label class="radio_lable" for="mb_male">是</label>
                                <span class="showdivrequiredstr">*</span>
                            </span>
                        </li>
                        <li class="bottom_line">
                            <span class="s_left" style="text-align:left;">备注说明</span>
                        </li>
                        <li>
                                <textarea class="input_area" id="module_remark"></textarea>
                        </li>
                    </ul>
                </div>
                <div class="showdov_bottom">
                    <div class="showdiv_bottom_right">
                        <input id="showdivclosebtn" type="button" class="input_botton" value="关 闭" />
                        <input id="savemodule" type="button" class="input_botton" style="margin-left:30px;" value="保 存" />
                    </div>
                </div>
            </div>
    </form>
</body>
</html>
