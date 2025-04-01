namespace Framework.Tools
{
    public static class NetworkUtility
    {
        public static bool IsInternetAvailable()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                using (client.OpenRead("http://google.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        // ... 其他网络方法，例如文件下载、HTTP请求等。
    }
}