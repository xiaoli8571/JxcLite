namespace JxcLite.Services;

[WebApi, Service]
class InventoryService(Context context) : ServiceBase(context)
{
    public Task<PagingResult<InventoryInfo>> QueryInventoriesAsync(PagingCriteria criteria)
    {
        var sql = $@"
select a.*,b.ImportQty,b.ImportReturnQty,b.ExportQty,b.ExportReturnQty
     ,b.ImportQty-b.ImportReturnQty-b.ExportQty+b.ExportReturnQty as InventoryQty 
from JxGoods a
left join (
  select l.GoodsId
        ,sum(case when h.Type='{BillType.Import}' then l.Qty else 0 end) as ImportQty
        ,sum(case when h.Type='{BillType.Export}' then l.Qty else 0 end) as ExportQty
        ,sum(case when h.Type='{BillType.ImportReturn}' then l.Qty else 0 end) as ImportReturnQty
        ,sum(case when h.Type='{BillType.ExportReturn}' then l.Qty else 0 end) as ExportReturnQty 
  from JxBillList l, JxBill h 
  where l.CompNo=@CompNo and l.HeadId=h.Id 
  group by l.GoodsId 
) b on b.GoodsId=a.Id 
where a.CompNo=@CompNo";
        return Database.QueryPageAsync<InventoryInfo>(sql, criteria);
    }

    public Task<PagingResult<StockLedgerInfo>> QueryStockLedgersAsync(PagingCriteria criteria)
    {
        var sql = $@"
select l.CreateTime,l.BillType,l.BillNo,l.BillDate
      ,case when l.QtyChange>0 then l.QtyChange else 0 end as InQty
      ,case when l.QtyChange<0 then -l.QtyChange else 0 end as OutQty
      ,l.BalanceQty
      ,g.Category,g.Code,g.Name,g.Model,g.Unit
      ,p.Name as PartnerName 
from JxStockLedger l 
left join JxGoods g on l.GoodsId=g.Id 
left join JxBill b on l.BillId=b.Id 
left join JxPartner p on b.PartnerId=p.Id 
where l.CompNo=@CompNo";
        criteria.Fields[nameof(StockLedgerInfo.Name)] = "g.Name";
        return Database.QueryPageAsync<StockLedgerInfo>(sql, criteria);
    }
}