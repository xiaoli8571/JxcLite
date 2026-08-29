namespace JxcLite;

/// <summary>
/// 系统常量类，定义系统所需的常量。
/// </summary>
public class AppConstant
{
    private AppConstant() { }

    public const string KeyFactory = "FactoryInfo";

    // 模块
    public const string Import = "Import";
    public const string Export = "Export";
    public const string Inventory = "Inventory";
    public const string Finance = "Finance";
    public const string Report = "Report";
    public const string Process = "Process";
}

public class DicCategory
{
    public const string GoodsType = "GoodsType";
    public const string Unit = "Unit";
}

public class AppNoRule
{
    public const string Import = "Import";
    public const string ImportReturn = "ImportReturn";
    public const string Export = "Export";
    public const string ExportReturn = "ExportReturn";
    public const string AccountCustomer = "AccountCustomer";
    public const string AccountSupplier = "AccountSupplier";
    public const string OtherFee = "OtherFee";
    public const string PaymentIn = "PaymentIn";
    public const string PaymentOut = "PaymentOut";
    public const string Order = "Order";
    public const string Process = "Process";
}

/// <summary>
/// 商业伙伴类型常量类。
/// </summary>
public class PartnerType
{
    public const string Supplier = "供应商";
    public const string Customer = "客户";
}

/// <summary>
/// 业务单据类型常量类。
/// </summary>
public class BillType
{
    public const string Import = "进货";
    public const string ImportReturn = "进退货";
    public const string Export = "销货";
    public const string ExportReturn = "销退货";

    /// <summary>
    /// 根据单据类型获取对应的退货单类型。
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetReturnType(string type)
    {
        if (type == Import)
            return ImportReturn;
        else if (type == Export)
            return ExportReturn;
        return string.Empty;
    }
}

[CodeInfo]
public class SettleModeType
{
    public const string Cash = "现付";
    public const string Account = "对账";
}

[CodeInfo]
public class PaymentSource
{
    public const string Manual = "新增";
    public const string Account = "对账单";
    public const string Other = "其他费用";
}

[CodeInfo]
public class FeeType
{
    public const string Income = "收入";
    public const string Expense = "支出";
}

[CodeInfo]
public class BizStatus
{
    public const string Save = "暂存";
    public const string Verifing = "待审核";
    public const string Verified = "已审核";
}