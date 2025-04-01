namespace Framework.Tools
{
    public static class FileUtility
    {
        public static bool FileExists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }

        public static string ReadFile(string filePath)
        {
            if (FileExists(filePath))
            {
                return System.IO.File.ReadAllText(filePath);
            }
            return null;
        }
    
        //... 其他文件处理方法
        
        public static bool IsNetworkAddress(string path)
        {
            return path.StartsWith("http://") || path.StartsWith("https://") || path.StartsWith("blob:http");
        }
        
        /// <summary>
        /// 是否是本地绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsLocalPath(string path)
        {
            return System.IO.Path.IsPathRooted(path);
        }
    }
}