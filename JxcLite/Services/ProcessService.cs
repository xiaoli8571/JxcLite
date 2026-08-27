namespace JxcLite.Services;

[WebApi, Service]
class ProcessService(Context context) : ServiceBase(context)
{
    public Task<PagingResult<ProcessInfo>> QueryProcessAsync(PagingCriteria criteria)
    {
        var sql = @"
select a.*,b.Name as GoodsName
from JxProcess a
left join JxGoods b on a.GoodsId=b.Id
where a.CompNo=@CompNo";
        var type = criteria.GetParameter<string>("Type");
        if (!string.IsNullOrWhiteSpace(type))
            sql += " and a.Type=@Type";
        criteria.Fields[nameof(ProcessInfo.Type)] = "a.Type";
        criteria.Fields[nameof(ProcessInfo.BillNo)] = "a.BillNo";
        criteria.Fields[nameof(ProcessInfo.Factory)] = "a.Factory";
        criteria.Fields[nameof(ProcessInfo.BillDate)] = "a.BillDate";
        return Database.QueryPageAsync<ProcessInfo>(sql, criteria);
    }

    public async Task<ProcessInfo> GetProcessAsync(string id)
    {
        ProcessInfo info = null;
        await Database.QueryActionAsync(async db =>
        {
            var sql = "select * from JxProcess where Id=@id";
            info = await db.QueryAsync<ProcessInfo>(sql, new { id });
            if (info == null)
            {
                // 新单:生成内部单号 JG+年月+4位序号
                var maxNo = await db.GetMaxRuleNoAsync<JxProcess>("Process", nameof(JxProcess.BillNo));
                info = new ProcessInfo
                {
                    Type = "Process",
                    BillNo = maxNo,
                    BillDate = DateTime.Now
                };
            }
        });
        return info;
    }

    public async Task<List<string>> GetFactoriesAsync()
    {
        var sql = "select Name from JxPartner where CompNo=@CompNo and Type=@Type order by Name";
        var partners = await Database.QueryListAsync<JxPartner>(sql, new { CompNo = CurrentUser?.CompNo ?? "1", Type = PartnerType.Supplier });
        return partners?.Select(p => p.Name).Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().ToList() ?? [];
    }

    /// <summary>
    /// 取得商品列表(品名规格关联库存)。
    /// </summary>
    public async Task<List<JxGoods>> GetGoodsListAsync()
    {
        var sql = "select * from JxGoods where CompNo=@CompNo order by Name";
        return await Database.QueryListAsync<JxGoods>(sql, new { CompNo = CurrentUser?.CompNo ?? "1" }) ?? [];
    }

    /// <summary>
    /// 按商品ID取商品信息(选择商品后回填品名规格等)。
    /// </summary>
    public async Task<JxGoods> GetGoodsByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;
        return await Database.QueryByIdAsync<JxGoods>(id);
    }

    public async Task<Result> DeleteProcessAsync(List<ProcessInfo> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        var database = Database;
        return await database.TransactionAsync(Language.Delete, async db =>
        {
            foreach (var item in infos)
            {
                await db.DeleteAsync<JxProcess>(item.Id);
            }
        });
    }

    public async Task<Result> SaveProcessAsync(UploadInfo<ProcessInfo> info)
    {
        var database = Database;
        var model = await database.QueryByIdAsync<JxProcess>(info.Model.Id);
        model ??= new JxProcess();
        model.FillModel(info.Model);
        model.Type ??= "Process";

        var vr = model.Validate(Context);
        if (!vr.IsValid)
            return vr;

        return await database.TransactionAsync(Language.Save, async db =>
        {
            if (model.IsNew)
            {
                var maxNo = await db.GetMaxRuleNoAsync<JxProcess>("Process", nameof(JxProcess.BillNo));
                model.BillNo = maxNo;
                model.BillDate ??= DateTime.Now;
            }
            model.Status ??= BizStatus.Save;
            await db.SaveAsync(model);
        });
    }
}
