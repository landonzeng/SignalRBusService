using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    /// <summary>
    /// 时间戳 转换工具
    /// </summary>
    public class Unixtimestamp
    {
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name=”time”></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((time - DateStart).TotalSeconds); 
        }
    }
}
