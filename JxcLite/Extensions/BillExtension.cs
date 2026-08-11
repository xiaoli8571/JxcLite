namespace JxcLite.Extensions;

static class BillExtension
{
    internal static Task<List<JxBillList>> GetBillListsAsync(this Database db, string headId)
    {
        var sql = @"
select a.*,b.Category,b.Code,b.Name,b.Model,b.Producer,b.Unit 
from JxBillList a, JxGoods b 
where a.GoodsId=b.Id and a.HeadId=@headId";
        return db.QueryListAsync<JxBillList>(sql, new { headId });
    }
}
