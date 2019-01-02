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
    window.location.href = "PaymentWay.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "PaymentWay.aspx?m_id=" + _m_id;
});
//保存
$("#save").click(function () {
    var saveappkey = getshaappkey();
    var name = $("#name").val();
    var state = $('input[name="state"]:checked').val();
    var isdefault = $('input[name="isdefault"]:checked').val();
    if (name != "" && name != null && name != undefined) {
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_PaymentWay/" + _id,
            method: "POST",
            headers: {
                //"Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": saveappkey
            },
            "data": {
                "PA_Name": name,
                "state": state,
                "isdefault":isdefault,
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
        AlertInfo("请输入支付名称！");
        $("#name").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#name").val("");
    $("input[name=state]:eq('启用')").attr("checked", 'checked');
    $("input[name=isdefault]:eq('否')").attr("checked", 'checked');   
}
//编辑加载方法
function load() {
    var loadppkey = getshaappkey();
    $.ajax({
        url: "https://d.apicloud.com/mcm/api/tb_PaymentWay/" + _id,
        method: "get",
        headers: {
            "Content-type": "application/json",
            "X-APICloud-AppId": _appid,
            "X-APICloud-AppKey": loadppkey
        },
        //data:{"id":_id},
        dataType: "json",
        success: function (data, status, header) {
            $("#name").val(data.PA_Name);
            $("input[name='state'][value='" + data.state + "']").attr("checked", true);
            $("input[name='isdefault'][value='" + data.isdefault + "']").attr("checked", true);
        },
        error: function (data, status, header) {

        }
    });
}
