using System.Text.RegularExpressions;

namespace Framework.Tools
{
    public static class RegexPatterns
    {
        // 邮箱地址的正则表达式模式
        public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        // URL地址的正则表达式模式
        public const string Url =  @"^(?:https?://(?:www\.)?[^\s/$.?#].[^\s]*|)?$";
        // IP地址的正则表达式模式（IPv4）
        public const string IPv4 = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";

        // IP地址的正则表达式模式（IPv6）
        public const string IPv6 = @"\b([0-9a-fA-F]{1,4}:){7}([0-9a-fA-F]{1,4}|:)\b";

        // 电话号码的正则表达式模式
        public const string PhoneNumber = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";

        // 邮政编码的正则表达式模式（5位或9位数字）
        public const string ZipCode = @"^\d{5}(?:[-\s]\d{4})?$";

        // 身份证号码的正则表达式模式（18位数字，最后一位可以为X）
        public const string IDCard = @"^\d{17}[\dXx]$";

        // 用户名的正则表达式模式（3到20个字符，允许字母、数字、下划线）
        public const string UserName = @"^[a-zA-Z0-9_]{3,20}$";

        // 密码的正则表达式模式（至少包含一个大写字母、一个小写字母、一个数字，长度至少8个字符）
        public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
        
        // 手机号码的正则表达式模式（11位数字，以1开头）
        public const string MobileNumber = @"^1\d{10}$";

        // 日期的正则表达式模式（YYYY-MM-DD）
        public const string Date = @"^\d{4}-\d{2}-\d{2}$";

        // 时间的正则表达式模式（HH:MM:SS）
        public const string Time = @"^(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d$";

        // 日期时间的正则表达式模式（YYYY-MM-DD HH:MM:SS）
        public const string DateTime = @"^\d{4}-\d{2}-\d{2} (?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d$";

        // 中文字符的正则表达式模式
        public const string ChineseCharacters = @"[\u4e00-\u9fa5]";

        // 车牌号的正则表达式模式（中国）
        public const string LicensePlate = @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[\u4e00-\u9fa5A-Z0-9]{5}$";
        
        // 带中文名称的正则表达式模式（可以包含英文、汉字、数字）
        public const string UserNameCn = @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$";
        
        // 带中文名称的正则表达式模式（可以包含英文、汉字、数字、下划线，总长度不超过14个字符，其中一个汉字算作两个字符）
        public const string UserNameCn14 = @"^(?:[\u4e00-\u9fa5]{1,7}|[a-zA-Z0-9_]{1,14}|(?=.*[\u4e00-\u9fa5])(?=.*[a-zA-Z0-9_]).{1,14})$";
        
        // 密码的正则表达式模式（只允许包含大小写字母、数字、.、*）
        public const string SimplePassword = @"^[a-zA-Z0-9.*]+$";
    }
    
    public static class RegexUtility
    {
        /// <summary>
        /// 验证输入字符串是否匹配指定的正则表达式模式
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(string input, string pattern)
        {
            try
            {
                // 使用正则表达式验证输入字符串
                return Regex.IsMatch(input, pattern);
            }
            catch (RegexMatchTimeoutException)
            {
                return false; // 匹配超时时返回false
            }
        }

        /// <summary>
        /// 提取匹配指定正则表达式模式的所有字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static MatchCollection Matches(string input, string pattern)
        {
            try
            {
                // 使用正则表达式提取匹配的字符串
                return Regex.Matches(input, pattern);
            }
            catch (RegexMatchTimeoutException)
            {
                return null; // 匹配超时时返回null
            }
        }

        /// <summary>
        /// 替换匹配指定正则表达式模式的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Replace(string input, string pattern, string replacement)
        {
            try
            {
                // 使用正则表达式替换匹配的字符串
                return Regex.Replace(input, pattern, replacement);
            }
            catch (RegexMatchTimeoutException)
            {
                return input; // 匹配超时时返回原始输入字符串
            }
        }

        /// <summary>
        /// 拆分匹配指定正则表达式模式的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string[] Split(string input, string pattern)
        {
            try
            {
                // 使用正则表达式拆分字符串
                return Regex.Split(input, pattern);
            }
            catch (RegexMatchTimeoutException)
            {
                return new string[] { input }; // 匹配超时时返回包含原始输入字符串的数组
            }
        }
        
        /// <summary>
        /// 根据传入的正则表达式，返回输入字符串中所有匹配的有效内容，将不匹配的字符移除
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string GetValidContent(string input, string pattern)
        {
            try
            {
                // 使用正则表达式匹配输入字符串中所有匹配项
                MatchCollection matches = Regex.Matches(input, pattern);
                string result = string.Empty;
                
                foreach (Match match in matches)
                {
                    result += match.Value;
                }
                
                return result;
            }
            catch (RegexMatchTimeoutException)
            {
                return input; // 匹配超时时返回原始输入字符串
            }
        }
        
        
        public static bool IsValidInput(string input,string pattern)
        { 
            // if (string.IsNullOrEmpty(input)) 
            // {
            //         return false; 
            // }
            return Regex.IsMatch(input, pattern);
        }
    
    }
}
