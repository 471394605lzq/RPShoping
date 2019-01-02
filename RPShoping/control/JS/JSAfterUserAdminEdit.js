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
    window.location.href = "AfterUserAdmin.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "AfterUserAdmin.aspx?m_id=" + _m_id;
});
//保存账号信息
$("#save").click(function () {
    var account_name = $("#accountname").val();
    var user_name = $("#username").val();
    var pwd = $("#accountpwd").val();
    var roe_id = $("#rolese").children('option:selected').val();
    var remark = $("#remark").val();
    var fields = [];
    if (account_name != "" && account_name != null && account_name != undefined) {
        if (user_name != "" && user_name != null && user_name != undefined) {
            if (pwd != "" && pwd != null && pwd != undefined) {
                fields.push({
                    account_name: account_name, user_name: user_name, pwd: pwd, roe_id: roe_id, remark: remark
                });
                var jsonstr = $.toJSON(fields);
                $.ajax({
                    type: "post",
                    contentType: "application/json",
                    url: "/Contract/AfterService.asmx/SaveAfterAccount",
                    data: '{"context":"' + encodeURI(jsonstr) + '","dbcmd":"' + dbcmd + '","au_id":"' + _id + '"}',
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
                AlertInfo("请输入密码！");
                $("#accountpwd").focus();
                return;
            }
        }
        else {
            AlertInfo("请输入用户姓名！");
            $("#username").focus();
            return;
        }
    }
    else {
        AlertInfo("请输入账号！");
        $("#accountname").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#accountname").val("");
    $("#username").val("");
    $("#accountpwd").val("");
    $("#remark").val("");
    $("#rolese option[value='0']").attr("selected", "selected");
}
//编辑加载方法
function load() {
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterService.asmx/GetAfterUserList",
        data: '{"where":"' + " and au_id=" + _id + '"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d.split("***");
            if (data != "" && data != undefined && data != null) {
                var jsondata = $.evalJSON(data);
                var account = jsondata.rows[0].AU_UserAccount;
                var user_name = jsondata.rows[0].AU_Name;
                var remark = jsondata.rows[0].AU_Remark;
                var rid = jsondata.rows[0].R_ID;
                var pwd = jsondata.rows[0].AU_Password;
                $("#accountname").val(account);
                $("#username").val(user_name);
                $("#accountpwd").val(pwd);
                $("#remark").val(remark);
                $("#rolese option[value='" + rid + "']").attr("selected", "selected");
            }
        }
    });
}