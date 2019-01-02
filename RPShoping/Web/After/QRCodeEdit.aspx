<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRCodeEdit.aspx.cs" Inherits="RPShoping.Web.After.QRCodeEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../control/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../CSS/AfterCommon.css" rel="stylesheet" />
    <link href="../../control/zui/zui.min.css" rel="stylesheet" />
    <link href="../../control/zui/zui.uploader.min.css" rel="stylesheet" />
    <%--<link href="../../control/kindeditor/kindeditor.min.css" rel="stylesheet" />--%>
    
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
    <script src="../../kindeditor/kindeditor-all-min.js"></script>
    
    <script src="../../kindeditor/lang/zh_CN.js" charset="utf-8" type="text/javascript"></script>
    <script type="text/javascript">
        KindEditor.ready(
        function (K) {
            editor = K.create('#content', {
                //上传管理
                uploadJson: 'uploader.ashx',
                //文件管理
                //fileManagerJson: 'KindEditor/asp.net/file_manager_json.ashx',
                allowFileManager: true,
                //要取值设置这里 这个函数就是同步KindEditor的值到textarea文本框
                //afterCreate: function () {
                //    var self = this;
                //    K.ctrl(document, 13, function () {
                //        self.sync();
                //        K('form[name=example]')[0].submit();
                //    });
                //    K.ctrl(self.edit.doc, 13, function () {
                //        self.sync();
                //        K('form[name=example]')[0].submit();
                //    });
                //},
                //remove:function()
                //{
                    
                //},
                //上传后执行的回调函数,获取上传图片的路径
                //afterUpload: function (data) {
                //    //alert(data);
                //},
                //同时设置这里  
                afterBlur: function () {
                    this.sync();
                },  
                width:'100%',
                height: '450px;',
                //编辑工具栏
                items: [
                'source', '|', 'undo', 'redo', '|', 'preview', 'print', 'template', 'code', 'cut', 'copy', 'paste',
                'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
                'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen', '/',
                'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
                'flash', 'media', 'insertfile', 'table', 'hr', 'emoticons', 'baidumap', 'pagebreak',
                'anchor', 'link', 'unlink', '|', 'about'
                ]
            });
            //prettyPrint();
        });   
    </script>

    <style type="text/css">
        .imagediv {
            width: 100px;
            height: 120px;
            margin-left: 10px;
            margin-top: 10px;
            float:left;
        }
        .image {
            width: 100px;
            height: 100px;
            float: left;
            border: none;
        }
        .closebtn {
            color: red;
            position: absolute;
            margin-left: -15px;
            cursor:pointer;
        }
        .setslt {
            width: 100px;
            height: 25px;
            border: 1px solid #666666;
            color: #ffffff;
            position: absolute;
            text-align:center;
            line-height:25px;
            font-size:10px;
            background:#666666;
            margin-top:75px;
            cursor:pointer;
            display:none;
            opacity:0.9;
        }
        #mainimagecontent {
            width: 100%;
            height: 120px;
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
            height: 280px;
        }
        .setbtn {
            width:150px;
            height:40px; text-align:center;line-height:40px; font-size:14px;background:#438eb9;color:#ffffff;border:1px solid #438eb9;border-radius:5px;float:left; cursor:pointer;
        }
    </style>
    <title>编辑分享二维码信息</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="datatb_all">
            <div class="datatb_all_mide">
                <div class="datatb_top">
                    <span id="back" class="top_back">
                        <i class="fa fa-reply" aria-hidden="true" style="color: #438eb9; margin-right: 5px; font-size: 22px;"></i>
                    </span>
                    <span class="top_title">编辑分享二维码信息</span>
                </div>
                <div class="datatb">
                    <div id="thumbnail" class="mainimage" style="height:160px;">
                        <div id="thumbnailtitle"  class="imagetitle">安卓二维码</div>
                        <div id="thumbnailcontent">
                            <div class="imagediv">
                                <img id="androidqrcodepath" class="image" src="../../Image/noimg.png" />
                                <input type="hidden" id="androidimgkey" value="" />
                            </div>
                            <div id="androiddelbtn" class="setbtn" style="width:80px;margin-top:70px; margin-left:50px;">删除</div>
                        </div>
                    </div>
                    <div id="iosthumbnail" class="mainimage" style="height:160px;">
                        <div id="iosthumbnailtitle"  class="imagetitle">苹果二维码</div>
                        <div id="iosthumbnailcontent">
                            <div class="imagediv">
                                <img id="iosqrcodepath" class="image" src="../../Image/noimg.png" />
                                <input type="hidden" id="iosimgkey" value="" />
                            </div>
                            <div id="iosdelbtn" class="setbtn" style="width:80px;margin-top:70px; margin-left:50px;">删除</div>
                        </div>
                    </div>
                    <div id="mainimage" class="mainimage">
                        <div id="imagetitle" class="imagetitle">上传图片</div>
                        <div id="mainimagecontent">
                            <div class="imagediv">
                                <img id="minimg" class="image" src="../../Image/noimg.png" />
                                <input type="hidden" id="minkey" class="key" value="" />
                                <i id="closebtn" class="fa fa-times closebtn" aria-hidden="true"></i>
                                <%--<div class="setslt">设置为缩略图</div>--%>
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
                    <div style="width:80%;height:50px; margin:0 auto;">
                        <div id="androidsetben" class="setbtn">设置安卓二维码</div>
                        <div id="iossetben"  class="setbtn" style="margin-left:100px;">设置苹果二维码</div>
                    </div>
                    <ul style="height: 550px; min-height: 550px; margin-bottom:25px;">
                        <li class="lileft">
                            <span class="titlename">安卓是否上线：</span>
                        </li>
                          <li class="liright">
                            <input id="androidisup1" type="radio" checked="checked" name="androidisup" value="否" /><label for="mb_female">否</label>
                            <input id="androidisup" type="radio" name="androidisup" value="是" /><label for="mb_male">是</label>
                        </li>
                        <li class="lileft">
                            <span class="titlename">苹果是否上线：</span>
                        </li>
                        <li class="liright">
                            <input id="iosisup1" type="radio" checked="checked" name="iosisup" value="否" /><label for="mb_female">否</label>
                            <input id="iosisup" type="radio" name="iosisup" value="是" /><label for="mb_male">是</label>
                        </li>
                        <li class="lileft">
                            <span class="titlename">安卓下载地址：</span><span class="requiredstr">*</span>
                        </li>
                        <li class="liright">
                            <input type="text" class="inputlb" id="androidurl" value="" />
                        </li>
                        <li class="lileft">
                            <span class="titlename">苹果下载地址：</span>
                        </li>
                        <li class="liright">
                            <input type="text" class="inputlb" id="iosurl" value="" />
                        </li>
                    </ul>
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
<script src="../../control/JS/JSQRCodeEdit.js"></script>
