<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomSelideEdit.aspx.cs" Inherits="RPShoping.Web.After.HomSelideEdit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../control/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../CSS/AfterCommon.css" rel="stylesheet" />
    <link href="../../control/zui/zui.min.css" rel="stylesheet" />
    <link href="../../control/zui/zui.uploader.min.css" rel="stylesheet" />
    <link href="../../kindeditor/themes/default/default.css" rel="stylesheet" />

    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../jquery/jquery.query-2.1.7.js"></script>
    <script src="../../jquery/json2.js"></script>
    <script src="../../control/layer/layer.js"></script>
    <script src="../../control/JS/JSCommonForAlter.js"></script>
    <script src="../../control/JS/JSCommon.js"></script>
    <script src="../../jquery/jquery-ui-1.9.2.custom.js"></script>
    <script src="../../control/JS/apicloudsha1.js"></script>
    <script src="../../jquery/JSAdvQuery.js"></script>

    <style type="text/css">
        .imagediv {
            width: 300px;
            height: 200px;
            margin-left: 10px;
            margin-top: 10px;
            float:left;
        }
        .image {
            width:300px;
            height: 200px;
            float: left;
            border: none;
        }
        .closebtn {
            color: red;
            /*position: absolute;*/
            float:left;
            margin-top:10px;
            margin-left: -15px;
            cursor:pointer;
        }
        #mainimagecontent {
            width: 100%;
            height: 210px;
            border: 1px solid #e8e8e8;
            margin-top: 10px;
            line-height: 120px;
        }
        .imagetitle {
            width: 100%;
            height: 25px;
            border-bottom: 1px dashed #e8e8e8;
            line-height: 25px;
            text-indent: 1em;
        }
        .mainimage {
            width: 80%;
            margin: 0 auto;
            height: 210px;
        }
    </style>
    <title>编辑幻灯片</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="datatb_all">
            <div class="datatb_all_mide">
                <div class="datatb_top">
                    <span id="back" class="top_back">
                        <i class="fa fa-reply" aria-hidden="true" style="color: #438eb9; margin-right: 5px; font-size: 22px;"></i>
                    </span>
                    <span class="top_title">编辑幻灯片</span>
                </div>
                <div class="datatb" style="overflow:auto;">
                    <ul style="height: 450px; min-height: 450px; margin-bottom:25px;">
                        <li class="lileft">
                            <span class="titlename">标题：</span><span class="requiredstr">*</span>
                        </li>
                        <li class="liright">
                            <input type="text" class="inputlb" id="title" value="" />
                        </li>
                        <li class="lileft">
                            <span class="titlename">排序序号：</span>
                        </li>
                        <li class="liright">
                            <input type="text" class="inputlb" id="number" value="" />
                        </li>
                        <li class="lileft">
                            <span class="titlename">链接类型：</span>
                        </li>
                        <li class="liright">
                            <select class="inputlb" id="linktype" runat="server">
                                <option value="-1">--请选择--</option>
                                <option value="0">直接跳转</option>
                                <option value="1">商品详情</option>
                                <option value="2">商品列表</option>
                            </select>
                        </li>
                        <li class="lileft">
                            <span class="titlename">链接参数：</span>
                        </li>
                        <li class="liright">
                            <input type="text" class="inputlb" id="linkparm" value="" />
                        </li>
                        <li class="lileft">
                            <span class="titlename">链接内容：</span>
                        </li>
                         <li class="commline">
                            <span class="commlinespan"></span>
                        </li>
                        <li class="commreamrktext">
                            <textarea class="input_area" id="linkcontent"></textarea>
                        </li>
                        <li class="allwidth lileft" style="margin-top: 100px;">
                            <span class="commremark titlename">备注说明</span>
                        </li>
                        <li class="commline">
                            <span class="commlinespan"></span>
                        </li>
                        <li class="commreamrktext">
                            <textarea class="input_area" id="remark"></textarea>
                        </li>
                    </ul>
                    <div id="mainimage" class="mainimage">
                        <div id="imagetitle" class="imagetitle">幻灯片图片</div>
                        <div id="mainimagecontent">
                            <div class="imagediv">
                                <img id="selideimg" class="image" src="../../Image/noimg.png" />
                                <input id="imgkey" type="hidden" class="key" value="" />
                                <i id="closebtn" class="fa fa-times closebtn" aria-hidden="true"></i>
                            </div>
                        </div>
                        <div id='logoUploader' class="uploader" style="width: 100%; margin: 20px auto;">
                            <input type="hidden" name="logo">
                            <div class="uploader-message text-center">
                                <div class="content"></div>
                                <button type="button" class="close">×</button>
                            </div>
                            <div id="tempimage" class="uploader-files file-list file-list-lg" data-drag-placeholder="请拖拽文件到此处"></div>
                            <div class="uploader-actions">
                                <div class="uploader-status pull-right text-muted"></div>
                                <button type="button" class="btn btn-link uploader-btn-browse"><i class="icon icon-plus"></i>选择文件</button>
                                <button type="button" class="btn btn-link uploader-btn-start"><i class="icon icon-cloud-upload"></i>开始上传</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="editbottom">
                <div class="btn_div">
                    <input type="button" class="edit_btn" id="cancel" value="关 闭" />
                    <input type="button" class="edit_btn" id="save" value="保 存" style="margin-left: 50px;" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<script src="../../jquery/qiniu.min.js"></script>
<script src="../../jquery/plupload.full.min.js"></script>
<script src="../../control/zui/zui.min.js"></script>
<script src="../../control/zui/zui.uploader.min.js"></script>

<script src="../../control/JS/JSHomSelideEdit.js"></script>










