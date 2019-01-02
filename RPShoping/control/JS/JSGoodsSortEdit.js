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
    window.location.href = "GoodsSort.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "GoodsSort.aspx?m_id=" + _m_id;
});
//保存
$("#save").click(function () {
    var saveappkey = getshaappkey();
    var sortname = $("#sortname").val();
    var icon = $("#icon").val();
    var remark = $("#remark").val();
    var isdelete = $('input[name="isdelete"]:checked').val();
    if (sortname != "" && sortname != null && sortname != undefined) {
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_ProductSort/" + _id,
            method: "POST",
            headers: {
                //"Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": saveappkey
            },
            "data": {
                "PS_Name": sortname,
                "PS_icon": icon,
                "PS_Remark": remark,
                "isdelete":isdelete,
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
        AlertInfo("请输入分类名称！");
        $("#sortname").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#sortname").val("");
    $("#icon").val("");
    $("#remark").val("");
    $("input[name=isdelete]:eq('否')").attr("checked", 'checked');
}
//编辑加载方法
function load() {
    var loadppkey = getshaappkey();
    $.ajax({
        url: "https://d.apicloud.com/mcm/api/tb_ProductSort/"+_id,
        method: "get",
        headers: {
            "Content-type": "application/json",
            "X-APICloud-AppId": _appid,
            "X-APICloud-AppKey": loadppkey
        },
        //data:{"id":_id},
        dataType: "json",
        success: function (data, status, header) {
            $("#sortname").val(data.PS_Name);
            $("#icon").val(data.PS_icon);
            $("#remark").val(data.PS_Remark);
            $("input[name='isdelete'][value='" + data.isdelete + "']").attr("checked", true);
        },
        error: function (data, status, header) {

        }
    });
}
