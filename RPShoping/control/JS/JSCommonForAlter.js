//纯文本提示
var AlertText = function (val) {
    layer.open({
        type: 0,
        moveType: 1,
        offset: ['100px', '600px'],
        title: "消息提示！",
        content: val
    });
    $(".layer-anim").css("color", "#ffffff");
    $(".layer-anim").css("background-color", "#438eb9");
}
//一般提示(5秒关闭)
var AlertInfo = function (val) {
    layer.msg(val, {
        //icon: 0,
        offset: ['100px', '600px'],
        time: 5000 //5秒关闭（如果不配置，默认是3秒）
    }, function () {
    });
    //$(".layer-anim").css("color", "#438eb9");
    $(".layer-anim").css("color", "#ffffff");
    $(".layer-anim").css("background-color", "#438eb9");
};
//错误提示(5秒关闭)
var AlertError = function (val) {
    layer.msg(val, {
        //icon: 5,
        offset: ['100px', '600px'],
        time: 5000 //5秒关闭（如果不配置，默认是3秒）
    }, function () {
    });
    //$(".layer-anim").css("color", "red");
    $(".layer-anim").css("color", "#ffffff");
    $(".layer-anim").css("background-color", "red");
};
//成功提示(2秒关闭)
var Alertsuccess = function (val) {
    layer.msg(val, {
        //icon: 1,
        offset: ['100px', '600px'],
        time: 2000 //2秒关闭（如果不配置，默认是3秒）
    }, function () {
    });
    //$(".layer-anim").css("color", "#2dd163");
    $(".layer-anim").css("color", "#ffffff");
    $(".layer-anim").css("background-color", "#2dd163");
};
//带回调的成功提示
var Alertsuccesscallback = function (val, callback) {
    layer.msg(val, {
        //icon: 1,
        offset: ['100px', '600px'],
        time: 1000 //1秒关闭（如果不配置，默认是3秒）
    }, function () {
        callback("1");
    });
    //$(".layer-anim").css("color", "#2dd163");
    $(".layer-anim").css("color", "#ffffff");
    $(".layer-anim").css("background-color", "#2dd163");
};

//询问提示
var AlertConfirm = function (val, callback) {
    layer.confirm(val,
        {
            icon: 3,
            moveType: 1,
            offset: ['100px', '600px'],
            btn: ['确定', '取消'], //按钮
            title: "提示！"
        }, function (index) {
            layer.close(index);
            callback("1");
        }, function () {
            callback("0");
        })
};
//带输入框提示
var AlertBackText = function (callback) {
    layer.prompt(function (val, index) {
        callback(val);
        layer.close(index);
    });
}
