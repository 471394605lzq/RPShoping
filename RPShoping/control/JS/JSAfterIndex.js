$(function () {
var userid = $.cookie("au_id");
var username = $.cookie("au_name");
var role_id = $.cookie("r_id");
load();
function load() {
    getMainMenu(function (c) {
        if (c == "1") {
            getchildmenu();
        }
    });

}

    
    $("#user_name").html(username);
    //计算框架div宽度开始
    var leftwidth = $("#leftmenu").width();
    var allwidth = $(".maxter_topdiv").width();
    $("#right_content").width(parseInt(allwidth - leftwidth) - 20);
//计算框架div宽度结束

        //$('span').click(function () {
    $(document.body).on("click", ".mainmenu", function () {
        //$('span').removeClass('on');
            $(this).removeClass('on');
            var $ul = $(this).next('ul');
            var $em = $(this).children('em');
            $('#sideMenu').find('em').removeClass('fa fa-angle-down')
            $('#sideMenu').find('em').addClass('fa fa-angle-right')
            if ($ul.is(':visible')) {
                $('#sideMenu').find('ul').slideUp();
                $(this).next('ul').slideUp();
                $(this).removeClass('on');
                $em.removeClass('fa fa-angle-down');
                $em.addClass('fa fa-angle-right');


            } else {
                $('#sideMenu').find('ul').slideUp();
                $(this).addClass('on');
                $(this).next('ul').slideDown();
                $em.removeClass('fa fa-angle-right');
                $em.addClass('fa fa-angle-down');

            }

        });
    $(document.body).on("click", "#sideMenu>ul>li", function () {
        $('#sideMenu>ul>li').removeClass('on');
        $(this).addClass('on');
    });
    //点击切换框架集iframe链接内容
    //$(".linka").click(function () {
    $(document.body).on("click", ".linka", function () {
        var name = $(this).attr("href");
        $("#iframe").attr("src", name);
        var tempourl = $("#iframe").attr("src");
    });



    //退出
    $("#return").click(function () {
        window.location.href = "/web/after/Login.aspx";
    });
    //修改密码
    $("#updatepwd").click(function () {
        $("#showdiv").css("display", "block");
    });
    //保存修改密码方法
    $("#savemodule").click(function () {
        var oldpwd = $("#oldpwd").val();
        var newpwd = $("#newpwd").val();
        if (oldpwd != "" && oldpwd != undefined && oldpwd != null) {
            if (newpwd != "" && newpwd != undefined && newpwd != null) {
                $.ajax({
                    type: "post",
                    contentType: "application/json",
                    url: "/Contract/AfterService.asmx/UpdateUserPwd",
                    data: '{"oldpwd":"' + oldpwd + '","newpwd":"' + newpwd + '","userid":"' + userid + '"}',
                    dataType: 'json',
                    success: function (result) {
                        var data = result.d;
                        if (parseInt(data) > 0) {
                            $("#oldpwd").val("");
                            $("#newpwd").val("");
                            $("#showdiv").css("display", "none");
                            Alertsuccess("操作成功");
                        }
                        else {
                            $("#oldpwd").val("");
                            $("#newpwd").val("");
                            $("#oldpwd").focus();
                            AlertError("操作失败" + data);
                        }
                    }
                });
            }
            else {
                AlertInfo("请输入新密码");
                $("#newpwd").focus();
                return;
            }
        }
        else {
            AlertInfo("请输入旧密码");
            $("#oldpwd").focus();
            return;
        }
    });
    //关闭修改密码弹出层
    $("#closediv").click(function () {
        $("#oldpwd").val("");
        $("#newpwd").val("");
        $("#showdiv").css("display", "none");
    });
    //关闭修改密码弹出层
    $("#showdivclosebtn").click(function () {
        $("#oldpwd").val("");
        $("#newpwd").val("");
        $("#showdiv").css("display", "none");
    });




    //浏览器宽度改变时控制iframe框架的宽度
    var $wind = $(window);//将浏览器加入缓存中
    var $do = $('.maxter_topdiv');//将你要改变宽度的div元素加入缓存中
    var win = $wind.outerWidth()//首先获取浏览器的宽度
    $wind.resize(function () {
        //浏览器变化宽度的动作。
        var newW = $wind.outerWidth();
        $do.width(Math.abs(win - (win - newW)));
        var leftwidth = $("#leftmenu").width();
        var allwidth = $(".maxter_topdiv").width();
        $("#right_content").width(parseInt(allwidth - leftwidth) - 20);
    });

//获取主菜单
    function getMainMenu(callback) {
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: '/Contract/AfterService.asmx/GetAfterModuleMenu',
            data: '{"role_id":"' + role_id + '","isparement":"1"}',
            dataType: 'json',
            success: function (result) {
                var data = result.d;
                var liid = "";
                var litext = "";
                var mainmenutophtml = "";
                //var tempchildmenuhtml = "";
                //var tempresulthtml = "";
                var tophtml=" <a class='linka' href='IndexData.aspx' style='text-decoration:none;' target='i'><span style='background-color: #FFFFFF; color: #0066CC'><i class='fa fa-home' style='color: #0066CC;'></i>首页<em style='color: #0066CC;' class='fa fa-tag'></em></span></a>"
                if (data != "0" || data != "") {
                    var s = eval("(" + data + ")");
                    var list = s.rows;
                    for (i = 0; i < list.length; i++) {
                        liid = list[i].M_ID;
                        litext = list[i].M_Name;
                        mainmenutophtml = mainmenutophtml + "<span  class='mainmenu'><i class='fa " + list[i].imagecode + "'></i>" + litext + "<em class='fa fa-angle-right'></em></span><ul id='mainmenu_" + list[i].M_ID + "'></ul>";
                    }
                    $("#sideMenu").append(tophtml + mainmenutophtml);
                    callback("1");
                }
            }
        });
    }
//获取子菜单
    function getchildmenu() {
        var tempchildmenuhtml = "";
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: '/Contract/AfterService.asmx/GetAfterModuleMenu',
            data: '{"role_id":"' + role_id + '","isparement":"0"}',
            dataType: 'json',
            success: function (result) {
                var data2 = result.d;
                if (data2 != "0" || data2 != "") {
                    var childmenu = eval("(" + data2 + ")");
                    var childlist = childmenu.rows;
                    $("#sideMenu ul").each(function () {
                        var thisid = $(this).attr("id");
                        var arr = thisid.split('_');
                        tempchildmenuhtml = "";
                        for (j = 0; j < childlist.length; j++) {
                            if (parseInt(childlist[j].parement_id) ==parseInt(arr[1])) {
                                tempchildmenuhtml = tempchildmenuhtml + "<li><a class='linka' href='" + childlist[j].M_EditUrl + "?m_id=" + childlist[j].M_ID + "' target='i'>" + childlist[j].M_Name + "</a></li>";
                            }
                        }
                        $(this).append(tempchildmenuhtml);
                    });
                }
            }
        });
    }

});