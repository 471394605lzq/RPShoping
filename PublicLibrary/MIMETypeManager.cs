using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary
{
  public  class MIMETypeManager
    {
        private static Dictionary<string, string> mimeDict;

        static MIMETypeManager()
        {
            mimeDict = new Dictionary<string, string>();
            mimeDict.Add(".*", "application/octet-stream");
            mimeDict.Add(".323", "text/h323");
            mimeDict.Add(".acx", "application/internet-property-stream");
            mimeDict.Add(".ai", "application/postscript");
            mimeDict.Add(".aif", "audio/x-aiff");
            mimeDict.Add(".aifc", "audio/x-aiff");
            mimeDict.Add(".aiff", "audio/x-aiff");
            mimeDict.Add(".asf", "video/x-ms-asf");
            mimeDict.Add(".asr", "video/x-ms-asf");
            mimeDict.Add(".asx", "video/x-ms-asf");
            mimeDict.Add(".au", "audio/basic");
            mimeDict.Add(".avi", "video/x-msvideo");
            mimeDict.Add(".axs", "application/olescript");
            mimeDict.Add(".bas", "text/plain");
            mimeDict.Add(".bcpio", "application/x-bcpio");
            mimeDict.Add(".bin", "application/octet-stream");
            mimeDict.Add(".bmp", "image/bmp");
            mimeDict.Add(".c", "text/plain");
            mimeDict.Add(".cat", "application/vnd.ms-pkiseccat");
            mimeDict.Add(".cdf", "application/x-cdf");
            mimeDict.Add(".cer", "application/x-x509-ca-cert");
            mimeDict.Add(".class", "application/octet-stream");
            mimeDict.Add(".clp", "application/x-msclip");
            mimeDict.Add(".cmx", "image/x-cmx");
            mimeDict.Add(".cod", "image/cis-cod");
            mimeDict.Add(".cpio", "application/x-cpio");
            mimeDict.Add(".crd", "application/x-mscardfile");
            mimeDict.Add(".crl", "application/pkix-crl");
            mimeDict.Add(".crt", "application/x-x509-ca-cert");
            mimeDict.Add(".csh", "application/x-csh");
            mimeDict.Add(".css", "text/css");
            mimeDict.Add(".dcr", "application/x-director");
            mimeDict.Add(".der", "application/x-x509-ca-cert");
            mimeDict.Add(".dir", "application/x-director");
            mimeDict.Add(".dll", "application/x-msdownload");
            mimeDict.Add(".dms", "application/octet-stream");
            mimeDict.Add(".doc", "application/msword");
            mimeDict.Add(".dot", "application/msword");
            mimeDict.Add(".dvi", "application/x-dvi");
            mimeDict.Add(".dxr", "application/x-director");
            mimeDict.Add(".eps", "application/postscript");
            mimeDict.Add(".etx", "text/x-setext");
            mimeDict.Add(".evy", "application/envoy");
            mimeDict.Add(".exe", "application/octet-stream");
            mimeDict.Add(".fif", "application/fractals");
            mimeDict.Add(".flr", "x-world/x-vrml");
            mimeDict.Add(".gif", "image/gif");
            mimeDict.Add(".gtar", "application/x-gtar");
            mimeDict.Add(".gz", "application/x-gzip");
            mimeDict.Add(".h", "text/plain");
            mimeDict.Add(".hdf", "application/x-hdf");
            mimeDict.Add(".hlp", "application/winhlp");
            mimeDict.Add(".hqx", "application/mac-binhex40");
            mimeDict.Add(".hta", "application/hta");
            mimeDict.Add(".htc", "text/x-component");
            mimeDict.Add(".htm", "text/html");
            mimeDict.Add(".html", "text/html");
            mimeDict.Add(".htt", "text/webviewhtml");
            mimeDict.Add(".ico", "image/x-icon");
            mimeDict.Add(".ief", "image/ief");
            mimeDict.Add(".iii", "application/x-iphone");
            mimeDict.Add(".ins", "application/x-internet-signup");
            mimeDict.Add(".isp", "application/x-internet-signup");
            mimeDict.Add(".jfif", "image/pipeg");
            mimeDict.Add(".jpe", "image/jpeg");
            mimeDict.Add(".jpeg", "image/jpeg");
            mimeDict.Add(".jpg", "image/jpeg");
            mimeDict.Add(".js", "application/x-javascript");
            mimeDict.Add(".latex", "application/x-latex");
            mimeDict.Add(".lha", "application/octet-stream");
            mimeDict.Add(".lsf", "video/x-la-asf");
            mimeDict.Add(".lsx", "video/x-la-asf");
            mimeDict.Add(".lzh", "application/octet-stream");
            mimeDict.Add(".m13", "application/x-msmediaview");
            mimeDict.Add(".m14", "application/x-msmediaview");
            mimeDict.Add(".m3u", "audio/x-mpegurl");
            mimeDict.Add(".man", "application/x-troff-man");
            mimeDict.Add(".mdb", "application/x-msaccess");
            mimeDict.Add(".me", "application/x-troff-me");
            mimeDict.Add(".mht", "message/rfc822");
            mimeDict.Add(".mhtml", "message/rfc822");
            mimeDict.Add(".mid", "audio/mid");
            mimeDict.Add(".mny", "application/x-msmoney");
            mimeDict.Add(".mov", "video/quicktime");
            mimeDict.Add(".movie", "video/x-sgi-movie");
            mimeDict.Add(".mp2", "video/mpeg");
            mimeDict.Add(".mp3", "audio/mpeg");
            mimeDict.Add(".mpa", "video/mpeg");
            mimeDict.Add(".mpe", "video/mpeg");
            mimeDict.Add(".mpeg", "video/mpeg");
            mimeDict.Add(".mpg", "video/mpeg");
            mimeDict.Add(".mpp", "application/vnd.ms-project");
            mimeDict.Add(".mpv2", "video/mpeg");
            mimeDict.Add(".ms", "application/x-troff-ms");
            mimeDict.Add(".mvb", "application/x-msmediaview");
            mimeDict.Add(".nws", "message/rfc822");
            mimeDict.Add(".oda", "application/oda");
            mimeDict.Add(".p10", "application/pkcs10");
            mimeDict.Add(".p12", "application/x-pkcs12");
            mimeDict.Add(".p7b", "application/x-pkcs7-certificates");
            mimeDict.Add(".p7c", "application/x-pkcs7-mime");
            mimeDict.Add(".p7m", "application/x-pkcs7-mime");
            mimeDict.Add(".p7r", "application/x-pkcs7-certreqresp");
            mimeDict.Add(".p7s", "application/x-pkcs7-signature");
            mimeDict.Add(".pbm", "image/x-portable-bitmap");
            mimeDict.Add(".pdf", "application/pdf");
            mimeDict.Add(".pfx", "application/x-pkcs12");
            mimeDict.Add(".pgm", "image/x-portable-graymap");
            mimeDict.Add(".pko", "application/ynd.ms-pkipko");
            mimeDict.Add(".pma", "application/x-perfmon");
            mimeDict.Add(".pmc", "application/x-perfmon");
            mimeDict.Add(".pml", "application/x-perfmon");
            mimeDict.Add(".pmr", "application/x-perfmon");
            mimeDict.Add(".pmw", "application/x-perfmon");
            mimeDict.Add(".pnm", "image/x-portable-anymap");
            mimeDict.Add(".pot,", "application/vnd.ms-powerpoint");
            mimeDict.Add(".ppm", "image/x-portable-pixmap");
            mimeDict.Add(".pps", "application/vnd.ms-powerpoint");
            mimeDict.Add(".ppt", "application/vnd.ms-powerpoint");
            mimeDict.Add(".prf", "application/pics-rules");
            mimeDict.Add(".ps", "application/postscript");
            mimeDict.Add(".pub", "application/x-mspublisher");
            mimeDict.Add(".qt", "video/quicktime");
            mimeDict.Add(".ra", "audio/x-pn-realaudio");
            mimeDict.Add(".ram", "audio/x-pn-realaudio");
            mimeDict.Add(".ras", "image/x-cmu-raster");
            mimeDict.Add(".rgb", "image/x-rgb");
            mimeDict.Add(".rmi", "audio/mid");
            mimeDict.Add(".roff", "application/x-troff");
            mimeDict.Add(".rtf", "application/rtf");
            mimeDict.Add(".rtx", "text/richtext");
            mimeDict.Add(".scd", "application/x-msschedule");
            mimeDict.Add(".sct", "text/scriptlet");
            mimeDict.Add(".setpay", "application/set-payment-initiation");
            mimeDict.Add(".setreg", "application/set-registration-initiation");
            mimeDict.Add(".sh", "application/x-sh");
            mimeDict.Add(".shar", "application/x-shar");
            mimeDict.Add(".vsit", "application/x-stuffit");
            mimeDict.Add(".snd", "audio/basic");
            mimeDict.Add(".spc", "application/x-pkcs7-certificates");
            mimeDict.Add(".spl", "application/futuresplash");
            mimeDict.Add(".src", "application/x-wais-source");
            mimeDict.Add(".sst", "application/vnd.ms-pkicertstore");
            mimeDict.Add(".stl", "application/vnd.ms-pkistl");
            mimeDict.Add(".stm", "text/html");
            mimeDict.Add(".svg", "image/svg+xml");
            mimeDict.Add(".sv4cpio", "application/x-sv4cpio");
            mimeDict.Add(".sv4crc", "application/x-sv4crc");
            mimeDict.Add(".swf", "application/x-shockwave-flash");
            mimeDict.Add(".vt", "application/x-troff");
            mimeDict.Add(".tar", "application/x-tar");
            mimeDict.Add(".tcl", "application/x-tcl");
            mimeDict.Add(".tex", "application/x-tex");
            mimeDict.Add(".texi", "application/x-texinfo");
            mimeDict.Add(".texinfo", "application/x-texinfo");
            mimeDict.Add(".tgz", "application/x-compressed");
            mimeDict.Add(".tif", "image/tiff");
            mimeDict.Add(".vtiff", "image/tiff");
            mimeDict.Add(".tr", "application/x-troff");
            mimeDict.Add(".trm", "application/x-msterminal");
            mimeDict.Add(".tsv", "text/tab-separated-values");
            mimeDict.Add(".txt", "text/plain");
            mimeDict.Add(".uls", "text/iuls");
            mimeDict.Add(".ustar", "application/x-ustar");
            mimeDict.Add(".vcf", "text/x-vcard");
            mimeDict.Add(".vrml", "x-world/x-vrml");
            mimeDict.Add(".wav", "audio/x-wav");
            mimeDict.Add(".wcm", "application/vnd.ms-works");
            mimeDict.Add(".wdb", "application/vnd.ms-works");
            mimeDict.Add(".wks", "application/vnd.ms-works");
            mimeDict.Add(".wmf", "application/x-msmetafile");
            mimeDict.Add(".wps", "application/vnd.ms-works");
            mimeDict.Add(".wri", "application/x-mswrite");
            mimeDict.Add(".wrl", "x-world/x-vrml");
            mimeDict.Add(".wrz", "x-world/x-vrml");
            mimeDict.Add(".xaf", "x-world/x-vrml");
            mimeDict.Add(".xbm", "image/x-xbitmap");
            mimeDict.Add(".xla", "application/vnd.ms-excel");
            mimeDict.Add(".xlc", "application/vnd.ms-excel");
            mimeDict.Add(".xlm", "application/vnd.ms-excel");
            mimeDict.Add(".xls", "application/vnd.ms-excel");
            mimeDict.Add(".xlt", "application/vnd.ms-excel");
            mimeDict.Add(".xlw", "application/vnd.ms-excel");
            mimeDict.Add(".xof", "x-world/x-vrml");
            mimeDict.Add(".xpm", "image/x-xpixmap");
            mimeDict.Add(".xwd", "image/x-xwindowdump");
            mimeDict.Add(".z", "application/x-compress");
            mimeDict.Add(".zip", "application/zip");
        }

        //根據擴展名獲取content-type
        public static string GetContentType(string extName)
        {
            string value = "";
            mimeDict.TryGetValue(extName.ToLower(), out value);
            if (string.IsNullOrWhiteSpace(value))
                value = mimeDict[".*"];
            return value;
        }

        //根據content-type獲取擴展名
        public static string GetExtName(string contentType)
        {
            var extName = "";

            var kvs = mimeDict.Where(p => p.Value.ToLower() == contentType.ToLower());
            if (kvs.Count() > 0) extName = kvs.First().Key;

            if (extName == ".*") extName = "";

            return extName;
        }
    }
}
