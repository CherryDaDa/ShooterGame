using System;

namespace Framework.Tools
{
    public static class TimeUtility
    {
        /// <summary>
        /// 将秒转换为 HH:MM:SS 格式的字符串。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>HH:MM:SS 格式的字符串。</returns>
        public static string SecondsToHHMMSS(ulong totalSeconds)
        {
            var hours = totalSeconds / 3600;
            var minutes = (totalSeconds % 3600) / 60;
            var seconds = totalSeconds % 60;

            return $"{hours:00}:{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// 将秒转换为 MM:SS 格式的字符串。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>MM:SS 格式的字符串。</returns>
        public static string SecondsToMMSS(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            return $"{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// 将秒转换为 DD:HH:MM:SS 格式的字符串。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>DD:HH:MM:SS 格式的字符串。</returns>
        public static string SecondsToDDHHMMSS(int totalSeconds)
        {
            int days = totalSeconds / 86400;
            int hours = (totalSeconds % 86400) / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            return $"{days:00}:{hours:00}:{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// 将秒转换为天数。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>天数。</returns>
        public static int SecondsToDays(int totalSeconds)
        {
            return totalSeconds / 86400;
        }

        /// <summary>
        /// 将秒转换为小时数。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>小时数。</returns>
        public static int SecondsToHours(int totalSeconds)
        {
            return totalSeconds / 3600;
        }

        /// <summary>
        /// 将秒转换为分钟数。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>分钟数。</returns>
        public static int SecondsToMinutes(int totalSeconds)
        {
            return totalSeconds / 60;
        }

        /// <summary>
        /// 获取剩余的小时部分（不包括转换为天数的部分）。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>小时数。</returns>
        public static int RemainingHours(int totalSeconds)
        {
            return (totalSeconds % 86400) / 3600;
        }

        /// <summary>
        /// 获取剩余的分钟部分（不包括转换为小时数的部分）。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>分钟数。</returns>
        public static int RemainingMinutes(int totalSeconds)
        {
            return (totalSeconds % 3600) / 60;
        }

        /// <summary>
        /// 获取剩余的秒部分（不包括转换为分钟数的部分）。
        /// </summary>
        /// <param name="totalSeconds">总秒数。</param>
        /// <returns>秒数。</returns>
        public static int RemainingSeconds(int totalSeconds)
        {
            return totalSeconds % 60;
        }

        /// <summary>
        /// 获取当前的UTC时间戳（毫秒）
        /// </summary>
        /// <returns></returns>
        public static ulong GetUtcTimestampMilliseconds()
        {
            ulong timestampMilliseconds = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return timestampMilliseconds;
        }

        /// <summary>
        /// 获取当前的UTC时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public static ulong GetUtcTimestamp()
        {
            ulong timestampSeconds = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            return timestampSeconds;
        }
        
        public static ulong GetTimestamp()
        {
            ulong timestampSeconds = (ulong)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
            return timestampSeconds;
        }
        
        public static ulong GetTimestampMilliseconds()
        {
            ulong timestampMilliseconds = (ulong)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return timestampMilliseconds;
        }
        
        /// <summary>
        /// 时间戳转时间格式
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string UnixTimeStampToDateTime(long unixTimeStamp, string format = "yyyy-MM-dd HH:mm:ss")
        {
            DateTime dateTime;
        
            // 判断时间戳是否是以秒为单位，如果是，直接转换为DateTime对象
            if (unixTimeStamp > 1_000_000_000_000)
            {
                dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).DateTime;
            }
            else
            {
                // Unix时间戳是以秒为单位的，所以需要转换为DateTime对象
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                dateTime = unixEpoch.AddSeconds(unixTimeStamp).ToLocalTime();
            }

            // 返回格式化的日期时间字符串
            return dateTime.ToString(format);
        }
        
        /// <summary>
        /// 根据指定时间返回时间戳
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ulong GetTimestamp(int year, int month, int day, int hour, int minute, int second)
        {
            try
            {
                // 创建一个本地时间的 DateTime 对象
                DateTime dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local);
        
                // 将本地时间转换为 UTC 时间
                DateTime utcDateTime = dateTime.ToUniversalTime();

                // 转换为 DateTimeOffset 以获取 Unix 时间戳
                long timestamp = ((DateTimeOffset)utcDateTime).ToUnixTimeSeconds();
        
                // 检查时间戳是否为负数
                if (timestamp < 0)
                {
                    throw new ArgumentException("Date is before Unix epoch");
                }

                // 返回时间戳，转换为无符号长整型
                return (ulong)timestamp;
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentException("Invalid date/time parameters", e);
            }
        }
    }
}
