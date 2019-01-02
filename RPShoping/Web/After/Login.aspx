<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RPShoping.Web.After.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>后台登陆</title>
    <link href="../../CSS/fastermain.css" rel="stylesheet" />
    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../control/JS/JSLogin.js"></script>
</head>
<body style="height: 100%;">
    <div class="wrapper" style="position: relative; top: 25%;">
        <div style="background: #b390f4 url(../../CSS/images/20150710114244511.jpg) top center no-repeat;" class="g-login-con clrfix" id="g_login">
            <div class="m-login-screen clrfix">
                <div id="loadingPicBlock" class="screen-left fl"></div>
                <div class="screen-right fr clrfix">
                    <div class="login-panel" id="LoginForm">
                        <dl>
                            <dt>
                                <em class="fl">登录</em>
                            </dt>
                            <dd>
                                <div class="register-form-con clrfix">
                                    <ul>
                                        <li>
                                            <input style="color: rgb(187, 187, 187);" id="zh_in" value="手机号/邮箱地址" maxlength="100" autocomplete="off" type="text" />
                                            <input type="text" class="hidetext" value="" id="usename" />
                                            <b class="passport-icon user-name transparent-png"></b>
                                            <b id="clearusername" style="display: none; width: 15px; height: 15px; color: #ff6600; line-height: 12px; text-indent: 0.1em; text-align: center; border-radius: 180px; position: absolute; left: 280px; font-size: 18px; top: 13px; cursor: pointer;">x</b>
                                            <!-- <em style="">手机号/邮箱地址</em>-->
                                        </li>
                                        <li>
                                            <input style="color: rgb(187, 187, 187);" id="hidepwd" maxlength="20" value="请输入您的密码" type="text" />
                                            <input class="hidetext" type="password" value="" id="pwd" />
                                            <b class="passport-icon login-password transparent-png"></b>
                                            <b id="clearpwd" style="display: none; width: 15px; height: 15px; color: #ff6600; line-height: 12px; text-indent: 0.1em; text-align: center; border-radius: 180px; position: absolute; left: 280px; font-size: 18px; top: 13px; cursor: pointer;">x</b>
                                            <!--<em style="">密码</em>-->
                                        </li>
                                    </ul>
                                </div>
                            </dd>
                            <dd class="error-message orange" style="display: none;" id="dd_error_msg"></dd>
                        </dl>
                        <p><a id="btnSubmitLogin" class="z-agreeBtn" tabindex="4">登录</a></p>
                        <ul id="j-tips-wrap" class="j-tips-wrap j-login-page">
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
