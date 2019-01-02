<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buy.aspx.cs" Inherits="RPShoping.buy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="keywords" content="RP云购|rp云购|一元云购|一元夺宝|梦想汽车,1元夺宝,1元购,1元购物,1元云购,夺宝,一元购,云购网|云购全球" />
<meta name="description" content="RP云购是全新的网购模式,全场汽车、手机、电脑、数码产品等只需1元即可参与一元购物。RP云购网将打造最公正的云购平台,一个只拼人品的云购平台,要相信总有人品爆发时!"/>
        <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
    <link  href="CSS/iconfont.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="CSS/iconfont.css" />
    <link rel="stylesheet" type="text/css" href="CSS/aui.css" />
    <title>RP云购-1元云购手机一元夺宝1元秒杀手机</title>
        <script src="jquery/jquery.min.js"></script>
    <script src="jquery/jquery.query-2.1.7.js"></script>
    <script src="jquery/json2.js"></script>
    <script src="jquery/vue.js"></script>
    <script src="control/JS/JSCommon.js"></script>
    <script src="control/JS/apicloudsha1.js"></script>
    <style>
        body, html {
            padding:0;
            margin:0;
            background:#ffffff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 1000px; height: 600px; margin: 50px auto; clear: both; background: #ffffff;">
            <div style="width: 100%; height: 50px; line-height: 60px; color: #333333; font-size: 22px; text-align: left;">
                <h1>抱歉我们<strong>rp云购</strong>只做移动端</h1>
            </div>
            <div style="width: 40%; height: 100px; line-height: 30px; color: #666666; font-size: 18px; text-align: left; margin-top: 15px;">
               抱歉！<strong>rp云购</strong>目前只做移动端，因为这样您就会随时随地发现惊喜！
            </div>
            <div style="width: 600px; height: 400px; margin: 0 auto;">
                <img src="Image/app.jpg" style="width: 600px; height: 400px;" alt="一元云购|rp云购|云购全球" />
            </div>
        </div>
        <div style="width: 1000px; min-width: 600px; height: 600px; margin: 0 auto; margin-top: 30px; background: #ffffff;">
            <div style="width: 100%; height: 50px; line-height: 50px; font-size: 20px; color: #333333; border-bottom: 1px solid #e8e8e8;">
                <h1>下载<strong>rp云购网</strong>官方手机app、随时随地、想购就购</h1></div>
            <div style="width: 100%; height: 250px; margin-top: 30px;">
                <div style="width: 30%; height: 150px; float: left;">
                    <div style="width: 100%; height: 150px;">
                        <div style="background-color: #5489D2; width: 190px; height: 50px; margin-top: 20px;">
                            <i class="iconfont icon-iphone" style="float: left; font-size: 40px; color: #ffffff; margin-top: 5px; margin-left: 5px;"></i>
                            <span style="float: right; color: #ffffff; height: 50px; line-height: 50px; margin-right: 25px; font-size: 16px;">iPhone 版下载</span>
                        </div>
                    </div>
                </div>
                <div style="width: 70%; height: 150px; float: right;">
                    <div style="width: 100%; height: 150px;">
                        <img id="iosimg" style="width: 150px; height: 150px; margin-left: 150px;" alt="一元云购安卓版" src="" />
                        <div id="iostempdiv" style="display: none; width: 150px; height: 150px; border: 1px solid #e8e8e8; margin-left: 150px; color: #666666; font-size: 14px; text-align: center; line-height: 150px;">敬请期待！</div>
                    </div>
                </div>
            </div>
            <div style="width: 100%; height: 250px;">
                <div style="width: 30%; height: 150px; float: left;">
                    <div style="width: 100%; height: 150px;">
                        <div style="background-color: #51C051; width: 190px; height: 50px; margin-top: 20px;">
                            <i class="iconfont icon-anzhuo1" style="float: left; font-size: 40px; color: #ffffff; margin-top: 5px; margin-left: 5px;"></i>
                            <span style="float: right; color: #ffffff; height: 50px; line-height: 50px; margin-right: 25px; font-size: 16px;">Android 版下载</span>
                        </div>
                    </div>
                </div>
                <div style="width: 70%; height: 150px; float: right;">
                    <div style="width: 100%; height: 150px;">
                        <img id="androidimg" style="width: 150px; height: 150px; margin-left: 150px;" alt="一元云购ios版" src="" />
                        <div id="androidtempdiv" style="display: none; width: 150px; height: 150px; border: 1px solid #e8e8e8; margin-left: 150px; color: #666666; font-size: 14px; text-align: center; line-height: 150px;">敬请期待！</div>
                    </div>
                </div>
            </div>
        </div>
        <div style="width: 1000px; height: 760px; margin: 50px auto; clear: both;">
            <div style="width: 100%; height: 50px; line-height: 60px; color: #333333; font-size: 22px; text-align: left;">
                <h1><strong>RP云购</strong>商家就在您身边</h1></div>
            <div style="width: 40%; height: 100px; line-height: 30px; color: #666666; font-size: 18px; text-align: left; margin-top: 15px;">
                我们<strong>RP云购</strong>会选择部分城市开展代理分点，大量高质量商户就在您身边！让我们更接近您
            </div>
            <div style="width: 100%; height: 600px;">
                <img src="Image/0002.png" alt="一元云购|rp云购|云购全球" />
            </div>
        </div>
    </form>
</body>
</html>
<script src="control/JS/JSbuy.js"></script>
