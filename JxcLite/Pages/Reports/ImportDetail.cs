namespace JxcLite.Pages.Reports;

class ImportDetail : BillDetail
{
    protected override string Type => BillType.Import;
}

class ImportReturnDetail : BillDetail
{
    protected override string Type => BillType.ImportReturn;
}