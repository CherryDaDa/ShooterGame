using System.Runtime.CompilerServices;

namespace Framework.Extension
{
	/// <summary>
	/// 字符串扩展方法
	/// </summary>
	public static class StringExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasValue(this string value)
		{
			return !string.IsNullOrEmpty(value);
		}
	}
}