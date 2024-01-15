namespace Moments.Common
{
    /// <summary>
    /// 提供对字符串操作的常用方法。
    /// </summary>
    public static class String
    {
        /// <summary>
        /// 从逗号分隔的字符串中获取标签列表。
        /// </summary>
        /// <param name="str">逗号分隔的字符串。</param>
        /// <returns>包含标签的列表。</returns>
        public static List<string> GetTags(string? str)
        {
            return str is null ? new List<string>() : new List<string>(str.Split(','));
        }

        /// <summary>
        /// 向标签列表中添加新标签，并返回以逗号分隔的字符串。
        /// </summary>
        /// <param name="tags">标签列表。</param>
        /// <param name="t">要添加的标签。</param>
        /// <returns>更新后的逗号分隔标签字符串。</returns>
        public static string AddTags(List<string> tags, string t)
        {
            tags.Add(t);
            return string.Join(",", tags);
        }

        /// <summary>
        /// 从标签列表中删除指定的标签，并返回以逗号分隔的字符串。
        /// </summary>
        /// <param name="tags">标签列表。</param>
        /// <param name="t">要删除的标签。</param>
        /// <returns>更新后的逗号分隔标签字符串。</returns>
        public static string DeleteTags(List<string> tags, string t)
        {
            tags.Remove(t);
            return string.Join(",", tags);
        }

        public static string GetPluginPackageName(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
    }
}