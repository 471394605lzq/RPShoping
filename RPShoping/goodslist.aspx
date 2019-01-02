<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="goodslist.aspx.cs" Inherits="RPShoping.goodslist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="keywords" content="RP云购|rp云购|一元云购|一元夺宝|梦想汽车,1元夺宝,1元购,1元购物,1元云购,夺宝,一元购,一元购物,云购" />
<meta name="description" content="RP云购是全新的网购模式,全场汽车、手机、电脑、数码产品等只需1元即可参与一元购物。RP云购网将打造最公正的云购平台,一个只拼人品的云购平台,要相信总有人品爆发时!"/>
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
    <link  href="CSS/iconfont.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="CSS/iconfont.css" />
    <link rel="stylesheet" type="text/css" href="CSS/aui.css" />
    <title>RP云购商品列表-1元云购手机一元夺宝1元秒杀手机</title>
    <style type="text/css">
        body, html {
            padding:0;
            margin:0;
            background:#ffffff;
        }
                .fordiv {
            width: 200px; height: 50px; float: left; margin-left: 10px; margin-top: 15px;line-height:50px; text-align:center;color:#999999;cursor:pointer;
            font-size:16px;
        }
            .fordiv:hover {
                color:#ff5500;
            }
            .fordiv i {
                color:#ff5500;font-size:22px;
            }
                .mark{
	    	float:left;
	    	position: absolute;
	    	left: 5px;
	    	top: 5px;
	    	z-index: 100;
	    	width:30px;
	        height:30px;
	        line-height: 15px;
	        text-align:center;
	    		background: #0099FF;
	        color: #ffffff;
	        font-size:11px;
	        /*transform:rotate(-30deg);*/
	    }
    </style>
    <script src="jquery/jquery.min.js"></script>
    <script src="jquery/jquery.query-2.1.7.js"></script>
    <script src="jquery/json2.js"></script>
    <script src="jquery/vue.js"></script>
    <script src="control/JS/JSCommon.js"></script>
    <script src="control/JS/apicloudsha1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div id="contentdiv">
         <div style="width: 1175px; height: 50px; margin: 0 auto; margin-top: 20px; line-height: 50px; text-align: left;">所有商品分类</div>
        <div style="width: 1175px; height: 250px; margin: 0 auto; border: 1px solid #ff5500;">
            <div class="fordiv" v-for="vo in producttypelist">
                <div onclick="selectitem(this)" v-bind:id="vo.id">
                    <i v-bind:class="vo.PS_icon"></i>
                    {{vo.PS_Name}}
                </div>
            </div>
        </div>
        <div class="content" style="width: 1175px; height: auto;margin: 0 auto;" id="likelist">
			<ul style="list-style: none;padding: 0px; margin: 0px;">
                <%--class="aui-border-l"--%>
				<li  style="float: left;width:23%; height:340px;border:1px solid #e8e8e8;margin-left:1%;margin-top:15px; margin-bottom:20px;" v-for="(vo,index) in likelist">
				<div style="width: 100%; height: 100%;position:relative; float: left;" >
					<div v-if="vo.ptype=='五元商品'||vo.ptype=='十元商品'" class="mark">{{vo.ptype}}</div>
					<div v-bind:id="vo.id+','+vo.P_ID.id" onclick="togoodsdetail(this)">
                        <img style="width: 50%; margin: 15px auto; height: 150px;" v-bind:alt="vo.P_ID.P_Name+'一元云购/夺宝'" v-bind:src="vo.P_ID.p_thumbnail">
                        <div style="width: 90%; height: 40px; margin: 0 auto; font-size: 11px; color: #666666;">
                            {{ vo.P_ID.P_Name }}
                        </div>
                    </div>
					<div class="aui-progress aui-progress-xxs" style="width: 90%; margin: 0 auto;margin-top:10px;">
						<!-- <div class="aui-progress-bar" style="width: 60%;background: #ff9900;"></div> -->
						<div class="aui-progress-bar" v-bind:style="vo.styleObject" ></div>
					</div>
					<div style="width: 90%; height: 40px;margin: 0 auto;">
						<span style="display: block; width: 30%; height: 40px; line-height:15px; color: #999999; float: left;">
							<span style="display: block; width: 100%; float: left;text-align: left;font-size:10px;color: #ff6600;">{{vo.AlreadyNumber}}</span>
							<span style="display: block; width: 100%; float: left;text-align: left;font-size:10px;">已参与</span>
						</span>
						<span style="display: block; width: 30%; height: 40px; line-height:15px; color: #999999; float: left; ">
							<span style="display: block; width: 100%; float: left;text-align: center;font-size:10px;">{{vo.countnumber}}</span>
							<span style="display: block; width: 100%; float: left;text-align: center;font-size:10px;">需总人次</span>
						</span>
						<span style="display: block; width: 30%; height: 40px; line-height:15px; color: #999999;float: right; ">
							<span style="display: block; width: 100%; float: left;text-align: right;font-size:10px; color: #1296db;">{{vo.SurplusNumber}}</span>
							<span style="display: block; width: 100%; float: left;text-align: right;font-size:10px;">剩余</span>
						</span>
					</div>
					<div style="width: 90%; height: 40px; margin: 0 auto;margin-top:10px;">
                        <a href="buy.aspx" target="_blank">
                            <div  style="width: 90%; height: 30px; cursor: pointer; margin: 0 auto; border: 1px solid #ff6600; border-radius: 15px; color: #ff6600; font-size: 12px; text-align: center; line-height: 30px;">
                                立即云购
                            </div>
                        </a>
<%--						<div tapmode="hover" v-bind:id="vo.id+','+vo.P_ID.id+','+vo.P_ID.P_Type" onclick="addcart(this)" class="titlefont" style="background-color: #ff6600; width: 30px; height:30px; line-height:30px; float: right;">
							<i class="iconfont icon-gouwuchetianjia" style="font-size: 18px;"></i>
						</div>--%>
					</div>
					</div>
				</li>
			</ul>
		</div>
        <div id="loadmore" onclick="loadmore()" style="width:100%;height:50px; line-height:50px; text-align:center;cursor:pointer;clear:both;">加载更多...</div>
        <div id="loadmoreno"  style="display:none; width:100%;height:50px; line-height:50px; text-align:center;clear:both;">无更多数据可加载...</div>
     </div>
    </form>
</body>
</html>
<script src="control/JS/JSgoodslist.js"></script>