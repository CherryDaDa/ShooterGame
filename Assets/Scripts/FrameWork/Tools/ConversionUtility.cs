namespace Framework.Tools
{
    public static class ConversionUtility
    {
        public static string BytesToString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }

        public static byte[] StringToBytes(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }

        public static int SafeStringToInt(string str, int defaultValue = 0)
        {
            int result;
            if (int.TryParse(str, out result))
                return result;
            return defaultValue;
        }

        // ... 其他转换方法
    }
}