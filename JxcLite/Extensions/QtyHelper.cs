using System.Globalization;

namespace JxcLite.Extensions;

/// <summary>
/// 数量文本解析工具(不依赖框架，可单独验证)。
/// </summary>
static class QtyHelper
{
    /// <summary>
    /// 拆分数量文本：开头数字 + 后面单位，如"350码"→(350, "350", "码")。
    /// 用于兼容把单位直接写进投坯数量框的填法，避免数量解析失败导致库存静默不扣减。
    /// </summary>
    /// <param name="text">数量文本。</param>
    /// <returns>数值、数字部分文本、单位部分文本。</returns>
    internal static (double Qty, string Number, string Unit) SplitQty(string text)
    {
        var s = text?.Trim() ?? "";
        var i = 0;
        while (i < s.Length && (char.IsAsciiDigit(s[i]) || s[i] == '.'))
            i++;

        double.TryParse(s[..i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var qty);
        return (qty, s[..i], s[i..].Trim());
    }
}
