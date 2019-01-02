$(function () {
    $('#usename').focus();

    //window.onkeydown = function (e) {
    //    if (e.keyCode == 9) {
    //        document.getElementById('tx').value = document.getElementById('tx').value + "    "
    //        return false;
    //    }
    //}
    //对用户名框进行操作 
    $("#zh_in").focus(function () {   //用户名框获得鼠标焦点 
        var txt_value = $(this).val();     //得到当前文本框的值
        if (txt_value == this.defaultValue) {
            $(this).hide();
            $('#usename').show();
            $('#usename').focus();
            //$('#dd_error_msg').hide();
            $('.user-name').css('background-position', '-25px 0');
            $('#usename').css('border-color', '#ff6600');
        }
        $('#usename').focus();
    });
    $("#usename").focus(function () {
        $('.user-name').css('background-position', '-25px 0');
        $('#usename').css('border-color', '#ff6600');
    });

    $("#usename").keyup(function () {
        var thisvalue = $("#usename").val();
        if(thisvalue!="")
        {
            $("#clearusername").show();
            //$('#dd_error_msg').hide();
        }
    });
    $("#usename").blur(function () {  //用户名框失去鼠标焦点 
        var txt_value = $("#usename").val();   //得到当前文本框的值
        $('.user-name').css('background-position', '0 0');
        $('#usename').css('border-color', '#DDD');
        if (txt_value == "") {
            $('#zh_in').show();
            $("#usename").hide();						//如果符合条件，则设置内容 		$(this).val(this.defaultValue);
            $("#clearusername").hide();
        }
        $(this).removeClass("focus");
    });
    

    //对密码框进行操作 
    $('#hidepwd').click(function () {
        if ($('#hidepwd').val() == "请输入您的密码") {
            $('#hidepwd').hide();
            $('#pwd').show();
            $('#pwd').focus();
            $('.login-password').css('background-position', '-25px -201px');
            $('#pwd').css('border-color', '#ff6600');
            $('#dd_error_msg').hide();
        }
        $('#pwd').addClass("focus");
    });
    $('#hidepwd').focus(function () {
        if ($('#hidepwd').val() == "请输入您的密码") {
            $('#hidepwd').hide();
            $('#pwd').show();
            $('#pwd').focus();
            $('.login-password').css('background-position', '-25px -201px');
            $('#pwd').css('border-color', '#ff6600');
            $('#dd_error_msg').hide();
        }
        $('#pwd').addClass("focus");
    });

    $("#pwd").focus(function () {
        $('.login-password').css('background-position', '-25px -201px');
        $('#pwd').css('border-color', '#ff6600');
    });
    $("#pwd").keyup(function () {
        if ($('#pwd').val() != "") {
            $("#clearpwd").show();
            $('#dd_error_msg').hide();
        }
    });
    $('#pwd').blur(function () {
        $('.login-password').css('background-position', '0 -25px');
        $('#pwd').css('border-color', '#DDD');
        if ($('#pwd').val() == "") {
            $('#pwd').hide();
            $('#hidepwd').show();
            $("#clearpwd").hide();
        }
        $(this).removeClass("focus");
    });
    //回车键登陆
    document.onkeydown = function(e){ 
        var ev = document.all ? window.event : e;
        if(ev.keyCode==13) {
            login();
            }
    }
    //登陆
    $('#btnSubmitLogin').click(function () {
        login();
    });
    //登陆方法
    function login()
    {
        var username = $('#usename').val();
        var pwd = $('#pwd').val();
        if (username == "") {
            $('#dd_error_msg').show();
            $('#dd_error_msg').html("请输入用户名！");
            $("#zh_in").hide();
            $('#usename').show();
            $('#usename').focus();
            return false;
        }
        else if (pwd == "") {
            $('#dd_error_msg').show();
            $('#dd_error_msg').html("请输入密码！");
            $('#hidepwd').hide();
            $('#pwd').show();
            $('#pwd').focus();
            return false;
        }
        else {
            $.ajax({
                type: "post",
                url: 'LoginForAfter.ashx?ajax=Login',
                data: { "usname": username, "pwd": pwd },
                success: function (d, status, resp) {
                    var c = d.split("***");
                    if (c[0] == "111111") {
                        window.location.href = "/web/after/index.aspx";
                    }
                    else {
                        $('#usename').focus();
                        $('#dd_error_msg').show();
                        $('#dd_error_msg').html("用户名或密码错误！");
                        $('#pwd').val("");
                        $("#clearpwd").hide();
                    }
                }
            });
        }
    }
    //清除用户名
    $("#clearusername").click(function () {
        $('#usename').focus();
        $('#usename').val("");
        $("#clearusername").hide();
    });
    //清除密码
    $("#clearpwd").click(function () {
        $('#pwd').focus();
        $('#pwd').val("");
        $("#clearpwd").hide();
    });

});