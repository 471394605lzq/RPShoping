var _id = $.query.get("id") || "";
var _m_id = $.query.get("m_id") || "";
var _copy = $.query.get("copy") || "";
var tempid = _id;
var _appid = getappid();
var _appkey = getshaappkey();
var dbcmd = "";
if (_id == "" && _copy == "") {
    dbcmd = "add";
    $("#balance").val("10000");
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
    window.location.href = "systemuserlist.aspx?m_id=" + _m_id;
});
$("#back").click(function () {
    window.location.href = "systemuserlist.aspx?m_id=" + _m_id;
});
//保存
$("#save").click(function () {
    var saveappkey = getshaappkey();
    var username = $("#username").val();
    var nickname = $("#nickname").val();
    var balance = $("#balance").val();

    


    var checkresult = check();
    if (checkresult) {
        if (dbcmd == "add") {
            $.ajax({
                "url": "https://d.apicloud.com/mcm/api/user",
                "type": "POST",
                "cache": false,
                "headers": {
                    //"Content-type": "application/json",
                    "X-APICloud-AppId": _appid,
                    "X-APICloud-AppKey": saveappkey
                },
                "data": {
                    "username": username,
                    "password": "sys123456",
                    "email": ""
                }
            }).done(function (data, status, header) {
                var tempusid = data.id;
                empetclt();
                var filter = {
                    "order": "onlynumber DESC",
                    "limit": 1
                }
                $.ajax({
                    "url": "https://d.apicloud.com/mcm/api/user?filter=" + encodeURIComponent(JSON.stringify(filter)),
                    "type": "GET",
                    "cache": false,
                    "headers": {
                        "X-APICloud-AppId": _appid,
                        "X-APICloud-AppKey": saveappkey
                    }
                }).done(function (data, status, header) {
                    //console.log(data);
                    var tempnumber=data[0].onlynumber;
                    var resultnumber=parseInt(tempnumber)+1;
                    $.ajax({
                        "url": "https://d.apicloud.com/mcm/api/user/" + tempusid,
                        "type": "POST",
                        "cache": false,
                        "headers": {
                            //"Content-type": "application/json",
                            "X-APICloud-AppId": _appid,
                            "X-APICloud-AppKey": saveappkey
                        },
                        "data": {
                            "issystem": "是",
                            "sex": "男",
                            "handimg": "../image/user/dh.png",
                            "nickname": nickname,
                            "balance": balance,
                            "integral": "0",
                            "rewardmoney": "0",
                            "onlynumber":resultnumber,
                            "_method": "PUT"
                        }
                    }).done(function (data, status, header) {
                        Alertsuccess("保存成功");
                        //success body
                    }).fail(function (header, status, errorThrown) {
                        //fail body
                    });
                    //success body
                }).fail(function (header, status, errorThrown) {
                    //fail body
                })
                
                //success body
            }).fail(function (header, status, errorThrown) {
                //fail body
            });
        }
        else {
            $.ajax({
                "url": "https://d.apicloud.com/mcm/api/user/" + tempid,
                "type": "POST",
                "cache": false,
                "headers": {
                    //"Content-type": "application/json",
                    "X-APICloud-AppId": _appid,
                    "X-APICloud-AppKey": saveappkey
                },
                "data": { 
                    "nickname": nickname,
                    "balance": balance,
                    "_method": "PUT"
                }
            }).done(function (data, status, header) {
                Alertsuccess("保存成功");
                //success body
            }).fail(function (header, status, errorThrown) {
                //fail body
            });
        }
    }
});
//验证保存信息
function check() {
    var checkresult = true;
    var username = $("#username").val();
    var nickname = $("#nickname").val();
    var balance = $("#balance").val();

    if (username == "" || username == null || username == undefined) {
        AlertInfo("请输入用户登陆名！");
        $("#username").focus();
        checkresult = false;
    }
    if (nickname == "" || nickname == null || nickname == undefined) {
        AlertInfo("请输入用户昵称！");
        $("#nickname").focus();
        checkresult = false;
    }
    if (balance == "" || balance == null || balance == undefined) {
        AlertInfo("请设置账户余额！");
        $("#balance").focus();
        checkresult = false;
    }
    return checkresult;
}
//清空控件值
function empetclt() {
    $("#nickname").val("");
    var tempusername = $("#username").val();
    var tempstr = parseInt(tempusername) + 1;
    $("#username").val(tempstr);
    $("#balance").val("10000");
}
//编辑加载方法
function load(id) {
    var loadppkey = getshaappkey();
    $.ajax({
        "url": "https://d.apicloud.com/mcm/api/user/" + id,
        "type": "GET",
        "cache": false,
        headers: {
            "Content-type": "application/json",
            "X-APICloud-AppId": _appid,
            "X-APICloud-AppKey": loadppkey
        },
    }).done(function (data, status, header) {
        console.log(data);
        $("#nickname").val(data.nickname);
        $("#username").val(data.username);
        $("#balance").val(data.balance);
        //success body
    }).fail(function (header, status, errorThrown) {
        //fail body
    });
    //var filter = {
    //    "order": "onlynumber DESC",
    //    "limit": 1
    //}
}


