var _id = $.query.get("id") || "";
var _m_id = $.query.get("m_id") || "";
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
    window.location.href = "Role.aspx?m_id=" + _m_id;
});
//返回列表页面
$("#back").click(function () {
    window.location.href = "Role.aspx?m_id=" + _m_id;
});
//保存信息
$("#save").click(function () {
    var rolename = $("#rolename").val();
    var rolekey = $("#rolekey").val();
    var remark = $("#remark").val();
    var fields = [];
    if (rolename != "" && rolename != null && rolename != undefined) {
        fields.push({
            rolename: rolename, rolekey: rolekey, remark: remark
        });
        var jsonstr = $.toJSON(fields);
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/Contract/AfterService.asmx/SaveAfterRole",
            data: '{"context":"' + encodeURI(jsonstr) + '","dbcmd":"' + dbcmd + '","id":"' + _id + '"}',
            dataType: 'json',
            success: function (result) {
                var data = result.d;
                if (parseInt(data) > 0) {
                    Alertsuccess("保存成功");
                    if (dbcmd == "add") {
                        empetclt();
                    }
                }
                else {
                    AlertError("保存失败" + data);
                }
            }
        });
    }
    else {
        AlertInfo("请输入角色名称！");
        $("#rolename").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#rolename").val("");
    $("#rolekey").val("");
    $("#remark").val("");
}
//编辑加载方法
function load() {
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterService.asmx/GetAfterUserRoleList",
        data: '{"where":"' + " where r_id=" + _id + '"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d.split("***");
            if (data != "" && data != undefined && data != null) {
                var jsondata = $.evalJSON(data);
                var rolename = jsondata.rows[0].R_Name;
                var rolekey = jsondata.rows[0].R_Key;
                var remark = jsondata.rows[0].R_Remark;
                $("#rolename").val(rolename);
                $("#rolekey").val(rolekey);
                $("#remark").val(remark);
            }
        }
    });
}