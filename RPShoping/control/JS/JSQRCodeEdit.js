var _id = $.query.get("id") || "";
var _m_id = $.query.get("m_id") || "";
var _copy = $.query.get("copy") || "";
var tempid = _id;
var _appid = getappid();
var dbcmd = "";
if (_id == "" && _copy == "") {
    dbcmd = "add";
}
else if (_id != "" && _copy != "") {
    dbcmd = "add";
    tempid = "";
    load(_id);
}
else {
    dbcmd = "edit";
    load(_id);
}
//关闭页面
$("#cancel").click(function () {
    window.location.href = "QRCode.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "QRCode.aspx?m_id=" + _m_id;
});
//保存
$("#save").click(function () {
    var saveappkey = getshaappkey();
    var androidqrcodepath = $("#androidqrcodepath").attr('src');
    var iosqrcodepath = $("#iosqrcodepath").attr('src');
    var androidimgkey=$("#androidimgkey").val();
    var iosimgkey = $("#iosimgkey").val();
    var androidisup = $('input[name="androidisup"]:checked').val();
    var iosisup = $('input[name="iosisup"]:checked').val();
    var androidurl = $("#androidurl").val();
    var iosurl = $("#iosurl").val();
    var checkresult = check();
    if (checkresult) {
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_QRCode/" + tempid,
            method: "POST",
            headers: {
                //"Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": saveappkey
            },
            "data": {
                "androidqrcodepath": androidqrcodepath,
                "iosqrcodepath": iosqrcodepath,
                "androidimgkey": androidimgkey,
                "iosimgkey": iosimgkey,
                "androidisup": androidisup,
                "iosisup": iosisup,
                "androidurl": androidurl,
                "iosurl": iosurl,
                "_method": "PUT"
            },
            dataType: "json",
            success: function (data, status, header) {
                //alert(JSON.stringify(data));
                Alertsuccess("保存成功");
            },
            error: function (data, status, header) {

            }
        });
    }
});
//验证保存信息
function check() {
    var checkresult = true;
    var androidqrcodepath = $("#androidqrcodepath").attr('src');
    var iosqrcodepath = $("#iosqrcodepath").attr('src');
    var androidurl = $("#androidurl").val();
    var iosurl = $("#iosurl").val();
    var androidisup = $('input[name="androidisup"]:checked').val();
    var iosisup = $('input[name="iosisup"]:checked').val();
    //var detailcontent = $("#content").val();
    if (androidisup == "是") {
        if (androidqrcodepath == "" || androidqrcodepath == null || androidqrcodepath == undefined) {
            AlertInfo("请设置安卓二维码图片！");
            checkresult = false;
        }
    }
    if (iosisup == "是") {
        if (iosqrcodepath == "" || iosqrcodepath == null || iosqrcodepath == undefined) {
            AlertInfo("请设置苹果二维码图片！");
            checkresult = false;
        }
    }
    if (androidisup == "是") {
        if (androidurl == "" || androidurl == null || androidurl == undefined) {
            AlertInfo("安卓下载地址不能为空！");
            checkresult = false;
        }
    }
    if (iosisup == "是") {
        if (iosurl == "" || iosurl == null || iosurl == undefined) {
            AlertInfo("苹果下载地址不能为空！");
            checkresult = false;
        }
    }
    return checkresult;
}
//清空控件值
function empetclt() {
    $("#androidimgkey").val("");
    $("#iosimgkey").val("");
    $("#androidqrcodepath").attr("src", "../../Image/noimg.png");
    $("#iosqrcodepath").attr("src", "../../Image/noimg.png");
    $("input[name=androidisup]:eq('否')").attr("checked", 'checked');
    $("input[name=iosisup]:eq('否')").attr("checked", 'checked');
    $("#androidurl").val("");
    $("#iosurl").val("");
}
//编辑加载方法
function load(id) {
    var loadppkey = getshaappkey();
    $.ajax({
        url: "https://d.apicloud.com/mcm/api/tb_QRCode/" + id,
        method: "get",
        headers: {
            "Content-type": "application/json",
            "X-APICloud-AppId": _appid,
            "X-APICloud-AppKey": loadppkey
        },
        //data:{"id":_id},
        dataType: "json",
        success: function (data, status, header) {
            $("#androidqrcodepath").attr("src", data.androidqrcodepath);
            $("#iosqrcodepath").attr("src", data.iosqrcodepath);
            $("#androidimgkey").val(data.androidimgkey);
            $("#iosimgkey").val(data.iosimgkey);
            $("input[name='androidisup'][value='" + data.androidisup + "']").attr("checked", true);
            $("input[name='iosisup'][value='" + data.iosisup + "']").attr("checked", true);
            $("#androidurl").val(data.androidurl);
            $("#iosurl").val(data.iosurl);
        },
        error: function (data, status, header) {

        }
    });
}



var options_logo = {
    limitFilesCount: 1,
    qiniu: {
        uptoken_url: '/Contract/AfterCommonService.asmx/SetQiNiuToken',
        //domain: 'http://qiniu.rpshoping.com' + '?key=',
        domain: 'http://qiniu.rpshoping.com/',
        flash_swf_url: '../../control/uploader/Moxie.swf',
        max_file_size: '3mb',
        max_retries: 3,
        multi_selection: true,
        key: function (up, file) {
            //file.id
            return "qrcode"+new Date().format("yyyyMMddmmss") + file.name;
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
        $("#minkey").val(key);
        $("#minimg").attr("src", url);
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


//当鼠标移动到主图上时显示设置为缩略图按钮显示
$(document.body).on("mouseover", "#mainimagecontent .imagediv", function () {
    var thisimgctl = $(this);
    var setbtn = thisimgctl.children(".setslt");
    setbtn.css("display", "block");
});
//当鼠标移开主图时显示设置为缩略图按钮隐藏
$(document.body).on("mouseout", "#mainimagecontent .imagediv", function (event) {
    var thisimgctl = $(this);
    var setbtn = thisimgctl.children(".setslt");
    setbtn.css("display", "none");
});
//设置安卓二维码图片
$("#androidsetben").click(function () {
    var oldkey = $("#androidimgkey").val();
    if (oldkey != "" && oldkey != null && oldkey != undefined)
    {
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/Contract/AfterCommonService.asmx/DeleteQiNiuImage",
            data: '{"key":"' + oldkey + ' "}',
            async: false,
            dataType: 'json',
            success: function (result) {
                var data = result.d;
                if (data == "200") {
                    $("#androidqrcodepath").attr("src", "../../Image/noimg.png");
                }
                else {
                    AlertError("删除失败");
                }
            }
        });
    }
    var imgsrc = $("#minimg").attr('src');
    var keyval = $("#minkey").val();
    $("#androidimgkey").val(keyval);
    $("#androidqrcodepath").attr("src", imgsrc);

});
//设置苹果二维码图片
$("#iossetben").click(function(){
    var oldkey = $("#iosimgkey").val();
    if (oldkey != "" && oldkey != null && oldkey != undefined) {
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/Contract/AfterCommonService.asmx/DeleteQiNiuImage",
            data: '{"key":"' + oldkey + ' "}',
            async: false,
            dataType: 'json',
            success: function (result) {
                var data = result.d;
                if (data == "200") {
                    $("#androidqrcodepath").attr("src", "../../Image/noimg.png");
                }
                else {
                    AlertError("删除失败");
                }
            }
        });
    }
    var imgsrc = $("#minimg").attr('src');
    var keyval = $("#minkey").val();
    $("#iosimgkey").val(keyval);
    $("#iosqrcodepath").attr("src", imgsrc);
});
//删除安卓二维码图片
$("#androiddelbtn").click(function () {
    var androidqrcodepath = $("#androidqrcodepath").attr('src');
    var key = $("#androidimgkey").val();
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterCommonService.asmx/DeleteQiNiuImage",
        data: '{"key":"' + key + ' "}',
        async: false,
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            if (data == "200") {
                $("#androidqrcodepath").attr("src", "../../Image/noimg.png");
                $("#androidimgkey").val("");
            }
            else {
                AlertError("删除失败");
            }
        }
    });
});
//删除苹果二维码图片
$("#iosdelbtn").click(function () {
    var key = $("#iosimgkey").val();
    var iosqrcodepath = $("#iosqrcodepath").attr('src');
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterCommonService.asmx/DeleteQiNiuImage",
        data: '{"key":"' + key + ' "}',
        async: false,
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            if (data == "200") {
                $("#iosqrcodepath").attr("src", "../../Image/noimg.png");
                $("#iosimgkey").val("");
            }
            else {
                AlertError("删除失败");
            }
        }
    });
});
//删除主图
$("#closebtn").click(function () {
    $("#minkey").val("");
    $("#minimg").attr("src", "../../Image/noimg.png");
    //var thisctl = $(this);
    //var imagediv = thisctl.parents('.imagediv');
    //var classs = imagediv.attr("class");
    //var key = imagediv.children(".key");
    //var keyval = key.val();
    //$.ajax({
    //    type: "post",
    //    contentType: "application/json",
    //    url: "/Contract/AfterCommonService.asmx/DeleteQiNiuImage",
    //    data: '{"key":"' + keyval + ' "}',
    //    async: false,
    //    dataType: 'json',
    //    success: function (result) {
    //        var data = result.d;
    //        if (data == "200") {
    //            imagediv.remove();
    //        }
    //        else {
    //            imagediv.remove();
    //            AlertError("删除失败");
    //        }
    //    }
    //});
});
