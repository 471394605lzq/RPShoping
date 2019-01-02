<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="RPShoping.Web.After.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" onkeydown="" />
    <title>后台管理</title>
    <link href="../../control/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../CSS/master.css" rel="stylesheet" />
    <link href="../../CSS/AfterCommon.css" rel="stylesheet" />
    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../jquery/jquery.cookie.js"></script>
    <script src="../../control/layer/layer.js"></script>
    <script src="../../control/JS/JSCommonForAlter.js"></script>
    <script src="../../control/JS/JSAfterIndex.js"></script>
  
</head>
<body>
    <div id="showdiv" class="showdiv" style="height: 250px; width: 350px; margin-left: 50%; margin-top: 5%;">
        <div class="showdiv_topall">
            <span class="showdiv_toptitle">修改密码</span>
            <i id="closediv" class="fa fa-times" style="float: right; color: red; cursor: pointer; font-size: 18px; margin-right: 15px; height: 50px; line-height: 50px;" aria-hidden="true"></i>
        </div>
        <div class="showul_div" style="height:200px;">
            <ul>
                <li>
                    <span class="s_left" style="width:20%; font-size:14px;">旧密码：</span>
                    <span class="s_right" style="width:200px;">
                        <input type="password" id="oldpwd" class="input_text"  />
                    </span>
                    <span style="color:red; margin-left:10px;line-height:35px;">*</span>
                </li>
                <li>
                    <span class="s_left" style="width:20%; font-size:14px;">新密码：</span>
                    <span class="s_right" style="width:200px;">
                        <input type="password" id="newpwd" class="input_text" />
                    </span><span style="color:red; margin-left:10px;line-height:35px;">*</span>
                </li>
            </ul>
        </div>
        <div class="showdov_bottom" style="height:50px;">
            <div class="showdiv_bottom_right" style="margin-top:10px;">
                <input id="showdivclosebtn" type="button" class="input_botton" value="关 闭" />
                <input id="savemodule" type="button" class="input_botton" style="margin-left: 30px;" value="保 存" />
            </div>
        </div>
    </div>

    <div class="master_top">
        <span class="top_left">后台管理系统</span>
        <div class="top_right" style="width: 250px;">
            您好！ <span id="user_name" style="font-size: 20px; color: #ff6a00;"></span>
            <span id="updatepwd" style="font-size: 12px; cursor: pointer;">修改密码</span>
            <span id="return" style="font-size: 12px; cursor: pointer;">退出</span>
        </div>
    </div>
    <div class="maxter_topdiv">
        <div id="leftmenu" class="side">
            <div class="sideMenu" id="sideMenu">
               <%-- <a class="linka" href="IndexData.aspx" style="text-decoration:none;" target="i">
                    <span style="background-color: #FFFFFF; color: #0066CC">
                        <i class="fa fa-home" style="color: #0066CC;"></i>
                        首页
                    <em style="color: #0066CC;" class="fa fa-tag"></em>
                    </span>
                </a>
                <span class="mainmenu"><i class="fa fa-user"></i>用户管理<em class="fa fa-angle-right"></em></span>
                <ul>
                    <li><a class="linka" href="AfterUserAdmin.aspx" target="i">用户列表</a></li>
                    <li><a class="linka" href="Role.aspx" target="i">角色管理</a></li>
                </ul>
                <span><i class="fa fa-gift"></i>商品管理<em class="fa fa-angle-right"></em></span>
                <ul>
                    <li><a class="linka" href="GoodsSort.aspx" target="i">商品分类</a></li>
                    <li><a class="linka" href="Goods.aspx" target="i">商品列表</a></li>
                    <li><a class="linka" name="../AfterGoods/GoodsList">商品采购</a></li>
                    <li><a class="linka">商品库存</a></li>
                    <li><a class="linka">商品出库</a></li>
                </ul>
                <span><i class="fa fa-shopping-cart"></i>订单管理 <em class="fa fa-angle-right"></em></span>
                <ul>
                    <li><a href="#" target="_parent">订单列表</a></li>
                </ul>
                <span><i class="fa fa-users"></i>会员中心<em class="fa fa-angle-right"></em></span>
                <ul>
                    <li><a href="#" target="_parent">会员列表</a></li>
                    <li><a href="#" target="_parent">意见反馈</a></li>
                    <li><a href="#" target="_parent">售后跟踪</a></li>
                </ul>
                <span><i class="fa fa-line-chart"></i>营销推广<em class="fa fa-angle-right"></em></span>
                <ul>
                    <li><a href="#" target="_parent">推广列表</a></li>
                    <li><a href="#" target="_parent">推广结算</a></li>
                </ul>
                <span><i class="fa fa-cogs"></i>系统设置<em class="fa fa-angle-right"></em></span>
                <ul>
                    <li><a class="linka" href="AfterRight.aspx" target="i">权限管理</a></li>
                    <li><a href="AfterModule.aspx" target="i">模块列表</a></li>
                    <li><a href="AfterModuleRight.aspx" target="i">模块权限</a></li>
                    <li><a href="AfterRoleModuleRight.aspx" target="i">角色模块权限</a></li>
                    <li><a href="#" target="_parent">广告设置</a></li>
                    <li><a href="#" target="_parent">活动设置</a></li>
                    <li><a href="#" target="_parent">支付方式</a></li>
                    <li><a href="#" target="_parent">管理员</a></li>
                    <li><a href="#" target="_parent">消息推送</a></li>
                    <li><a href="#" target="_parent">物流公司</a></li>

                </ul>--%>
            </div>
        </div>
        <div id="right_content" class="iframe_div">
            <iframe style="width: 100%; border: none; min-height: 900px; height: auto;" src="IndexData.aspx" id="iframe" name="i"></iframe>
        </div>
    </div>
</body>
</html>
