namespace JxcLite;

public static class AppUtils
{
    public static string GetPartnerName(string billType)
    {
        return billType == BillType.Import || billType == BillType.ImportReturn ? "供应商" : "客户";
    }
}