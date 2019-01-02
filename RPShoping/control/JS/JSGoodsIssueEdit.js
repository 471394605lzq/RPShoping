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
    window.location.href = "GoodsIssue.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "GoodsIssue.aspx?m_id=" + _m_id;
});
//保存
$("#save").click(function () {
    var saveappkey = getshaappkey();
    var product_name = $("#product_name").val();
    var product_price = $("#product_price").val();
    var product_sort = $("#product_sort").children('option:selected').val();
    var merchant = $("#merchant").children('option:selected').val();
    var product_remark = $("#product_remark").val();
    var product_explain = $("#product_explain").val();
    var mainimg = "";
    $(".imagediv .image").each(function (index, element) {
        mainimg = $(element).attr('src') + ",";
    });
    mainimg = mainimg.substring(0, mainimg.length - 1);
    editor.sync();
    var detailcontent = $("#content").val();

    if (product_name != "" && product_name != null && product_name != undefined) {
        if (product_price != "" && product_price != null && product_price != undefined) {
            if (product_sort != "" && product_sort != null && product_sort != undefined) {
                if (merchant != "" && merchant != null && merchant != undefined) {
                    if (mainimg != "" && mainimg != null && mainimg != undefined) {
                        if (detailcontent != "" && detailcontent != null && detailcontent != undefined) {
                            $.ajax({
                                url: "https://d.apicloud.com/mcm/api/tb_Product/" + _id,
                                method: "POST",
                                headers: {
                                    //"Content-type": "application/json",
                                    "X-APICloud-AppId": _appid,
                                    "X-APICloud-AppKey": saveappkey
                                },
                                "data": {
                                    "PS_ID": product_sort,
                                    "BM_ID": merchant,
                                    "P_Name": product_name,
                                    "P_Image": mainimg,
                                    "P_Explain": product_explain,
                                    "P_Price": product_price,
                                    "P_Details": detailcontent,
                                    "P_Rema": product_remark,
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
                            AlertInfo("请设置商品详情！");
                            return;
                        }
                    }
                    else {
                        AlertInfo("请设置商品主图！");
                        return;
                    }
                }
                else {
                    AlertInfo("请选择商户！");
                    $("#merchant").focus();
                    return;
                }
            }
            else {
                AlertInfo("请选择产品分类！");
                $("#product_sort").focus();
                return;
            }
        }
        else {
            AlertInfo("请输入产品价格！");
            $("#product_price").focus();
            return;
        }
    }
    else {
        AlertInfo("请输入产品名称！");
        $("#product_name").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#sortname").val("");
    $("#icon").val("");
    $("#remark").val("");
}
//编辑加载方法
function load() {
    var loadppkey = getshaappkey();
    $.ajax({
        url: "https://d.apicloud.com/mcm/api/tb_ProductSort/" + _id,
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
        },
        error: function (data, status, header) {

        }
    });
}



var options_logo = {
    limitFilesCount: 1,
    qiniu: {
        uptoken_url: '/Contract/AfterCommonService.asmx/SetQiNiuToken',
        //domain: 'http://om7tfvzjh.bkt.clouddn.com' + '?key=',
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
        var html = "<div class='imagediv'>" +
                                "<img class='image' src='" + url + "' />" +
                                "<input type='hidden' class='key' value='" + key + "' />" +
                                "<i id='closebtn' class='fa fa-times closebtn' aria-hidden='true'></i>" +
                            "</div>";
        $("#mainimagecontent").append(html);
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
    var thisctl = $(this);
    var imagediv = thisctl.parents('.imagediv');
    var classs = imagediv.attr("class");
    var key = imagediv.children(".key");
    var keyval = key.val();
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
                imagediv.remove();
            }
            else {
                AlertInfo("删除失败");
            }
        }
    });
});