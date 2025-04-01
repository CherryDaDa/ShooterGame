using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Framework.Core;
using Framework.Tools;

namespace Framework.SensitiveWord
{
    public class SensitiveWordFilter : Singleton<SensitiveWordFilter>
    {
        private readonly HashSet<string> _sensitiveWords = new HashSet<string>();
        private string _sensitiveWordPattern;
        
        public void Init(string configStr)
        {
            var words = configStr.Split('\n');
            foreach (var word in words)
            {
                string trimmedWord = word.Trim();
                if (!string.IsNullOrEmpty(trimmedWord))
                {
                    _sensitiveWords.Add(trimmedWord);
                }
            }
            // 对每个敏感词进行转义并生成正则表达式模式
            _sensitiveWordPattern = string.Join("|", _sensitiveWords.Select(Regex.Escape));
        }

        /// <summary>
        /// 过滤敏感字并替换为replacement
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public string FilterText(string inputText, char replacement = '*')
        {
            if (string.IsNullOrEmpty(_sensitiveWordPattern))
            {
                return inputText; // 如果没有敏感词，直接返回输入文本
            }

            var filterText = Regex.Replace(inputText, _sensitiveWordPattern, match => new string(replacement, match.Value.Length), RegexOptions.IgnoreCase);
            DebugUtil.Log($"过滤后文本：{filterText}");
            return filterText;
        }
        
        /// <summary>
        /// 返回输入字符串中的敏感字
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public string[] FindSensitiveWords(string inputText)
        {
            if (string.IsNullOrEmpty(_sensitiveWordPattern))
            {
                return Array.Empty<string>(); // 如果没有敏感词，返回空数组
            }

            var matches = Regex.Matches(inputText, _sensitiveWordPattern, RegexOptions.IgnoreCase);
            return matches.Cast<Match>().Select(m => m.Value).Distinct().ToArray();
        }
        
        /// <summary>
        /// 检测输入字符串中是否含有敏感字
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public bool ContainsSensitiveWords(string inputText)
        {
            return !string.IsNullOrEmpty(_sensitiveWordPattern) &&
                   Regex.IsMatch(inputText, _sensitiveWordPattern, RegexOptions.IgnoreCase);
        }
    }
}