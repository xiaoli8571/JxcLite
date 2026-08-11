namespace JxcLite.Services;

[WebApi, Service]
class ProcessService(Context context) : ServiceBase(context)
{
    public Task<PagingResult<ProcessInfo>> QueryProcessAsync(PagingCriteria criteria)
    {
        var sql = "select * from JxProcess where CompNo=@CompNo";
        var type = criteria.GetParameter<string>("Type");
        if (!string.IsNullOrWhiteSpace(type))
            sql += " and Type=@Type";
        criteria.Fields[nameof(ProcessInfo.Type)] = "Type";
        criteria.Fields[nameof(ProcessInfo.BillNo)] = "BillNo";
        criteria.Fields[nameof(ProcessInfo.Factory)] = "Factory";
        criteria.Fields[nameof(ProcessInfo.BillDate)] = "BillDate";
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
