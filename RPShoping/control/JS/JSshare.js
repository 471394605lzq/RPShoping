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
            $("#androidisnoshow").css("display", "none");
            $("#androidshow").css("display", "block");
            //$("#androidimg").attr("src", data[0].androidqrcodepath);
            $('#androidurl').attr('href', data[0].androidurl);
        }
        else {
            $("#androidisnoshow").css("display", "block");
            $("#androidshow").css("display", "none");
        }
        if (iosisup == "是") {
            $("#iosisnoshow").css("display", "none");
            $("#iosshow").css("display", "block");
            //$("#iosimg").attr("src", data[0].iosqrcodepath);
            $('#iosurl').attr('href', data[0].iosurl);
        }
        else {
            $("#iosisnoshow").css("display", "block");
            $("#iosshow").css("display", "none");
        }
    },
    error: function (data, status, header) {

    }
});