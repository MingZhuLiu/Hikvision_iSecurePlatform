using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hikvision_iSecurePlatform.Lib.Commons
{
    public static class Tools
    {

        static Tools()
        {
            jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }



        public static long NewID { get => DateTime.Now.ToTimestamp(true) * 1000 + GeneratteRandom(3); }
        //public static string NewID { get => DateTime.Now.ToTimestamp(true); }

        /// <summary>
        /// 转换指定时间得到对应的时间戳
        /// Add by 成长的小猪（Jason.Song） on 2019/05/07
        /// http://blog.csdn.net/jasonsong2008
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
        /// <returns>返回对应的时间戳</returns>
        public static long ToTimestamp(this DateTime dateTime, bool millisecond = true)
        {
            return dateTime.ToTimestampLong(millisecond);
        }

        private static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static string GenerateRandomStr(int Length) //调用时想生成几位就几位；Length等于多少就多e69da5e887aa7a686964616f31333365636639少位。
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

        private static Random rad = new Random();
        public static long GeneratteRandom(int length)
        {
            return rad.Next((int)Math.Pow(10, length), (int)(Math.Pow(10, length + 1) - 1));
        }

        /// <summary>
        /// 转换指定时间得到对应的时间戳
        /// Add by 成长的小猪（Jason.Song） on 2019/05/07
        /// http://blog.csdn.net/jasonsong2008
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
        /// <returns>返回对应的时间戳</returns>
        public static long ToTimestampLong(this DateTime dateTime, bool millisecond = true)
        {
            var ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return millisecond ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
        }



        public static String URLEncode(String input)
        {
            return System.Web.HttpUtility.UrlEncode(input, Encoding.UTF8);
        }

        public static String URIEncode(String input)
        {
            return Uri.EscapeDataString(input);
        }

        public static String URLDecode(String input)
        {
            return System.Web.HttpUtility.UrlDecode(input, Encoding.UTF8);
        }

        public static String URLDecode(String input, Encoding encoding)
        {
            return System.Web.HttpUtility.UrlDecode(input, encoding);
        }

        public static string UrlDecode(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            return System.Web.HttpUtility.UrlDecode(str);
        }

        public static string UrlEncode(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            return System.Web.HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string String2Unicode(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string GetSizeShowStr(double size)
        {
            var num = 1024.00; //byte
            if (size < num)
                return size + "B";
            if (size < Math.Pow(num, 2))
                return (size / num).ToString("f2") + "Kb"; //kb
            if (size < Math.Pow(num, 3))
                return (size / Math.Pow(num, 2)).ToString("f2") + "Mb"; //M
            if (size < Math.Pow(num, 4))
                return (size / Math.Pow(num, 3)).ToString("f2") + "Gb"; //G
            return (size / Math.Pow(num, 4)).ToString("f2") + "Tb"; //T
        }

        public static string GetTimeSpanShowStr(int seconds)
        {
            int h = seconds / 3600;
            int m = seconds % 3600 / 60;
            int s = seconds % 60; //不足60的就是秒，够60就是分
            return h + "小时" + m + "分钟" + s + "秒";
        }

        public static String getStrOfSeconds(long seconds)
        {
            if (seconds < 0)
            {
                return "秒数必须大于0";
            }
            long one_day = 60 * 60 * 24;
            long one_hour = 60 * 60;
            long one_minute = 60;
            long day, hour, minute, second = 0L; ;

            day = seconds / one_day;
            hour = seconds % one_day / one_hour;
            minute = seconds % one_day % one_hour / one_minute;
            second = seconds % one_day % one_hour % one_minute;

            if (seconds < one_minute)
            {
                return seconds + "秒";
            }
            else if (seconds >= one_minute && seconds < one_hour)
            {
                return minute + "分" + second + "秒";
            }
            else if (seconds >= one_hour && seconds < one_day)
            {
                return hour + "时" + minute + "分" + second + "秒";
            }
            else
            {
                return day + "天" + hour + "时" + minute + "分" + second + "秒";
            }
        }

        private static Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings = null;


        public static string ToJson(Object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data, jsonSerializerSettings);
        }


    }
}