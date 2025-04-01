using System.Text;

namespace Framework.Extension
{
    /// <summary>
    /// 数组扩展方法
    /// </summary>
    public static class ArrayExtensions
    {
        public static string ToCustomString<T>(this T[] array)
        {
            if (array == null) return "null";
        
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");

            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i]?.ToString() ?? "null");

                if (i < array.Length - 1)
                    sb.Append(", ");
            }

            sb.Append(" ]");

            return sb.ToString();
        }
    }
}