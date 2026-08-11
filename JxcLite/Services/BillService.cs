namespace JxcLite.Services;

[WebApi, Service]
class BillService(Context context) : ServiceBase(context)
{
    private const string BillSQL = @"
select a.*, b.Name as PartnerName, c.BillNo as RefBillNo 
from JxBill a 
left join JxPartner b on a.PartnerId=b.Id 
left join JxBill c on a.RefBillId=c.Id";

    public Task<PagingResult<BillInfo>> QueryBillsAsync(PagingCriteria criteria)
    {
        var sql = $"{BillSQL} where a.CompNo=@CompNo";
        criteria.Fields[nameof(BillInfo.Type)] = "a.Type";
        criteria.Fields[nameof(BillInfo.PartnerId)] = "a.PartnerId";
        return Database.QueryPageAsync<BillInfo>(sql, criteria);
    }

    public async Task<BillInfo> GetBillAsync(string id)
    {
        BillInfo info = null;
        await Database.QueryActionAsync(async db =>
        {
            var sql = $"{BillSQL} where a.Id=@id";
            info = await db.QueryAsync<BillInfo>(sql, new { id });
            if (info == null)
            {
                var maxNo = await db.GetMaxBillNoAsync(id);
                info = new BillInfo
                {
                    Type = id,
                    BillNo = maxNo,
                    Status = BizStatus.Save,
                    BillDate = DateTime.Now
                };
            }
            info.Lists = await db.GetBillListsAsync(id);
        });
        return info;
    }

    public async Task<Result> DeleteBillsAsync(List<BillInfo> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        if (infos.Exists(d => d.Status != BizStatus.Save))
            return Result.Error(AppLanguage.TipOperateSaveRecord);

        var database = Database;
        var oldFiles = new List<string>();
        var result = await database.TransactionAsync(Language.Delete, async db =>
        {
            foreach (var item in infos)
            {
                if (AppConfig.OnBillDelete != null)
                    await AppConfig.OnBillDelete.Invoke(db, item.Id);
                await db.DeleteFilesAsync(item.Id, oldFiles);
                await db.DeleteFlowAsync(item.Id);

                var bill = await db.QueryByIdAsync<JxBill>(item.Id);
                if (bill != null)
                {
                    var lists = await db.QueryListAsync<JxBillList>(d => d.HeadId == item.Id);
                    if (lists != null && lists.Count > 0)
                    {
                        await db.ReverseStockAsync(bill, lists);
                    }
                }

                await db.DeleteAsync<JxBillList>(d => d.HeadId == item.Id);
                await db.DeleteAsync<JxBill>(item.Id);
            }
        });
        if (result.IsValid)
            AttachFile.DeleteFiles(oldFiles);
        return result;
    }

    public async Task<Result> UnVerifyBillsAsync(List<BillInfo> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        return await Database.TransactionAsync("反审", async db =>
        {
            foreach (var item in infos)
            {
                var model = await db.QueryByIdAsync<JxBill>(item.Id);
                if (model != null)
                {
                    model.Status = BizStatus.Save;
                    await db.SaveAsync(model);
                    await db.DeletePaymentAsync(model);
                    await db.AddFlowLogAsync(model.Id, "取消审核", "反审", "取消单证审核");
                }
            }
        });
    }

    public async Task<Result> SaveBillAsync(UploadInfo<BillInfo> info)
    {
        var database = Database;
        var model = await database.QueryByIdAsync<JxBill>(info.Model.Id);
        model ??= new JxBill();
        model.FillModel(info.Model);

        if (model.Status != BizStatus.Save)
            return Result.Error(AppLanguage.TipOperateSaveRecord);

        var vr = model.Validate(Context);
        if (!vr.IsValid)
            return vr;

        var fileFiles = info.Files?.GetAttachFiles(nameof(BillInfo.Files), "BillFiles");
        return await database.TransactionAsync(Language.Save, async db =>
        {
            if (model.IsNew)
            {
                // 从客户订单导入时已填入单据编号,则保留;否则自动生成
                if (string.IsNullOrWhiteSpace(model.BillNo) || model.BillNo.StartsWith("EX"))
                    model.BillNo = await db.GetMaxBillNoAsync(model.Type);
                await db.CreateFlowAsync(BillFlow.GetBizInfo(model));
            }
            if (info.Model.IsVerify)
            {
                model.Status = BizStatus.Verified;
                await db.AddPaymentAsync(model);
                await db.AddFlowLogAsync(model.Id, "单证审核", "审核", "单证已审核");
            }
            await db.AddFilesAsync(fileFiles, model.Id, key => model.Files = key);
            await db.SaveAsync(model);
            //更新表体数据
            if (info.Model.Lists != null && info.Model.Lists.Count > 0)
            {
                var index = 1;
                foreach (var item in info.Model.Lists)
                {
                    item.HeadId = model.Id;
                    item.SeqNo = index++;
                    await db.SaveAsync(item);
                }
                await db.AdjustStockAsync(model, info.Model.Lists);
            }
            info.Model.Id = model.Id;
        }, info.Model);
    }
}