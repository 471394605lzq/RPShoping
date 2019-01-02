var _appid = getappid();
var loadppkey = getshaappkey();
$.ajax({
    url: "https://d.apicloud.com/mcm/api/tb_QRCode/",
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
        var androidisup = data[0].androidisup;
        var iosisup = data[0].iosisup;
        if (androidisup == "是") {
            $("#androidtempdiv").css("display", "none");
            $("#androidimg").css("display", "block");
            $("#androidimg").attr("src", data[0].androidqrcodepath);
        }
        else {
            $("#androidtempdiv").css("display", "block");
            $("#androidimg").css("display", "none");
        }
        if (iosisup == "是") {
            $("#iostempdiv").css("display", "none");
            $("#iosimg").css("display", "block");
            $("#iosimg").attr("src", data[0].iosqrcodepath);
        }
        else {
            $("#iostempdiv").css("display", "block");
            $("#iosimg").css("display", "none");
        }
    },
    error: function (data, status, header) {

    }
});