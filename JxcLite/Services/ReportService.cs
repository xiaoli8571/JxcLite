namespace JxcLite.Services;

[WebApi, Service]
class ReportService(Context context) : ServiceBase(context)
{
    public Task<PagingResult<BillDetailInfo>> QueryBillDetailsAsync(PagingCriteria criteria)
    {
        if (criteria.OrderBys == null || criteria.OrderBys.Length == 0)
            criteria.OrderBys = [$"{nameof(BillDetailInfo.BillDate)}"];

        var sql = @"
select a.Id,a.CreateTime,a.SeqNo,a.Qty,a.Price,a.Amount,a.Note as ListNote
      ,b.Type,b.BillNo,b.Status,b.BillDate,b.ContractNo,b.InvoiceNo
      ,b.SettleMode,b.SumAmount,b.SumTaxAmount,b.SumTotalAmount
      ,b.Logistics,b.LogisticsNo,b.LogisticsFee,b.Note
      ,c.BillNo as RefBillNo 
      ,d.Name as PartnerName 
      ,e.Category,e.Code,e.Name,e.Model,e.Producer,e.Unit 
from JxBillList a 
left join JxBill b on b.Id=a.HeadId 
left join JxBill c on c.Id=b.RefBillId 
left join JxPartner d on d.Id=b.PartnerId 
left join JxGoods e on e.Id=a.GoodsId 
where a.CompNo=@CompNo";
        criteria.Fields[nameof(JxBill.Type)] = "b.Type";
        return Database.QueryPageAsync<BillDetailInfo>(sql, criteria);
    }

    public Task<PagingResult<ProfitInfo>> QueryProfitsAsync(PagingCriteria criteria)
    {
        var sql = $@"
select a.*,b.ImportAmount,b.ImportReturnAmount,b.ExportAmount,b.ExportReturnAmount
     ,b.ExportAmount-b.ExportReturnAmount-b.ImportAmount+b.ImportReturnAmount as Profit
from JxGoods a 
left join (
  select l.GoodsId
        ,sum(case when h.Type='{BillType.Import}' then l.Amount else 0 end) as ImportAmount
        ,sum(case when h.Type='{BillType.Export}' then l.Amount else 0 end) as ExportAmount
        ,sum(case when h.Type='{BillType.ImportReturn}' then l.Amount else 0 end) as ImportReturnAmount
        ,sum(case when h.Type='{BillType.ExportReturn}' then l.Amount else 0 end) as ExportReturnAmount 
  from JxBillList l, JxBill h 
  where l.CompNo=@CompNo and l.HeadId=h.Id 
  group by l.GoodsId 
) b on b.GoodsId=a.Id 
where a.CompNo=@CompNo";
        return Database.QueryPageAsync<ProfitInfo>(sql, criteria);
    }
}