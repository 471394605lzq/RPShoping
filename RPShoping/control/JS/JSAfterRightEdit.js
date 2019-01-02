var _id = $.query.get("id") || "";
var _m_id = $.query.get("m_id") || "";
var dbcmd = "";
if (_id == "") {
    dbcmd = "add";
    $("input[name='isdelete'][value='0']").attr("checked", true);
}
else {
    dbcmd = "edit";
    load();
}
//关闭页面
$("#cancel").click(function () {
    window.location.href = "AfterRight.aspx?m_id=" + _m_id;
});
//返回列表页面
$("#back").click(function () {
    window.location.href = "AfterRight.aspx?m_id=" + _m_id;
});
//保存信息
$("#save").click(function () {
    var righrcode = $("#righrcode").val();
    var rightname = $("#rightname").val();
    var remark = $("#remark").val();
    var right_imagecode = $("#right_imagecode").val();
    var isdelete = $('input[name="isdelete"]:checked').val();

    var fields = [];
    if (rightname != "" && rightname != null && rightname != undefined) {
        if (righrcode != "" && righrcode != null && righrcode != undefined) {
            fields.push({
                righrcode: righrcode, rightname: rightname, remark: remark, isdelete: isdelete, right_imagecode: right_imagecode
            });
            var jsonstr = $.toJSON(fields);
            $.ajax({
                type: "post",
                contentType: "application/json",
                url: "/Contract/AfterService.asmx/SaveAfterRight",
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
        } else {
            AlertInfo("请输入权限编号！");
            $("#righrcode").focus();
            return;
        }
    }
    else {
        AlertInfo("请输入权限名称！");
        $("#rightname").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#righrcode").val("");
    $("#rightname").val("");
    $("#remark").val("");
    $("#right_imagecode").val("");
    $("input[name=isdelete]:eq(0)").attr("checked", 'checked');
}
//编辑加载方法
function load() {
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterService.asmx/GetAfterUserRightList",
        data: '{"where":"' + " where right_id=" + _id + '"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d.split("***");
            if (data != "" && data != undefined && data != null) {
                var jsondata = $.evalJSON(data);
                var right_code = jsondata.rows[0].right_code;
                var rightname = jsondata.rows[0].right_name;
                var remark = jsondata.rows[0].right_remark;
                var isdelete = jsondata.rows[0].right_isdelete;
                var right_imagecode = jsondata.rows[0].right_imagecode;
                $("#righrcode").val(right_code);
                $("#rightname").val(rightname);
                $("#remark").val(remark);
                $("#right_imagecode").val(right_imagecode);
                $("input[name='isdelete'][value='" + isdelete + "']").attr("checked", true);
            }
        }
    });
}