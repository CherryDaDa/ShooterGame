using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Tools
{
    public static class StringUtility
    {
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        public static string RemoveSpecificChars(string input, char[] charsToRemove)
        {
            return new string(input.Where(ch => !charsToRemove.Contains(ch)).ToArray());
        }
    
        /// <summary>
        /// 格式化资源大小
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatAssetSize(ulong number)
        {
            if (number >= 1000000)
            {
                return (number / 1000000.0).ToString("0.#") + "MB";
            }
            else if (number >= 1000)
            {
                return (number / 1000.0).ToString("0.#") + "KB";
            }
            else
            {
                return number.ToString();
            }
        }
        
        //... 其他字符串处理方法
        
        /// <summary>
        /// 返回一个字符串的MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string input)
        {
            // 创建一个 MD5 实例
            using (MD5 md5 = MD5.Create())
            {
                // 将字符串转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            
                // 计算哈希值
                byte[] hashBytes = md5.ComputeHash(inputBytes);
            
                // 将字节数组转换为十六进制字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }

}