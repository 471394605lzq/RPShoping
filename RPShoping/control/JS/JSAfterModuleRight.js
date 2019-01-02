load();
var _mid = 0;
function load() {
    loadmainmodule(function (c) {
        if (c == "1") {
            loadchildmodule(function (c) {
                if (c == "1") {
                    
                }
            });
        }
    });

}
//获取主菜单模块
function loadmainmodule(callback) {
    $("#modulediv").empty();
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: '/Contract/AfterService.asmx/GetAfterModuleList',
        data: '{"where":" where isparement=1 "}',
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            var liid = "";
            var lihtml = "";
            var litext = "";
            if (data != "0" || data != "") {
                var s = eval("(" + data + ")");
                var list = s.rows;
                for (i = 0; i < list.length; i++) {
                    liid = list[i].M_ID;
                    litext = list[i].M_Name;
                    if (liid != "" && liid != undefined && liid != null) {
                        if (i == 0) {
                            lihtml = lihtml + "<div id='" + liid + "' class='leftmodulediv addcolor2'>" + litext + "<i class='fa fa-check displaynone' aria-hidden='true' style='color: #0efa45;'></i></div>";
                        }
                        else {
                            lihtml = lihtml + "<div id='" + liid + "' class='leftmodulediv'>" + litext + "<i class='fa fa-check displaynone' aria-hidden='true' style='color: #0efa45;'></i></div>";
                        }
                    }
                }
                $("#modulediv").append(lihtml);
                callback("1");
            }
        }
    });
}
//获取子级菜单模块
function loadchildmodule(callback)
{
    $("#childmoduleul").empty();
    var mainmoduleid = $(".addcolor2").attr("id");
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: '/Contract/AfterService.asmx/GetAfterModuleList',
        data: '{"where":" where parement_id=' + mainmoduleid + '"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            var liid = "";
            var lihtml = "";
            var litext = "";
            if (data != "0" || data != "") {
                var s = eval("(" + data + ")");
                var list = s.rows;
                for (i = 0; i < list.length; i++) {
                    liid = list[i].M_ID;
                    litext = list[i].M_Name;
                    if (liid != "" && liid != undefined && liid != null) {
                        lihtml = lihtml + "<li id='" + liid + "' class='module_li'>" + litext +
                            "<i class='fa fa-check displaynone' aria-hidden='true' style='color: #0efa45;'></i>"+
                            "<div class='tempshowdiv' style='width:100%; height:20px;bottom:0px; background:#ffffff; opacity:0.5;border-radius:2px; color:#6C77F7; line-height:20px; font-size:12px;'>功能详情</div> </li>";
                    }
                }
                $("#childmoduleul").append(lihtml);
                callback("1");
            }
        }
    });
}

//获取子级菜单模块权限功能
function loadchildmoduleright(callback) {
    $("#showdiv").empty();
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: '/Contract/AfterService.asmx/GetAfterModuleRightList',
        data: '{"where":" where right_isdelete<>1"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            var liid = "";
            var lihtml = "";
            var litext = "";
            if (data != "0" || data != "") {
                var s = eval("(" + data + ")");
                var list = s.rows;
                for (i = 0; i < list.length; i++) {
                    liid = list[i].right_id;
                    litext = list[i].right_name;
                    if (liid != "" && liid != undefined && liid != null) {
                        lihtml = lihtml + "<div class='moduleright showtemp' id=" + liid + ">" +
                        "<i class='fa fa-check displaynone' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>" + litext + "</div>"
                    }
                }
                $("#showdiv").append(lihtml);
                callback("1");
            }
        }
    });
}

//获取模块权限
function getrightbymodule(moduleid) {
    var isparent = "0";
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: '/Contract/AfterService.asmx/GetAfterModuleRight',
        data: '{"where":" where module_id=' + moduleid+ ' "}',
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            var liid = "";
            if (data != "0" || data != "") {
                var s = eval("(" + data + ")");
                var list = s.rows;
                $("#showdiv .moduleright").each(function () {
                    var thisid = $(this).attr("id");
                    
                    for (i = 0; i < list.length; i++) {
                        liid = list[i].right_id;
                        if (liid != "" && liid != undefined && liid != null && thisid == liid) {
                            $(this).children("i").removeClass("displaynone");
                            $(this).children("i").addClass("displayblock");
                        }
                    }
                });
            }
        }
    });
}



//选择主菜单事件
$(document.body).on("click", "#modulediv .leftmodulediv", function () {
    var thisid = $(this).attr("id");
    $("#modulediv .leftmodulediv").each(function () {
        if (thisid == $(this).attr("id")) {
            $(this).addClass("addcolor2");
            loadchildmodule(function (c) {
                if (c == "1") {
                    var childmoduleid = $(".addcolor2").attr("id");
                    var roleid = $(".addcolor").attr("id");
                }
            });
            //loadchildmodule(thisid);
        }
        else {
            $(this).removeClass("addcolor2");
        }
    });
});
//选择子菜单权限事件
$(document.body).on("click", "#showdiv .moduleright", function () {
    var displayval = $(this).children("i").css('display');
    var right_id = $(this).attr("id");
    if (displayval == "none") {
        $(this).children("i").removeClass("displaynone");
        $(this).children("i").addClass("displayblock");
        savemodulerightupdate(right_id, _mid, "0");
    }
    else {
        $(this).children("i").removeClass("displayblock");
        $(this).children("i").addClass("displaynone");
        savemodulerightupdate(right_id, _mid, "1");
    }
});
//当鼠标移动到子菜单下方的div时就显示当前子菜单的权限功能
$(document.body).on("mouseover", "#childmoduleul .module_li .tempshowdiv", function () {
    var offset = $(this).offset();
    var thisheight = offset.top + $(this).height();
    var mid = $(this).parent("li").attr("id");
    //loadchildmoduleright();
    loadchildmoduleright(function (c) {
        if (c == "1") {
            getrightbymodule(mid);
            
            _mid = mid;
            $("#showdiv").css("position", "absolute");
            $("#showdiv").css("left", offset.left + "px");
            $("#showdiv").css("top", thisheight + "px");
            $("#showdiv").css("display", "block");
            $("#showdiv").show();
        }
    });

});

//隐藏模块权限
$(document.body).on("mouseout", "#showdiv", function (event) {
    var x=event.clientX; 
    var y = event.clientY;
    var t=$("#showdiv").offset();
    var divx1 = t.left;
    var divy1 = t.top;
    var divx2 = t.left + $("#showdiv")[0].offsetWidth;
    var divy2 = t.top + $("#showdiv")[0].offsetHeight;
    if (x < divx1 || x > divx2 || y < divy1 || y > divy2) {

        $("#showdiv").hide();
    }
});
//保存模块权限修改方法type为0时是新增 否则就是删除
function savemodulerightupdate(right_id, moduleid, type) {
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterService.asmx/SaveModuleRight",
        data: '{"rightid":"' + right_id + '","moduleid":"' + moduleid + '","type":"' + type + '"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            if (parseInt(data) > 0) {
                Alertsuccess("操作成功");
            }
            else {
                AlertError("操作失败" + data);
            }
        }
    });
}
