//获取appid
var getappid = function () {
    var appid = "A6945248167961";
    return appid;
};
//获取加密的appkey
var getshaappkey = function () {
    var now = Date.now();
    var appid = "A6945248167961";
    var appkey = "8B57C137-F109-EBDD-9762-BCE9254D17CB";
    var shaappkey = SHA1(appid + "UZ" + appkey + "UZ" + now) + "." + now;
    return shaappkey;
};


//公用删除方法
var CommonDelete = function (tablename, id, idvalue, callback) {
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterCommonService.asmx/CommonDelete",
        data: '{"tablename":"' + tablename + '","id":"' + id + '","idvalue":"'+idvalue + '"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            if (parseInt(data) > 0) {
                callback("1");
            }
            else {
                callback("删除失败,详情：" + data);
            }
        }
    });
};
//公用物理删除方法(修改isdelete字段状态)
var CommonIsDelete = function (tablename, id, idvalue,state,setcolum, callback) {
    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "/Contract/AfterCommonService.asmx/CommonIsDelete",
        data: '{"tablename":"' + tablename + '","id":"' + id + '","idvalue":"' + idvalue + '","state":"' + state + '","setcolum":"' + setcolum + '"}',
        dataType: 'json',
        success: function (result) {
            var data = result.d;
            if (parseInt(data) > 0) {
                callback("1");
            }
            else {
                callback("操作失败,详情：" + data);
            }
        }
    });
};


