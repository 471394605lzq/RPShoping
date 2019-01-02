<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Share.aspx.cs" Inherits="RPShoping.Share" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="keywords" content="RP云购|rp云购|一元云购|一元夺宝|梦想汽车,1元夺宝,1元购,1元购物,1元云购,夺宝,一元购,一元购物,云购" />
<meta name="description" content="RP云购是全新的网购模式,全场汽车、手机、电脑、数码产品等只需1元即可参与一元购物。RP云购网将打造最公正的云购平台,一个只拼人品的云购平台,要相信总有人品爆发时!"/>
    <title>RP云购-1元云购手机一元夺宝1元秒杀手机</title>
    <link  href="CSS/iconfont.css" rel="stylesheet" />
    <style type="text/css">
        body, html {
            padding:0;
            margin:0;
        }
        a:link,a:visited{
         text-decoration:none;  /*超链接无下划线*/
        }
    </style>
    <script src="jquery/jquery.min.js"></script>
    <script src="jquery/jquery.query-2.1.7.js"></script>
    <script src="jquery/json2.js"></script>
    <script src="control/JS/JSCommon.js"></script>
    <script src="control/JS/apicloudsha1.js"></script>
    <script src="control/JS/JSshare.js"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div style="width:95%;height:auto; margin:0 auto; margin-top:30px;">
            <div style="width:100%; height:50px; line-height:50px; font-size:20px; color:#333333; border-bottom:1px solid #e8e8e8;">下载手机app、随时随地、想购就购</div>
            <div style="width:100%; height:150px; margin-top:30px;">
                <div style="width:100%; height:150px; float:left;">
                    <div style="width:100%; height:150px;">
                        <div id="iosshow" style="background-color:#5489D2; width:80%;height:150px;margin:0 auto;margin-top:20px; text-align:center;">
                            <a id="iosurl" href="">
                                <i class="iconfont icon-iphone" style=" font-size: 80px;color:#ffffff;margin-top:50px;margin-left:5px;"></i>
                                <span style="color:#ffffff;height: 150px;line-height: 150px;margin-right: 25px;font-size:30px;">点此下载 iPhone 版</span>
                            </a>
                        </div>
                        <div id="iosisnoshow" style="display:none; background-color:#5489D2; width:80%;height:150px;margin:0 auto;margin-top:20px; text-align:center;">
                            <i class="iconfont icon-iphone" style=" font-size: 80px;color:#ffffff;margin-top:50px;margin-left:5px;"></i>
                            <span style="color:#ffffff;height: 150px;line-height: 150px;margin-right: 25px;font-size:30px;">敬请期待！</span>
                        </div>
                    </div>
                </div>
<%--                <div style="width:70%; height:150px;float:right;">
                    <div style="width: 100%; height: 150px;">
                        <img id="iosimg" style="width:150px;height:150px;margin-left:150px;" src="" />
                        <div id="iostempdiv" style="display:none; width:100%;height:50px;border:1px solid #e8e8e8;margin-left: 50px;color:#666666;font-size:14px; text-align:center; line-height:50px;">点击下载</div>
                    </div>
                </div>--%>
            </div>
            <div style="width: 100%; height: 150px; margin-top:50px; ">
                <div style="width:100%; height:150px; float:left;">
                    <div style="width:100%; height:150px;">
                        <div id="androidshow" style="background-color:#51C051; width:80%;height:150px;margin:0 auto;margin-top:20px;text-align:center;">
                           <a id="androidurl" href="">
                            <i class="iconfont icon-anzhuo1" style="font-size: 80px;color:#ffffff;margin-top:50px;margin-left:5px;"></i>
                            <span style="color:#ffffff;height: 150px;line-height: 150px;margin-right: 25px;font-size:30px;">点此下载 Android 版</span>
                           </a>
                        </div>
                        <div id="androidisnoshow" style="display:none; background-color:#51C051; width:80%;height:150px;margin:0 auto;margin-top:20px;text-align:center;">
                            <i class="iconfont icon-anzhuo1" style="font-size: 80px;color:#ffffff;margin-top:50px;margin-left:5px;"></i>
                            <span style="color:#ffffff;height: 150px;line-height: 150px;margin-right: 25px;font-size:30px;">敬请期待！</span>
                        </div>
                    </div>
                </div>
<%--                <div style="width: 70%; height: 150px; float: right;">
                    <div style="width: 100%; height: 150px;">
                        <img id="androidimg" style="width:150px;height:150px;margin-left: 150px;" src="" />
                        <div id="androidtempdiv" style="display:none;width:150px;height:150px;border:1px solid #e8e8e8;margin-left: 150px;color:#666666;font-size:14px; text-align:center; line-height:150px;">敬请期待！</div>
                    </div>
                </div>--%>
            </div>
        </div>
    </form>
</body>
</html>
