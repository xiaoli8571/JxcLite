namespace JxcLite.Models;

/// <summary>
/// 工厂信息类。
/// </summary>
public class FactoryInfo
{
    /// <summary>
    /// 取得或设置工厂简称。
    /// </summary>
    [Form(Row = 1, Column = 1)]
    [DisplayName("工厂简称")]
    public string ShortName { get; set; }

    /// <summary>
    /// 取得或设置工厂全称。
    /// </summary>
    [Form(Row = 1, Column = 1)]
    [DisplayName("工厂全称")]
    public string Name { get; set; }

    /// <summary>
    /// 取得或设置工厂地址。
    /// </summary>
    [Form(Row = 1, Column = 1)]
    [DisplayName("工厂地址")]
    public string Address { get; set; }

    /// <summary>
    /// 取得或设置联系方式。
    /// </summary>
    [Form(Row = 2, Column = 1)]
    [DisplayName("联系方式")]
    public string Contact { get; set; }

    /// <summary>
    /// 取得或设置注意事项。
    /// </summary>
    [Form(Row = 3, Column = 1)]
    [DisplayName("注意事项")]
    public string Note { get; set; }
}