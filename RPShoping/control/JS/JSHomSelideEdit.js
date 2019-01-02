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
    window.location.href = "HomSelide.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "HomSelide.aspx?m_id=" + _m_id;
});
//保存
$("#save").click(function () {
    var saveappkey = getshaappkey();
    var title = $("#title").val();
    var number = $("#number").val();
    var linkcontent = $("#linkcontent").val();
    var remark = $("#remark").val();
    var selideimg = $("#selideimg").attr('src');
    var linktype = $("#linktype").children('option:selected').val();
    var linkparm = $("#linkparm").val();

    var validateval = validate();//保存验证
    if (validateval) {
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_HomSelide/" + _id,
            method: "POST",
            headers: {
                //"Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": saveappkey
            },
            "data": {
                "HS_Title": title,
                "HS_SerialNumber": number,
                "HS_LinkContent": linkcontent,
                "remark": remark,
                "HS_Image": selideimg,
                "type": linktype,
                "parameter": linkparm,
                "_method": "PUT"
            },
            dataType: "json",
            success: function (data, status, header) {
                console.log(data);
                if (dbcmd == "add") {
                    empetclt();
                }
                Alertsuccess("保存成功");
            },
            error: function (data, status, header) {

            }
        });
    }
});
//验证
function validate() {
    var returnresult = true;
    var linkcontent = $("#linkcontent").val();
    var linktype = $("#linktype").children('option:selected').val();
    var linkparm = $("#linkparm").val();

    if (linkcontent == "" || linkcontent == null || linkcontent == undefined) {
        returnresult = false;
        AlertInfo("请输入链接内容");
        $("#linkcontent").focus();
        return;
    }
    if (linktype == "" || linktype == null || linktype == undefined || linktype == "-1") {
        returnresult = false;
        AlertInfo("请选择链接跳转类型！");
        $("#linktype").focus();
        return;
    }
    else if (linktype == "1") {
        if (linkparm == "" || linkparm == null || linkparm == undefined) {
            returnresult = false;
            AlertInfo("请输入链接参数");
            $("#linkparm").focus();
            return;
        }
    }
    return returnresult;
}

//清空控件值
function empetclt() {
    $("#title").val("");
    $("#number").val("");
    $("#linkcontent").val("");
    $("#remark").val("");
    $("#selideimg").attr("src", "../../Image/noimg.png");
    $("#imgkey").val("");
    $("#linktype option[value='0']").attr("selected", "selected");
    $("#linkparm").val("");
}
//编辑加载方法
function load() {
    var loadppkey = getshaappkey();
    $.ajax({
        url: "https://d.apicloud.com/mcm/api/tb_HomSelide/" + _id,
        method: "get",
        headers: {
            "Content-type": "application/json",
            "X-APICloud-AppId": _appid,
            "X-APICloud-AppKey": loadppkey
        },
        //data:{"id":_id},
        dataType: "json",
        success: function (data, status, header) {
            $("#title").val(data.HS_Title);
            $("#number").val(data.HS_SerialNumber);
            $("#linkcontent").val(data.HS_LinkContent);
            $("#remark").val(data.remark);
            $("#selideimg").attr("src", data.HS_Image);
            $("#linkparm").val(data.parameter);
            $("#linktype option[value='" + data.type + "']").attr("selected", "selected");
            var imgs = data.HS_Image;
            var temp = imgs.split("/");
            var keyval = temp[3];
            $("#imgkey").val(keyval);
        },
        error: function (data, status, header) {

        }
    });
}



var options_logo = {
    limitFilesCount: 1,
    qiniu: {
        uptoken_url: '/Contract/AfterCommonService.asmx/SetQiNiuToken',
        domain: 'http://qiniu.rpshoping.com/',
        flash_swf_url: '../../control/uploader/Moxie.swf',
        max_file_size: '3mb',
        max_retries: 3,
        multi_selection: true,
        key: function (up, file) {
            //file.id
            return new Date().format("yyyyMMddmmss") + file.name;
        },
    },
    filters: {
        prevent_duplicates: true,
        mime_types: [{ title: "Image files", extensions: "jpg,gif,png" }]
    },

    deleteActionOnDone: function (file, doRemoveFile) {
        //$.ajax({
        //    url: routes.prvt.file.deleteCommonFile + "?key=" + file.remoteData.key,
        //    type: "GET",
        //    success: function (data) {
        //        if (data.success) {
        //            $form.find("[name='logo']").val("");
        //            xbky.toastr.success("删除成功");
        //        } else {
        //            xbky.toastr.error(data.message);
        //        }
        //    }
        //});
        return true;
    },
    onFileUploaded: function (file) {
        //console.log(file);
        var key = file.remoteData.key;
        var url = file.url;
        //var id = file.id;
        $("#imgkey").val(key);
        $("#selideimg").attr("src", url);
        //$form.find("[name='logo']").val(file.remoteData.key);
        //var obj = {};
        //obj['name'] = file['name'];
        //obj['type'] = file['type'];
        //obj['ext'] = file['ext'];
        //obj['origSize'] = file['origSize'];
        //obj['previewImage'] = file['previewImage'];
        //obj['isImage'] = file['isImage'];
        //obj['size'] = file['remoteData']['fsize'];
        //obj['bucket'] = file['remoteData']['bucket'];
        //obj['hash'] = file['remoteData']['hash'];
        //obj['fkey'] = file['remoteData']['key'];
        //$.post(routes.prvt.attach.save, obj).done(function (data) {
        //    Alertsuccess("保存成功");
        //}).fail(function () {
        //    AlertInfo("上传失败");
        //});
    },
    onError: function (error) {
        AlertInfo(error.message);
    }
};

var $logoUploader = $('#logoUploader').uploader(options_logo).data("zui.uploader");


//删除主图
$(document.body).on("click", "#closebtn", function () {
    var keyval = $("#imgkey").val();
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterCommonService.asmx/DeleteQiNiuImage",
        data: '{"key":"' + keyval + ' "}',
        async: false,
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            if (data == "200") {
                $("#selideimg").attr("src", "../../Image/noimg.png");
                $("#imgkey").val();
            }
            else {
                AlertError("删除失败");
            }
        }
    });
});