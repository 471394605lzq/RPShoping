$(document).ready(function () {
    $(".blacks").removeClass("black");
    $("#tw").addClass("black");
    $(".create_access").css("display", "block");
    $(".tex").css("display", "none");
    $(".editor_toolbar").css("display", "none");
    $("#fileImage").css("display", "none");
    $("#msgType").val("news");
});
$(function () {
    $("#texs").keyup(function () {
        if ($("#texs").val().length > 600) {
            $("#texs").val($("#texs").val().substring(0, 600));
        }
        $("#word").text(600 - $("#texs").val().length);
    });
    //$(document).on('click', ".add_gray_wrp", function (e) {
    //    e.preventDefault();
    //    $(this).next().click();
    //});
});
//用msgtype做为一个临时变量  然后获取隐藏控件的值  将值给msgtype 最后找到对应的名字（news，test，image等）
function setHideController(msgtype) {
    var ctl = document.getElementById("msgType");
    ctl.value = msgtype;

    switch (msgtype) {
        case "text":
            $(".blacks").removeClass("black");
            $("#wz").addClass("black");
            $(".create_access").css("display", "none");
            $(".tex").css("display", "block");
            $(".editor_toolbar").css("display", "block");
            $(".checkitem_content").css("display", "none");
            $("#deleteid").css("display", "none");
            $(".tab_content").css("display", "block");
            break;
        case "news":
            $(".blacks").removeClass("black");
            $("#tw").addClass("black");
            $(".create_access").css("display", "block");
            $(".tex").css("display", "none");
            $(".editor_toolbar").css("display", "none");
            $("#fileImage").css("display", "none");
            break;
        case "voice":
            $(".blacks").removeClass("black");
            $("#yy").addClass("black");
            $(".create_access").css("display", "block");
            $(".tex").css("display", "none");
            $(".editor_toolbar").css("display", "none");
            $("#fileImage").css("display", "none");
            $(".checkitem_content").css("display", "none");
            $("#deleteid").css("display", "none");
            $(".tab_content").css("display", "block");
            break;
        case "image":
            $(".blacks").removeClass("black");
            $("#tp").addClass("black");
            $(".create_access").css("display", "block");
            $(".tex").css("display", "none");
            $(".editor_toolbar").css("display", "none");
            $("#fileImage").css("display", "block");
            $("#inerstNews").css("display", "none");
            $(".checkitem_content").css("display", "none");
            $("#deleteid").css("display", "none");
            $(".tab_content").css("display", "block");
            break;
        case "video":
            $(".blacks").removeClass("black");
            $("#sp").addClass("black");
            $(".create_access").css("display", "block");
            $(".tex").css("display", "none");
            $(".editor_toolbar").css("display", "none");
            $("#fileImage").css("display", "none");
            $(".checkitem_content").css("display", "none");
            $("#deleteid").css("display", "none");
            $(".tab_content").css("display", "block");
            break;
        default:
            alert("未实现");
            break;
    }
};
$(function () {
    $(document.body).on("hover", ".item_content", function () {
        var children = $(this).children(".item_temp");
        children.css("display", "block");
        children.css("height", $(this).height());
    });
    $(document.body).on("mouseleave", ".item_content", function () {
        var erwei_time = null;
        var children = $(this).children(".item_temp");
        var id = children.attr("id");
        var itemid = $("#" + id).children("i").attr("id");
        erwei_time = setTimeout(function () {
            if (itemid != "check") {
                $('#' + id).css("display", "none");
            }
        }, 100);
    });
    $(document.body).on("click", ".item_temp", function () {
        var tempi = $(".item_temp").find("#check");
        var tempid = tempi.attr("id") + "id";
        if (tempid != "undefinedid") {
            var tempiparent = tempi.parent(".item_temp");
            var parentid = tempiparent.attr("id");
            $('#' + parentid).css("display", "none");
            tempi.css("color", "#999999");
            tempi.attr("id", "check" + tempiparent.attr("id"));
        }
        var id = $(this).attr("id");
        var item = $("#" + id).children("i");
        item.css("color", "green");
        item.attr("id", "check");

    });
    $('#selectnews').click(function () {
        
        var mestype = $("#msgType").val();
        switch (mestype) {
            case "news":
                $.ajax({
                    type: "post",
                    url: "/Home/GetMaterialData",
                    //contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $("#all").css("display", "block");
                        $("#selectmaterial").css("display", "block");
                        $("#selectmaterial").html(data.data);
                    }, error: function (error) {
                        alert("选择图文素材失败！");
                    }
                });
                break;
            case "voice":
                break;
            case "image":
                $.ajax({
                    type: "post",
                    url: "/Home/GetImagesMaterial",
                    data: { "msgtype": "image" },
                    //contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $("#all").css("display", "block");
                        $("#selectmaterial").css("display", "block");
                        $("#selectmaterial").html(data.data);
                    }, error: function (error) {
                        alert("选择图片素材失败！");
                    }
                });
                break;
            case "video":
                break;
            default:
                alert("未实现");
                break;
        }
    });
    $('#qx').click(function () {
        $("#all").css("display", "none");
    });
    $('#isok').click(function () {
        $(".tab_content").css("display", "none");
        var tempi = $(".item_temp").find("#check");
        var parent = tempi.parent(".item_temp");
        $("#all").css("display", "none");
        var parenttemp = parent.parent(".item_content");
        parent.css("display", "none");
        parenttemp.removeClass();
        parenttemp.addClass("checkitem_content");
        var tt = parenttemp.clone(true);
        $(".tab_panel").children(".checkitem_content").remove();
        $(".tab_panel").append(tt);
        $("#deleteid").css("display", "block");
        $("#media_id").val($(".hiddentemp").val());
    });
    $('#deleteid').click(function () {
        $(".checkitem_content").css("display", "none");
        $(".tab_content").css("display", "block");
        $("#deleteid").css("display", "none");
    });
});