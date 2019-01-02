var _id = $.query.get("id") || "";
var _m_id = $.query.get("m_id") || "";
var _appid = getappid();
var _appkey = getshaappkey();
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
    window.location.href = "MerchantList.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "MerchantList.aspx?m_id=" + _m_id;
});
//保存
$("#save").click(function () {
    var saveappkey = getshaappkey();
    var name = $("#name").val();
    var tel = $("#tel").val();
    var address = $("#address").val();
    var longitudestr = $("#longitude").val();
    var dimensionstr = $("#dimension").val();
    //alert(longitudestr + "," + dimensionstr);
    var reamrk = $("#remark").val();
    var province = $("#province").children('option:selected').val();
    var provincesplit = province.split(",");
    var city = $("#city").children('option:selected').val();
    var citysplit = city.split(",");
    var town = $("#town").children('option:selected').val();
    var townsplit = town.split(",");
    var state = $('input[name="state"]:checked').val();

    if (name != "" && name != null && name != undefined) {
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_BackstageMerchant/" + _id,
            method: "POST",
            headers: {
                //"Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": saveappkey
            },
            "data": {
                "BM_Name": name,
                "BM_Tel": tel,
                "BM_Address": address,
                "BM_Status": state,
                "BM_Remark": reamrk,
                "point": { lat: longitudestr, lng: dimensionstr },
                "province": provincesplit[0],
                "city": citysplit[0],
                "town": townsplit[0],
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
        AlertInfo("请输入商户名称！");
        $("#sortname").focus();
        return;
    }
});
//清空控件值
function empetclt() {
    $("#name").val("");
    $("#tel").val("");
    $("#address").val("");
    $("#longitude").val("");
    $("#dimension").val("");
    $("#remark").val("");
    $("#province option[value='0']").attr("selected", "selected");
    $("#city").empty();
    $("#town").empty();
    $("input[name=state]:eq(0)").attr("checked", 'checked');
}
//编辑加载方法
function load() {
    var filter = {
        "include": ["provincePointer", "cityPointer", "townPointer"],
        "where": { "id": _id }
    }
    $.ajax({
        //url: "https://d.apicloud.com/mcm/api/tb_BackstageMerchant/" + _id,
        url: "https://d.apicloud.com/mcm/api/tb_BackstageMerchant?filter=" + encodeURIComponent(JSON.stringify(filter)),
        method: "get",
        headers: {
            "Content-type": "application/json",
            "X-APICloud-AppId": _appid,
            "X-APICloud-AppKey": _appkey
        },
        //data:{"id":_id},
        dataType: "json",
        success: function (data, status, header) {
            $("#name").val(data[0].BM_Name);
            $("#tel").val(data[0].BM_Tel);
            $("#address").val(data[0].BM_Address);
            var point = data[0].point;
            $("#longitude").val(point.lat);
            $("#dimension").val(point.lng);
            $("#remark").val(data[0].BM_Remark);
            $("input[name='state'][value='" + data[0].BM_Status + "']").attr("checked", true);
            $("#province option[value='" + data[0].province.id + "," + data[0].province.AreaID + "']").attr("selected", "selected");

            loadarea(data[0].province.AreaID, "city", data[0].city.id + "," + data[0].city.AreaID, function (c) {
                if (c == "1") {
                    loadarea(data[0].city.AreaID, "town", data[0].town.id + "," + data[0].town.AreaID, function (c) {

                    })
                }
            })
        },
        error: function (data, status, header) {

        }
    });
}
//加载城市控件
function loadarea(id,ctlid,loadval, callback)
{
    var filter = {
        "where": { "ParentID": id }
    }
    $.ajax({
        url: "https://d.apicloud.com/mcm/api/Area?filter=" + encodeURIComponent(JSON.stringify(filter)),
        method: "get",
        headers: {
            "Content-type": "application/json",
            "X-APICloud-AppId": _appid,
            "X-APICloud-AppKey": _appkey
        },
        //data:{"name":"zhangsan","age":"20"},
        dataType: "json",
        success: function (data, status, header) {
            $("#" + ctlid).empty();
            $("select[name=" + ctlid + "]").append('<option value="0">请选择</option>');
            //var info = jQuery.parseJSON(data);
            if (data.length > 0) {
                for (i = 0; i < data.length; i++) {
                    $("select[name=" + ctlid + "]").append('<option value="' + data[i].id+","+data[i].AreaID + '">' + data[i].Name + '</option>');
                }
                $("#" + ctlid + " option[value='" + loadval + "']").attr("selected", "selected");
                callback("1");
            }
        },
        error: function (data, status, header) {
            console.log(data);
        }
    });


    //$.ajax({
    //    type: "post",
    //    contentType: "application/json",
    //    url: "/Contract/AfterCommonService.asmx/getareainfo",
    //    data: '{"id":"' + id + '"}',
    //    dataType: 'json',
    //    success: function (result) {
    //        var data = result.d;
    //        if (data != "0") {
    //            $("#" + ctlid).empty();
    //            $("select[name=" + ctlid + "]").append('<option value="0">请选择</option>');
    //            var info = jQuery.parseJSON(data);
    //            for (i = 0; i < info.length; i++) {
    //                $("select[name=" + ctlid + "]").append('<option value="' + info[i].AreaID + '">' + info[i].Name + '</option>');
    //            }
    //            $("#" + ctlid + " option[value='" + loadval + "']").attr("selected", "selected");
    //            callback("1");
    //        }
    //        else {
    //        }
    //    }
    //});
}
var longtitude = 0;
var latitude = 0;
//根据位置信息获取经度纬度
function GetPostion(address) {
    //通过百度获取经纬度
    //var address = "广东省东莞市莞城区创业新村";
    var url = "http://api.map.baidu.com/geocoder/v2/?address=" + address + "&output=json&ak=5482a36ffce6210692796439b05bcb6d&callback=?";
    $.getJSON(url, function (data) {
        longtitude = data.result.location.lng;
        latitude = data.result.location.lat;
        $("#longitude").val(longtitude);
        $("#dimension").val(latitude);
        //alert(longtitude + "," + latitude);
    });
}
//选择省份事件
$("#province").change(function () {
    var thisvalue = $("#province").children('option:selected').val();
    var splitstr = thisvalue.split(",");
    loadarea(splitstr[1], "city", "0", function (c) {
        if (c == "1")
        {
            $("#town").empty();
        }
    });
});
//选择城市事件
$("#city").change(function () {
    var thisvalue = $("#city").children('option:selected').val();
    var splitstr = thisvalue.split(",");
    loadarea(splitstr[1], "town", "0", function () {

    });
});
//选择镇区事件
$("#town").change(function () {
    var province = $("#province").children('option:selected').text();
    var city = $("#city").children('option:selected').text();
    var town = $("#town").children('option:selected').text();
    if (province != "" && province != undefined && province != null) {
        if (city != "" && city != undefined && city != null) {
            if (town != "" && town != undefined && town != null) {
                GetPostion(province + city + town);
            }
            else {
                AlertInfo("请选择镇区！");
                return;
            }
        }
        else {
            AlertInfo("请选择城市！");
            return;
        }
    }
    else {
        AlertInfo("请选择省份！");
        return;
    }
});
//移动本地area表数据到数据云上的area表
$("#movearea").click(function () {
    //$.ajax({
    //    type: "post",
    //    contentType: "application/json",
    //    url: "/Contract/AfterService.asmx/MoveAreaInfo",
    //    data: '',
    //    dataType: 'json',
    //    success: function (result) {
    //        var data = result.d;
    //        if (parseInt(data) > 0) {
    //            Alertsuccess("保存成功");
    //        }
    //        else {
    //            AlertError("保存失败" + data);
    //        }
    //    }
    //});


    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterService.asmx/SetAreaToTxt",
        data:'',
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            if (parseInt(data) > 0) {
                Alertsuccess("保存成功");
            }
            else {
                AlertError("保存失败" + data);
            }
        }
    });
});