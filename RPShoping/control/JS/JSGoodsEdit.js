var _id = $.query.get("id") || "";
var _m_id = $.query.get("m_id") || "";
var _copy = $.query.get("copy") || "";
var tempid = _id;
var _appid = getappid();
var dbcmd = "";
if (_id == "" && _copy=="") {
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
    window.location.href = "Goods.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "Goods.aspx?m_id=" + _m_id;
});
//选择商户时更新经度纬度
$("#merchant").change(function () {
    var shid = $("#merchant").children('option:selected').val();
    if (shid != "" && shid != null && shid != undefined && shid!="0") {
        var loadppkey = getshaappkey();
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_BackstageMerchant/" + shid,
            method: "get",
            headers: {
                "Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": loadppkey
            },
            //data:{"id":_id},
            dataType: "json",
            success: function (data, status, header) {
                console.log(data);
                var point = data.point;
                //alert(point.lat);
                $("#longitude").val(point.lat);
                $("#dimension").val(point.lng);
            },
            error: function (data, status, header) {

            }
        });
    }
    else {
        $("#longitude").val("");
        $("#dimension").val("");
    }
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
    var thumbnail = $("#thumbnailimg").attr('src');
    var type = $("#type").children('option:selected').val();
    var state = $('input[name="state"]:checked').val();
    var isdistance = $('input[name="isdistance"]:checked').val();
    var longitudestr = $("#longitude").val();
    var dimensionstr = $("#dimension").val();
    var mainimg = "";
    $("#mainimagecontent .imagediv .image").each(function (index, element) {
        mainimg =mainimg+ $(element).attr('src')+",";
    });
    mainimg = mainimg.substring(0, mainimg.length - 1);
    editor.sync();
    var tempcontent=$("#content").val();
    var detailcontent = escape(tempcontent);

    var checkresult = check();
    if (checkresult) {
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_Product/" + tempid,
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
                "p_thumbnail": thumbnail,
                "P_Type": type,
                "p_state": state,
                "isdistance": isdistance,
                "point": { lat: longitudestr, lng: dimensionstr },
                "_method": "PUT"
            },
            dataType: "json",
            success: function (data, status, header) {
                //alert(JSON.stringify(data));
                if (dbcmd == "add") {
                    empetclt();
                    var p_id = data.id;
                    var p_price = data.P_Price;
                    $.ajax({
                        type: "post",
                        contentType: "application/json",
                        url: "/Contract/AfterService.asmx/SetProductIssue",
                        data: '{"id":"' + p_id + '","productprice":"' + p_price + '","number":"1"}',
                        dataType: 'json',
                        success: function (result) {
                            var data = result.d;
                            if (parseInt(data) == 1) {
                                Alertsuccess("设置成功");
                            }
                            else if (parseInt(data) == 2) {
                                AlertInfo("该产品已有未开始期数！");
                            }
                            else {
                                AlertError("设置失败" + data);
                            }
                        }
                    });
                }
                else {
                    Alertsuccess("保存成功");
                }
            },
            error: function (data, status, header) {

            }
        });
    }
});
//验证保存信息
function check()
{
    var checkresult = true;
    var thumbnail = $("#thumbnailimg").attr('src');
    //var detailcontent = $("#content").val();
    var mainimg = "";
    $("#mainimagecontent .imagediv .image").each(function (index, element) {
        mainimg = mainimg + $(element).attr('src') + ",";
    });
    mainimg = mainimg.substring(0, mainimg.length - 1);
    editor.sync();
    var merchant = $("#merchant").children('option:selected').val();
    var product_sort = $("#product_sort").children('option:selected').val();
    var type = $("#type").children('option:selected').val();
    var product_price = $("#product_price").val();
    var product_name = $("#product_name").val();

    if (thumbnail == "" || thumbnail == null || thumbnail == undefined) {
        AlertInfo("请设置商品缩略图！");
        checkresult = false;
    }
    //if (detailcontent == "" || detailcontent == null || detailcontent == undefined) {
    //    AlertInfo("请设置商品详情！");
    //    checkresult = false;
    //}
    if (mainimg == "" || mainimg == null || mainimg == undefined) {
        AlertInfo("请设置商品主图！");
        checkresult = false;
    }
    if (merchant == "" || merchant == null || merchant == undefined) {
        AlertInfo("请选择商户！");
        $("#merchant").focus();
        checkresult = false;
    }
    if (product_sort == "" || product_sort == null || product_sort == undefined || product_sort==0) {
        AlertInfo("请选择产品分类！");
        $("#product_sort").focus();
        checkresult = false;
    }
    if (product_price == "" || product_price == null || product_price == undefined) {
        AlertInfo("请输入产品价格！");
        $("#product_price").focus();
        checkresult = false;
    }
    if (product_name == "" || product_name == null || product_name == undefined) {
        AlertInfo("请输入产品名称！");
        $("#product_name").focus();
        checkresult = false;
    }
    if (type == "" || type == null || type == undefined || type == 0) {
        AlertInfo("请选择产品类别！");
        $("#type").focus();
        checkresult = false;
    }
    return checkresult;
}
    //清空控件值
    function empetclt() {
        $("#product_name").val("");
        $("#product_price").val("");
        $("#product_sort option[value='0']").attr("selected", "selected");
        $("#merchant option[value='0']").attr("selected", "selected");
        $("#type option[value='0']").attr("selected", "selected");
        $("#product_explain").val("");
        $("#product_remark").val("");
        $("#mainimagecontent").html("");
        $("#thumbnailimg").attr("src", "../../Image/noimg.png");
        $("input[name=state]:eq(0)").attr("checked", 'checked');
        $("input[name=isdistance]:eq(0)").attr("checked", 'checked');
        $("#longitude").val("");
        $("#dimension").val("");
    
    }
    //编辑加载方法
    function load(id) {
        var loadppkey = getshaappkey();
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_Product/" + id,
            method: "get",
            headers: {
                "Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": loadppkey
            },
            //data:{"id":_id},
            dataType: "json",
            success: function (data, status, header) {
                $("#product_name").val(data.P_Name);
                $("#product_price").val(data.P_Price);
                $("#product_sort option[value='" + data.PS_ID + "']").attr("selected", "selected");
                $("#type option[value='" + data.P_Type + "']").attr("selected", "selected");
                $("#merchant option[value='" + data.BM_ID + "']").attr("selected", "selected");
                $("input[name='state'][value='" + data.p_state + "']").attr("checked", true);
                $("input[name='isdistance'][value='" + data.isdistance + "']").attr("checked", true);
                $("#product_explain").val(data.P_Explain);
                $("#product_remark").val(data.P_Remark);
                var tempcontent = unescape(data.P_Details);
                KindEditor.html("#content", tempcontent);
                $("#thumbnailimg").attr("src", data.p_thumbnail);
                var point = data.point;
                $("#longitude").val(point.lat);
                $("#dimension").val(point.lng);
                var imgs = data.P_Image;
                var imglist = imgs.split(",");
                var html = ""; 
                for (i = 0; i < imglist.length; i++) {
                    var temp = imglist[i].split("/");
                    var keyval = temp[3];
                    html = "<div class='imagediv'>" +
                                        "<img class='image' src='" + imglist[i] + "' />" +
                                        "<input type='hidden' class='key' value='" + keyval + "' />" +
                                        "<i id='closebtn' class='fa fa-times closebtn' aria-hidden='true'></i>" +
                                        "<div class='setslt'>设置为缩略图</div>" +
                                    "</div>";
                    $("#mainimagecontent").append(html);
                }
            
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
            multi_selection:true,
            key: function (up, file) {
                //file.id
                return new Date().format("yyyyMMddmmss")+ file.name;
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
            var html = "<div class='imagediv'>"+
                                    "<img class='image' src='" + url + "' />" +
                                    "<input type='hidden' class='key' value='" + key + "' />" +
                                    "<i id='closebtn' class='fa fa-times closebtn' aria-hidden='true'></i>" +
                                    "<div class='setslt'>设置为缩略图</div>"+
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
    //设置主图为缩略图
    $(document.body).on("click", ".setslt", function () {
        var thissetbtn = $(this);
        var parentctl = thissetbtn.parent(".imagediv");
        var setimgctl = parentctl.children(".image");
        var imgsrc = setimgctl.attr('src');
        $("#thumbnailimg").attr("src", imgsrc);
    });
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
                    imagediv.remove();
                    AlertError("删除失败");
                }
            }
        });
    });