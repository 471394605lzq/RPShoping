using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections;
using DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;
using System.Reflection;

namespace PublicLibrary
{
    public class SetUnit
    {
        /// <summary>
        /// 获取唯一的值
        /// </summary>
        /// <returns></returns>
        public static string GetGuId()
        {
            string Guid = System.Guid.NewGuid().ToString().Replace("-", "");
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //string TiSpan = Convert.ToInt64(ts.TotalSeconds).ToString();
            return Guid;
        }


        //
        /// <summary>
        /// 获取已上传文件的原文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string RestoreUploadFilename(string filename)
        {
            if (filename.IndexOf(",") > 0)
            {
                return "文档.zip";
            }
            else
            {
                var point = filename.LastIndexOf("_");
                if (point > 0)
                {
                    return filename.Substring(0, point) + Path.GetExtension(filename);
                }
                else
                {
                    return filename;
                }
            }
        }

        /// <summary>
        /// 下载指定的文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="response"></param>
        /// <param name="request"></param>
        public static void DownloadFile(string filename, HttpResponse response, HttpRequest request)
        {
            DownloadFile(filename, response, request, null);
        }
        public static void DownloadFile(string filename, HttpResponse response, HttpRequest request, string showFilename)
        {
            filename = filename.Trim(',');
            var files = filename.Split(',');
            if (files.Length > 1)
            {
                filename = CompressDocument(files, showFilename);
            }
            if (!File.Exists(filename)) return;

            long requestPoint = 0;
            var extFilename = Path.GetExtension(filename);
            var fn = Path.GetFileName(filename);
            if (string.IsNullOrWhiteSpace(extFilename)) extFilename = ".*";
            if (string.IsNullOrWhiteSpace(showFilename)) showFilename = fn;
            var contentType = MIMETypeManager.GetContentType(extFilename);
            try
            {
                using (var fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var size = fs.Length;

                    response.Clear();
                    response.Buffer = false;
                    if (request.Headers["Range"] != null)
                    {
                        response.StatusCode = 206;
                        requestPoint = long.Parse(request.Headers["Range"].Replace("bytes=", "").Split('-')[0].Trim());
                        response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", requestPoint, size - 1, size));
                    }

                    response.ContentType = contentType;
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(request.ContentEncoding.GetBytes(showFilename)).Replace("+", "%20"));
                    response.AppendHeader("Content-Length", (size - requestPoint).ToString());

                    int buff = 1 * 1024, currBuff = buff;
                    byte[] sbuff = new byte[buff];
                    long i = requestPoint;
                    fs.Seek(i, SeekOrigin.Begin);
                    while (i < size)
                    {
                        if ((i + buff) > size)
                            currBuff = (int)(size - i);

                        if (response.IsClientConnected)
                        {
                            fs.Read(sbuff, 0, buff);
                            response.OutputStream.Write(sbuff, 0, currBuff);
                            response.Flush();
                        }
                        else
                        {
                            break;
                        }
                        i += buff;
                    }
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                response.End();
            }
        }

        /// <summary>
        /// 返回组装后的电话号码
        /// </summary>
        /// <param name="Phone"></param>
        public static string GetNewPhone(string Phone, HttpContext context)
        {
            try
            {
                //Phone = Phone.TrimStart('8','6').Trim();
                string Code = context.Session["regionalcornet"].ToString();//区号
                string addnumber = context.Session["mumber"].ToString();//号码前加拨
                string longaddnumber = context.Session["longmumber"].ToString();//长途加拨
                string Callnumber = Phone;/*号码*/
                string AreaCode = string.Empty;//区号
                string ProvinceCity = "未知";
                string Province = "";
                string City = "";
                if (isMobile(Phone))//手机号码
                {
                    string response = GetWeb(Phone);
                    var token = JsonConvert.DeserializeObject(response) as JToken;
                    if (token != null)
                    {
                        token = token.SelectToken("data") as JToken;
                        if (token != null)
                        {
                            AreaCode = token.SelectToken("areacode").ToString();
                            Province = token.SelectToken("province").ToString();
                            City = token.SelectToken("city").ToString();
                        }
                    }



                    //string phoneinfo = GetWeb(Phone).Trim().TrimStart('{').TrimEnd('}');
                    //string[] phoneinfostr = phoneinfo.Split(',');
                    //List<string> liststr = new List<string>(phoneinfostr);
                    //string phonestr = liststr.Where(p => p.Contains("AreaCode")).FirstOrDefault();
                    //string Province = liststr.Where(p => p.Contains("Province")).FirstOrDefault();
                    //string City = liststr.Where(p => p.Contains("City")).FirstOrDefault();

                    //if (phonestr != "")
                    //    AreaCode = phonestr.Split(':')[1].Trim('"');
                    //if (Province != "")
                    //    Province = Province.Split(':')[1].Trim('"');
                    //if (City != "")
                    //    City = City.Split(':')[1].Trim('"');

                    if (AreaCode == Code)//市内手机 加拨号+号码
                        Callnumber = addnumber + Callnumber;
                    else//国内长途 
                        Callnumber = addnumber + longaddnumber + Callnumber;
                    ProvinceCity = Province + "-" + City;
                }
                else//座机
                {

                    string[] CallStr = Callnumber.Split(' ');
                    if (CallStr.Length == 1)//加拨号码+号码
                    {
                        Callnumber = addnumber + CallStr[0];
                    }
                    else if (CallStr.Length == 2)
                    {
                        AreaCode = CallStr[0];
                        Callnumber = CallStr[1];
                        if (AreaCode == Code)//市内电话
                            Callnumber = addnumber + Callnumber;
                        else//加拨号码+区号+号码
                            Callnumber = addnumber + AreaCode + Callnumber;
                    }
                    else if (CallStr.Length == 3)
                    {
                        string CallCodeS = CallStr[0];//国际区号
                        AreaCode = CallStr[1];
                        Callnumber = CallStr[2];
                        if (AreaCode == Code)//市内电话
                            Callnumber = addnumber + Callnumber;
                        else
                            Callnumber = addnumber + AreaCode + Callnumber;
                    }
                    string sql = string.Format("SELECT region FROM  dbo.CRM_phoneBase WHERE areacode='{0}'", AreaCode);
                    //SqlDataReader reader = DBHelper.GetExecuteReader(sql);
                    //if (reader.Read())
                    //    ProvinceCity = reader["region"] != DBNull.Value ? reader["region"].ToString() : "未知";
                    //reader.Close();

                }
                return Callnumber + ":" + ProvinceCity;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 压缩指定路径的文档
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>返回文件本地路路径</returns>
        public static string CompressDocument(string[] paths, string filename)
        {
            var path = HttpContext.Current.Server.MapPath("/crm/crm_uploadfiles/document/ziptemp");
            path = Path.Combine(path, filename);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                ZipOutputStream zs = new ZipOutputStream(fs);
                foreach (var file in paths)
                {
                    ZipEntry zip = new ZipEntry(Path.GetFileName(file));
                    zs.PutNextEntry(zip);
                    Write(file, ref zs);
                }
                zs.Flush();
                fs.Flush();
                zs.Close();
                fs.Close();

                return path;
            }
        }
        private static void Write(string filename, ref ZipOutputStream zs)
        {
            if (!File.Exists(filename)) { return; }

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var len = 0;
                var bytes = new byte[1024];
                while ((len = fs.Read(bytes, 0, bytes.Length)) > 0)
                {
                    zs.Write(bytes, 0, len);
                }
                fs.Close();
            }
        }
        /// <summary>
        /// 远程获取号码信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private static string GetWeb(string phone)
        {
            var url = "http://apis.baidu.com/chazhao/mobilesearch/phonesearch";
            var param = string.Format("phone={0}", phone);
            string strURL = url + '?' + param;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", "5a2e899e25fbc0ba7f2c236c5767ed8d");
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader reader = new StreamReader(s, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 手机
        /// </summary>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        public static bool isMobile(string mobiles)
        {
            return Regex.IsMatch(mobiles, "^((13[0-9])|(14[0-9])|(15[0-9])|(18[0-9]))\\d{8}$");
        }




        #region  缩略图
        /// <summary> 
        /// 根据路径读取文件，支持远程文件，本地文件 
        /// </summary> 
        /// <param name="path"></param> 
        /// <returns></returns> 
        private static System.Drawing.Image GetImage(string path)
        {
            if (path.StartsWith("http"))
            {
                System.Net.WebRequest request = System.Net.WebRequest.Create(path);
                request.Timeout = 10000;
                System.Net.HttpWebResponse httpresponse = (System.Net.HttpWebResponse)request.GetResponse();
                Stream s = httpresponse.GetResponseStream();

                return System.Drawing.Image.FromStream(s);
            }
            else
            {
                return System.Drawing.Image.FromFile(path);
            }
        }
        /// <summary>创建规定大小的图像 
        /// </summary> 
        /// <param name="oPath">源图像绝对路径</param> 
        /// <param name="tPath">生成图像绝对路径</param> 
        /// <param name="width">生成图像的宽度</param> 
        /// <param name="height">生成图像的高度</param> 
        public static void CreateImageOutput(int width, int height, string oPath, string tPath)
        {
            Bitmap originalBmp = null;// new Bitmap(oPath); 
            originalBmp = new Bitmap(GetImage(oPath));
            // 源图像在新图像中的位置 
            int left, top;


            if (originalBmp.Width <= width && originalBmp.Height <= height)
            {
                // 原图像的宽度和高度都小于生成的图片大小 
                left = (int)Math.Round((decimal)(width - originalBmp.Width) / 2);
                top = (int)Math.Round((decimal)(height - originalBmp.Height) / 2);


                // 最终生成的图像 
                Bitmap bmpOut = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(bmpOut))
                {
                    // 设置高质量插值法 
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 清空画布并以白色背景色填充 
                    graphics.Clear(Color.White);
                    // 把源图画到新的画布上 
                    graphics.DrawImage(originalBmp, left, top);
                }
                bmpOut.Save(tPath);//保存为文件，tpath 为要保存的路径 
                bmpOut.Dispose();

            }



            // 新图片的宽度和高度，如400*200的图像，想要生成160*120的图且不变形， 
            // 那么生成的图像应该是160*80，然后再把160*80的图像画到160*120的画布上 
            int newWidth, newHeight;
            if (width * originalBmp.Height < height * originalBmp.Width)
            {
                newWidth = width;
                newHeight = (int)Math.Round((decimal)(originalBmp.Height * width / originalBmp.Width));
                // 缩放成宽度跟预定义的宽度相同的，即left=0，计算top 
                left = 0;
                top = (int)Math.Round((decimal)(height - newHeight) / 2);
            }
            else
            {
                newWidth = (int)Math.Round((decimal)(originalBmp.Width * height / originalBmp.Height));
                newHeight = height;
                // 缩放成高度跟预定义的高度相同的，即top=0，计算left 
                left = (int)Math.Round((decimal)(width - newWidth) / 2);
                top = 0;
            }
            // 生成按比例缩放的图，如：160*80的图 
            Bitmap bmpOut2 = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(bmpOut2))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                graphics.DrawImage(originalBmp, 0, 0, newWidth, newHeight);
            }
            // 再把该图画到预先定义的宽高的画布上，如160*120 
            //    Bitmap lastbmp = new Bitmap(width, height);
            //等比例缩放画布
            Bitmap lastbmp = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(lastbmp))
            {
                // 设置高质量插值法 
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 清空画布并以白色背景色填充 
                graphics.Clear(Color.White);
                // 把源图画到新的画布上 全部平铺
                graphics.DrawImage(bmpOut2, left, 0);
            }
            lastbmp.Save(tPath);//保存为文件，tpath 为要保存的路径 
            lastbmp.Dispose();
        }


        #endregion




        //使用C#把发表的时间改为几个月,几天前,几小时前,几分钟前,或几秒前
        //2008年03月15日 星期六 02:35
        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else
            {
                if (span.TotalDays > 30)
                {
                    return
                    "1个月前";
                }
                else
                {
                    if (span.TotalDays > 14)
                    {
                        return
                        "2周前";
                    }
                    else
                    {
                        if (span.TotalDays > 7)
                        {
                            return
                            "1周前";
                        }
                        else
                        {
                            if (span.TotalDays > 1)
                            {
                                return
                                string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                            }
                            else
                            {
                                if (span.TotalHours > 1)
                                {
                                    return
                                    string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                                }
                                else
                                {
                                    if (span.TotalMinutes > 1)
                                    {
                                        return
                                        string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                    }
                                    else
                                    {
                                        if (span.TotalSeconds >= 1)
                                        {
                                            return
                                            string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                        }
                                        else
                                        {
                                            return
                                            "1秒前";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //C#中使用TimeSpan计算两个时间的差值
        //可以反加两个日期之间任何一个时间单位。
        private static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;
        }


        //说明：
        /**/
        /*1.DateTime值类型代表了一个从公元0001年1月1日0点0分0秒到公元9999年12月31日23点59分59秒之间的具体日期时刻。因此，你可以用DateTime值类型来描述任何在想象范围之内的时间。一个DateTime值代表了一个具体的时刻
        2.TimeSpan值包含了许多属性与方法，用于访问或处理一个TimeSpan值
        下面的列表涵盖了其中的一部分：
        Add：与另一个TimeSpan值相加。 
        Days:返回用天数计算的TimeSpan值。 
        Duration:获取TimeSpan的绝对值。 
        Hours:返回用小时计算的TimeSpan值 
        Milliseconds:返回用毫秒计算的TimeSpan值。 
        Minutes:返回用分钟计算的TimeSpan值。 
        Negate:返回当前实例的相反数。 
        Seconds:返回用秒计算的TimeSpan值。 
        Subtract:从中减去另一个TimeSpan值。 
        Ticks:返回TimeSpan值的tick数。 
        TotalDays:返回TimeSpan值表示的天数。 
        TotalHours:返回TimeSpan值表示的小时数。 
        TotalMilliseconds:返回TimeSpan值表示的毫秒数。 
        TotalMinutes:返回TimeSpan值表示的分钟数。 
        TotalSeconds:返回TimeSpan值表示的秒数。
        */

        /**/
        /// <summary>
        /// 日期比较
        /// </summary>
        /// <param name="today">当前日期</param>
        /// <param name="writeDate">输入日期</param>
        /// <param name="n">比较天数</param>
        /// <returns>大于天数返回true，小于返回false</returns>
        public static bool CompareDate(string today, string writeDate, int n)
        {
            DateTime Today = Convert.ToDateTime(today);
            DateTime WriteDate = Convert.ToDateTime(writeDate);
            WriteDate = WriteDate.AddDays(n);
            if (Today >= WriteDate)
                return false;
            else
                return true;
        }




        /// <summary>
        /// 判断对象是否为null,dbnull,empty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(object obj)
        {
            if (obj == null)
                return true;

            if (obj == DBNull.Value)
                return true;

            if (string.IsNullOrWhiteSpace(obj.ToString()))
                return true;

            return false;
        }

        /// <summary>
        /// 将对象转换成字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            if (IsNull(obj))
                return string.Empty;

            return obj.ToString();
        }

        /// <summary>
        /// 将以象转换为int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(object obj)
        {
            if (IsNull(obj)) return 0;

            int result = 0;
            if (!int.TryParse(obj.ToString(), out result)) return 0;

            return result;
        }

        /// <summary>
        /// 将对象转换为datetime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDatetime(object obj)
        {
            if (IsNull(obj))
                return DateTime.MinValue;

            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(obj.ToString(), out result)) return DateTime.MinValue;

            return result;
        }

        /// <summary>
        /// 输出全部异常
        /// </summary>
        /// <param name="exc"></param>
        /// <returns></returns>
        public static string OutputException(Exception exc)
        {
            int maxLevel = 10, currLevel = 0;
            StringBuilder exceptions = new StringBuilder();
            for (var e = exc; e != null; e = exc.InnerException)
            {
                exceptions.Append(e.Message);
                exceptions.Append(Environment.NewLine);

                currLevel++;
                if (currLevel > maxLevel) break;
            }
            return exceptions.ToString();
        }

        /// <summary>
        /// 设置指定时间为所在月份的指定天,如果指定的天数大于当月最大天数，则为当月最后一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="day"></param>
        public static DateTime SetMonthDay(DateTime dateTime, int day)
        {
            var currDay = dateTime.Day;
            var nextMonth = dateTime.AddMonths(1);
            var maxDay = nextMonth.AddDays(0 - nextMonth.Day).Day;
            if (day < maxDay)
                return dateTime.AddDays(day - currDay);
            else
                return dateTime.AddDays(maxDay - currDay);
        }

        /// <summary>
        /// 设置指定时间为所在周的指定星期,星期从0开始，表示星期天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="week"></param>
        public static DateTime SetWeekDay(DateTime dateTime, int week)
        {
            var currWeek = (int)dateTime.DayOfWeek;
            return dateTime.AddDays(week - currWeek);
        }




        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public static string GetValidateCode()
        {
            int rnd = new Random().Next(1000, 9999);
            return rnd.ToString().PadLeft(4, '1');
        }


        #region  日期


        /// <summary>
        /// 获取本周
        /// </summary>
        /// <returns></returns>
        public static string GetWeek(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
            DateTime endWeek = startWeek.AddDays(6);  //本周周日
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startWeek, endWeek);
        }
        /// <summary>
        /// 上周
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetSWeek(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startWeek = DateTime.Now.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) - 7);        //上周一  
            DateTime endWeek = DateTime.Now.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) - 7).AddDays(6);     //上周末
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startWeek, endWeek);
        }

        /// <summary>
        /// 下周
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetXWeek(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startWeek = DateTime.Now.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) + 7);        //下周一  
            DateTime endWeek = DateTime.Now.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) + 7).AddDays(6);      //下周末  
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startWeek, endWeek);
        }

        /// <summary>
        /// 获取本月
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetMonth(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初  
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末 
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startMonth, endMonth);
        }
        /// <summary>
        /// 获取上月
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetSMonth(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);
            DateTime endMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1);
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startMonth, endMonth);
        }

        /// <summary>
        /// 获取下月
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetXMonth(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(1);
            DateTime endMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(2).AddDays(-1);
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startMonth, endMonth);
        }

        /// <summary>
        /// 本季度
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetQuarter(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startQuarter = dt.AddMonths(0 - (dt.Month - 1) % 3).AddDays(1 - dt.Day);  //本季度初  
            DateTime endQuarter = startQuarter.AddMonths(3).AddDays(-1);  //本季度末 
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startQuarter, endQuarter);
        }
        /// <summary>
        /// 获取上季度
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetSQuarter(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startQuarter = DateTime.Now.AddMonths(-3 - ((DateTime.Now.Month - 1) % 3)).AddDays(1 - DateTime.Now.Day);
            DateTime endQuarter = DateTime.Now.AddMonths(0 - ((DateTime.Now.Month - 1) % 3)).AddDays(1 - DateTime.Now.Day).AddDays(-1);
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startQuarter, endQuarter);
        }
        /// <summary>
        /// 本年度
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetYear(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startYear = new DateTime(dt.Year, 1, 1);  //本年年初  
            DateTime endYear = new DateTime(dt.Year, 12, 31);  //本年年末  
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startYear, endYear);
        }

        /// <summary>
        /// 上年度
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetSYear(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startYear = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1);
            DateTime endYear = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddDays(-1);
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startYear, endYear);
        }

        /// <summary>
        /// 前十天
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetQDay(string sqlfile)
        {
            DateTime dt = DateTime.Now;
            DateTime startYear = DateTime.Now.AddDays(-10);  //前十天
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, startYear, DateTime.Now);
        }

        /// <summary>
        /// 后十天
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetHDay(string sqlfile)
        {
            DateTime endYear = DateTime.Now.AddDays(10);  //后十天
            return string.Format(" {0} between '{1}' and '{2}'  ", sqlfile, DateTime.Now, endYear);
        }
        /// <summary>
        /// 当天
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetDateTime(string sqlfile)
        {
            return string.Format("  CONVERT(varchar(10), {0}, 120 )='{1}'  ", sqlfile, DateTime.Now.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// 少于
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetUnderDateTime(string sqlfile)
        {
            return string.Format(" {0}<GETDATE() ", sqlfile);
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="sqlfile"></param>
        /// <returns></returns>
        public static string GetGreaterthanDateTime(string sqlfile)
        {
            return string.Format(" {0}>GETDATE() ", sqlfile);
        }

        #endregion



        #region
        /// <summary>
        /// 枚举引用方法
        /// </summary>
        public class Enum_Name
        {
            /// <summary>
            /// 返回枚举集合
            /// 描述值
            /// Value值
            /// </summary>
            /// <param name="type"> typeof(枚举)</param>
            /// <returns></returns>
            public static ArrayList GetEnumValueArrayList(Type type)
            {

                ArrayList list = new ArrayList();
                Array value = Enum.GetValues(type);//获取枚举的values值
                unchecked
                {
                    foreach (int n in value)
                    {
                        string text = GetEnumDescription(type, n);
                        ListItem listitem = new ListItem(text, n.ToString());
                        list.Add(listitem);
                    }
                }
                return list;
            }

            /// <summary>
            /// 返回集合
            /// 描述值
            /// Name值
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static ArrayList GetEnumNameArrayList(Type type)
            {
                ArrayList list = new ArrayList();
                string[] names = Enum.GetNames(type);//获取枚举的Name值
                Array value = Enum.GetValues(type);//获取枚举的values值
                unchecked
                {
                    int index = 0;
                    foreach (int n in value)
                    {
                        string text = GetEnumDescription(type, names[index].ToString());
                        ListItem listitem = new ListItem(text, names[index++].ToString());
                        list.Add(listitem);
                    }
                }
                return list;
            }

            /// <summary>
            /// 根据Name获取枚举描述信息
            /// </summary>
            /// <param name="enumSubitem">typeof(枚举类型)</param>
            /// <returns>枚举Text值</returns>
            public static string GetEnumDescription(Type type, string enumSubitem)
            {
                try
                {
                    object subitem = Enum.Parse(type, enumSubitem);
                    FieldInfo fieldinfo = subitem.GetType().GetField(enumSubitem);
                    Object[] objs = fieldinfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                    if (objs == null || objs.Length == 0)
                        return enumSubitem;
                    else
                    {
                        System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                        return da.Description;
                    }
                }
                catch (Exception exe)
                {
                    return "";
                }
            }

            /// <summary>
            /// 根据values找枚举描述
            /// </summary>
            /// <param name="type">typeof(枚举类型)</param>
            /// <param name="num">枚举Value值</param>
            /// <returns></returns>
            public static string GetEnumDescription(Type type, int num)
            {
                try
                {
                    object subitem = Enum.Parse(type, num.ToString());
                    string Values = subitem.ToString();
                    FieldInfo fieldinfo = subitem.GetType().GetField(Values);
                    Object[] objs = fieldinfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                    if (objs == null || objs.Length == 0)
                        return Values;
                    else
                    {
                        System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                        return da.Description;
                    }
                }
                catch (Exception exe)
                {
                    return "";
                }

            }

            /// <summary>
            /// 根据Value获取Name值
            /// </summary>
            /// <param name="type">typeof（枚举）</param>
            /// <returns>Name</returns>
            public static string GetEnumName(Type type, int num)
            {
                string[] names = Enum.GetNames(type);//获取枚举的Name值
                Array value = Enum.GetValues(type);//获取枚举的values值
                unchecked
                {
                    int index = 0;
                    foreach (int n in value)
                    {
                        if (n == num)
                            return names[index].ToString();
                        index++;
                    }
                }
                return "";
            }





        }
        #endregion
    }
}
