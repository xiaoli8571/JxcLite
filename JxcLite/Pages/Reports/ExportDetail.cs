namespace JxcLite.Pages.Reports;

class ExportDetail : BillDetail
{
    protected override string Type => BillType.Export;
}

class ExportReturnDetail : BillDetail
{
    protected override string Type => BillType.ExportReturn;
}