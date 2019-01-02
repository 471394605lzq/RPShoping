var _id = $.query.get("id") || "";
var _parentid = $.query.get("parentid") || "0";
var _m_id = $.query.get("m_id") || "";
if (parseInt(_parentid) > 0) {
    $("#parement_id").val(_parentid);
    $("input[name='isparemnt'][value='0']").attr("checked", true);
    $("input[name=isparemnt]").attr("disabled", true);
}
else {
    $("input[name='isparemnt'][value='1']").attr("checked", true);
    $("input[name=isparemnt]").attr("disabled", true);
}

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
    window.location.href = "AfterModule.aspx?m_id=" + _m_id;
});
//返回列表页面
$("#back").click(function () {
    window.location.href = "AfterModule.aspx?m_id=" + _m_id;
});
//保存信息
$("#save").click(function () {
    var module_key = $("#module_key").val();
    var module_name = $("#module_name").val();
    var module_code = $("#module_code").val();
    var showstate = $('input[name="showstate"]:checked').val();
    var module_remark = $("#module_remark").val();
    var parement_id = $("#parement_id").val();
    var editurl = $("#m_editurl").val();
    var isparemnt = $('input[name="isparemnt"]:checked').val();
    var imagecode = $("#imagecode").val();
    var reorder = $("#reorder").val();
    if (reorder == "" || reorder == null || reorder == undefined)
    {
        reorder = "0";
    }

    var fields = [];
    if (module_key != "" && module_key != null && module_key != undefined) {
        if (module_name != "" && module_name != null && module_name != undefined) {
            if (module_code != "" && module_code != null && module_code != undefined) {
                fields.push({
                    module_key: module_key, module_name: module_name, module_code: module_code, showstate: showstate, module_remark: module_remark,
                    parement_id: parement_id, editurl: editurl, isparemnt: isparemnt, imagecode: imagecode, reorder: reorder
                });
                var jsonstr = $.toJSON(fields);
                $.ajax({
                    type: "post",
                    contentType: "application/json",
                    url: "/Contract/AfterService.asmx/SaveAfterModule",
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
                AlertInfo("请输入模块编号！");
                $("#module_code").focus();
                return;
            }
        } else {
            AlertInfo("请输入模块名称！");
            $("#module_name").focus();
            return;
        }
    }
    else {
        AlertInfo("请输入模块key！");
        $("#module_key").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#module_key").val("");
    $("#module_name").val("");
    $("#module_code").val("");
    $("#module_remark").val("");
    $("#m_editurl").val("");
    $("#imagecode").val("");
    $("#reorder").val("");
    if (_parentid <= 0) {
        $("#parement_id").val("");
    }
    $("input[name=showstate]:eq(0)").attr("checked", 'checked');
    $("input[name=isparemnt]:eq(0)").attr("checked", 'checked');
}
//编辑加载方法
function load() {
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterService.asmx/GetAfterModuleList",
        data: '{"where":"' + " where M_ID=" + _id + '"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d.split("***");
            if (data != "" && data != undefined && data != null) {
                var jsondata = $.evalJSON(data);
                var module_key = jsondata.rows[0].M_ModuleKey;
                var module_name = jsondata.rows[0].M_Name;
                var module_code = jsondata.rows[0].M_Level_No;
                var module_remark = jsondata.rows[0].M_Remark;
                var m_showstate = jsondata.rows[0].M_ShowState;
                var reorder = jsondata.rows[0].reorder;
                if (m_showstate == "" || m_showstate == null || m_showstate == undefined) {
                    m_showstate = "0";
                }
                var isparemnt = jsondata.rows[0].isparement;
                if (isparemnt == "" || isparemnt == null || isparemnt == undefined)
                {
                    isparemnt = "0";
                }
                var editurl = jsondata.rows[0].M_EditUrl;
                var parement_id = jsondata.rows[0].parement_id;
                var imagecode = jsondata.rows[0].imagecode;

                $("#module_key").val(module_key);
                $("#module_name").val(module_name);
                $("#module_code").val(module_code);
                $("#module_remark").val(module_remark);
                $("#m_editurl").val(editurl);
                $("#parement_id").val(parement_id);
                $("input[name='radio_name'][value='" + m_showstate + "']").attr("checked", true);
                $("input[name='isparemnt'][value='" + isparemnt + "']").attr("checked", true);
                $("#imagecode").val(imagecode);
                $("#reorder").val(reorder);
            }
        }
    });
}