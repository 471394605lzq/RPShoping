var _id = $.query.get("id") || "";
var _m_id = $.query.get("m_id") || "";
var _appid = getappid();
var dbcmd = "";
if (_id == "") {
    dbcmd = "add";
}
else {
    dbcmd = "edit";
    load();
}
//关闭页面
$("#cancel").click(function () {
    window.location.href = "HotSearch.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "HotSearch.aspx?m_id=" + _m_id;
});
//保存
$("#save").click(function () {
    var saveappkey = getshaappkey();
    var keyword = $("#keyword").val();
    if (name != "" && name != null && name != undefined) {
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_HotSearch/" + _id,
            method: "POST",
            headers: {
                //"Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": saveappkey
            },
            "data": {
                "HS_Keyword": keyword,
                "_method": "PUT"
            },
            dataType: "json",
            success: function (data, status, header) {
                if (dbcmd == "add") {
                    empetclt();
                }
                Alertsuccess("保存成功");
            },
            error: function (data, status, header) {

            }
        });
    }
    else {
        AlertInfo("请输入关键词！");
        $("#keyword").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#keyword").val("");
}
//编辑加载方法
function load() {
    var loadppkey = getshaappkey();
    $.ajax({
        url: "https://d.apicloud.com/mcm/api/tb_HotSearch/" + _id,
        method: "get",
        headers: {
            "Content-type": "application/json",
            "X-APICloud-AppId": _appid,
            "X-APICloud-AppKey": loadppkey
        },
        //data:{"id":_id},
        dataType: "json",
        success: function (data, status, header) {
            $("#keyword").val(data.HS_Keyword);
        },
        error: function (data, status, header) {

        }
    });
}
