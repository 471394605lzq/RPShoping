using MirrorMirror;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PublicLibrary
{
   public static class Extension
    {
        /// <summary>
        /// 使用指定的字符串连接列表
        /// </summary>
        /// <typeparam name="T">源类型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="splitStr">连接字符串</param>
        /// <param name="refstr">引用字符串</param>
        /// <returns>连接后的字符串</returns>
        public static string Concat<T>(this IEnumerable<T> list, string splitStr, string refstr)
        {
            StringBuilder rsb = new StringBuilder();
            foreach (var item in list)
            {
                rsb.Append(refstr);
                rsb.Append(item.ToString());
                rsb.Append(refstr);
                rsb.Append(splitStr);
            }
            if (rsb.Length >= splitStr.Length)
            {
                rsb.Remove(rsb.Length - splitStr.Length, splitStr.Length);
            }
            return rsb.ToString();
        }

        public static string GetValue(string context, string key)
        {
            var arr = context.Split(';');
            foreach (var item in arr)
            {
                var itemArr = item.Split('|');
                if (itemArr[0].Equals(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    return itemArr[1];
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 将可以枚举的类型用指定的字符串连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static string Concat<T>(this IEnumerable<T> list, string splitStr)
        {
            return Concat<T>(list, splitStr, "");
        }

        /// <summary>
        /// 将源数组转换成目标类型的数组
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="array"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static TOutput[] Convert<TInput, TOutput>(this TInput[] array, Converter<TInput, TOutput> converter)
        {
            if (converter == null)
                throw new ArgumentException("参数converter不能为空");
            TOutput[] outArray = new TOutput[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                outArray[i] = converter(array[i]);
            }
            return outArray;
        }

        /// <summary>
        /// 让数组中的每一个元素执行指定的某个操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (T item in array)
            {
                action(item);
            }
        }

        /// <summary>
        /// 是否为数字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNumber<T>(this T obj)
        {
            if (obj == null) return false;
            return IsNumber(obj.ToString());
        }

        /// <summary>
        /// 是否为数字
        /// </summary>
        public static bool IsNumber(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return !Regex.IsMatch(str, "[^\\d]");
        }

        /// <summary>
        /// 是否为时间格式
        /// </summary>
        public static bool IsDateTime(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            var dt = new DateTime();
            return DateTime.TryParse(str, out dt);
        }

        public static bool IsEmailAddress(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return Regex.IsMatch(str, @"^\s*([A-Za-z0-9_-]+(\.\w+)*@([\w-]+\.)+\w{2,3})\s*$");
        }
        /// <summary>
        /// 将源数组中的部分元素复制到另一个数组的指定部分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="srcIndex"></param>
        /// <param name="tagArray"></param>
        /// <param name="tagIndex"></param>
        public static void CopyTo<T>(this T[] array, int srcIndex, T[] tagArray, int tagIndex)
        {
            T[] srcRes = new T[array.Length - srcIndex];
            for (var i = 0; i < array.Length; i++)
            {
                if (i >= srcIndex)
                {
                    srcRes[i - srcIndex] = array[i];
                }
            }
            for (var k = 0; k < srcRes.Length; k++)
            {
                tagArray[tagIndex + k] = srcRes[k];
            }
        }

        /// <summary>
        /// 将对象转换成json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString<T>(this T obj) where T : class
        {
            JsonQueryStringConverter json = new JsonQueryStringConverter();
            return json.ConvertValueToString(obj, typeof(T));
        }

        /// <summary>
        /// 将字符串中的特殊字符转换成相应的HTML字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CleanupStringForGrid(this string str)
        {
            return DataConvert.ReplaceSpecStr(str);
        }

        /// <summary>
        /// 獲取對象的屬性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty(this object obj, string propertyName)
        {
            if (obj.HasFieldOrProperty(propertyName))
                return obj.Get(propertyName);
            else if (obj.GetType() == typeof(DataRow))
            {
                return ((DataRow)obj)[propertyName];
            }

            return null;
        }

        /// <summary>
        /// 是否存在指定的属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object HasProperty(this object obj, string propertyName)
        {
            if (obj.GetType() == typeof(DataRow))
                return ((DataRow)obj).Table.Columns.Contains(propertyName);
            else
                return obj.HasFieldOrProperty(propertyName);
        }

        /// <summary>
        /// 获取指定字符串的后几位
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Right(this string str, int len)
        {
            if (str.Length < len)
                return str;

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// 获取指定字符串的前几位
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(this string str, int len)
        {
            if (str.Length < len)
                return str;

            return str.Substring(0, len);
        }

        /// <summary>
        /// 根据时间戳获取UTC时间
        /// </summary>
        /// <param name="span">与1970年的毫秒数</param>
        /// <returns></returns>
        public static DateTime FromTimeStampUTC(this DateTime @this, long span)
        {
            var circlePoint = new DateTime(
                year: 1970, month: 01, day: 01,
                hour: 00, minute: 00, second: 00,
                kind: DateTimeKind.Utc
                );

            return circlePoint.AddMilliseconds(span);
        }
    }
}
