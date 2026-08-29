namespace JxcLite.Services;

[WebApi, Service]
class InventoryService(Context context) : ServiceBase(context)
{
    public Task<PagingResult<InventoryInfo>> QueryInventoriesAsync(PagingCriteria criteria)
    {
        var sql = $@"
select a.*,a.InitialQty
     ,b.ImportQty,b.ImportReturnQty,b.ExportQty,b.ExportReturnQty
     ,p.ProcessUseQty,p.ProcessReturnQty
     ,ifnull(d.StockQty,a.InitialQty) as InventoryQty 
from JxGoods a
left join JxInventory d on d.GoodsId=a.Id
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
left join (
  select t.GoodsId
        ,sum(case when t.Type='Process' then t.QtyNum else 0 end) as ProcessUseQty
        ,sum(case when t.Type='ProcessReturn' then t.QtyNum else 0 end) as ProcessReturnQty
  from (
    select GoodsId,Type,cast(InputQty as real) as QtyNum 
    from JxProcess 
    where GoodsId is not null and GoodsId<>'' and Status<>'{BizStatus.Verified}'
  ) t
  group by t.GoodsId 
) p on p.GoodsId=a.Id 
where a.CompNo=@CompNo";
        return Database.QueryPageAsync<InventoryInfo>(sql, criteria);
    }

    /// <summary>
    /// 按商品ID查询当前库存(公式同列表:期初+进-进退-销+销退-加工领用+加工退回)。
    /// </summary>
    public async Task<InventoryInfo> GetInventoryByGoodsIdAsync(string goodsId)
    {
        if (string.IsNullOrWhiteSpace(goodsId))
            return null;
        InventoryInfo info = null;
        await Database.QueryActionAsync(async db =>
        {
            var sql = $@"
select a.Id,a.InitialQty
     ,ifnull(b.ImportQty,0) as ImportQty,ifnull(b.ImportReturnQty,0) as ImportReturnQty
     ,ifnull(b.ExportQty,0) as ExportQty,ifnull(b.ExportReturnQty,0) as ExportReturnQty
     ,ifnull(p.ProcessUseQty,0) as ProcessUseQty,ifnull(p.ProcessReturnQty,0) as ProcessReturnQty
     ,ifnull(d.StockQty,a.InitialQty) as InventoryQty 
from JxGoods a
left join JxInventory d on d.GoodsId=a.Id
left join (
  select l.GoodsId
        ,sum(case when h.Type='{BillType.Import}' then l.Qty else 0 end) as ImportQty
        ,sum(case when h.Type='{BillType.Export}' then l.Qty else 0 end) as ExportQty
        ,sum(case when h.Type='{BillType.ImportReturn}' then l.Qty else 0 end) as ImportReturnQty
        ,sum(case when h.Type='{BillType.ExportReturn}' then l.Qty else 0 end) as ExportReturnQty 
  from JxBillList l, JxBill h 
  where l.HeadId=h.Id and l.GoodsId=@goodsId 
  group by l.GoodsId 
) b on b.GoodsId=a.Id 
left join (
  select t.GoodsId
        ,sum(case when t.Type='Process' then t.QtyNum else 0 end) as ProcessUseQty
        ,sum(case when t.Type='ProcessReturn' then t.QtyNum else 0 end) as ProcessReturnQty
  from (
    select GoodsId,Type,cast(InputQty as real) as QtyNum 
    from JxProcess 
    where GoodsId=@goodsId and Status<>'{BizStatus.Verified}'
  ) t
  group by t.GoodsId 
) p on p.GoodsId=a.Id 
where a.Id=@goodsId";
            info = await db.QueryAsync<InventoryInfo>(sql, new { goodsId });
        });
        return info;
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
