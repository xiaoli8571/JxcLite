namespace JxcLite.Services;

[WebApi, Service]
class FinanceService(Context context) : ServiceBase(context)
{
    #region Account
    public Task<PagingResult<AccountInfo>> QueryAccountsAsync(PagingCriteria criteria)
    {
        return Database.Query<JxAccount>(criteria).ToPageAsync<AccountInfo>();
    }

    public async Task<AccountInfo> GetAccountAsync(string id)
    {
        AccountInfo info = null;
        await Database.QueryActionAsync(async db =>
        {
            info = await db.Query<JxAccount>().FirstAsync<AccountInfo>(d => d.Id == id);
            info ??= new AccountInfo
            {
                Type = id,
                Status = BizStatus.Save,
                AccountNo = await db.GetMaxAccountNoAsync(id),
                AccountDate = DateTime.Now
            };
        });
        return info;
    }

    public async Task<Result> DeleteAccountsAsync(List<AccountInfo> infos)
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
                await db.DeleteFilesAsync(item.Id, oldFiles);
                await db.DeleteFlowAsync(item.Id);
                await db.DeleteAsync<JxPayment>(d => d.BizId == item.Id);
                await db.DeleteAsync<JxAccountList>(d => d.HeadId == item.Id);
                await db.DeleteAsync<JxAccount>(item.Id);
            }
        });
        if (result.IsValid)
            AttachFile.DeleteFiles(oldFiles);
        return result;
    }

    public async Task<Result> UnVerifyAccountsAsync(List<AccountInfo> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        return await Database.TransactionAsync("反审", async db =>
        {
            foreach (var item in infos)
            {
                var model = await db.QueryByIdAsync<JxAccount>(item.Id);
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

    public async Task<Result> SaveAccountAsync(UploadInfo<AccountInfo> info)
    {
        var database = Database;
        var model = await database.QueryByIdAsync<JxAccount>(info.Model.Id);
        model ??= new JxAccount();
        model.FillModel(info.Model);

        if (!model.IsNew && model.Status != BizStatus.Save)
            return Result.Error(AppLanguage.TipOperateSaveRecord);

        var vr = model.Validate(Context);
        if (!vr.IsValid)
            return vr;

        var fileFiles = info.Files?.GetAttachFiles(nameof(JxAccount.Files), "AccountFiles");
        return await database.TransactionAsync(Language.Save, async db =>
        {
            if (model.IsNew)
            {
                model.AccountNo = await db.GetMaxAccountNoAsync(model.Type);
                await db.CreateFlowAsync(AccountFlow.GetBizInfo(model));
            }
            if (info.Model.IsVerify)
            {
                model.Status = BizStatus.Verified;
                await db.AddPaymentAsync(model);
                await db.AddFlowLogAsync(model.Id, "单证审核", "审核", "单证已审核");
            }
            await db.AddFilesAsync(fileFiles, model.Id, key => model.Files = key);
            await db.SaveAsync(model);
            info.Model.Id = model.Id;
        }, info.Model);
    }
    #endregion

    #region OtherFee
    public Task<PagingResult<OtherFeeInfo>> QueryOtherFeesAsync(PagingCriteria criteria)
    {
        return Database.Query<JxOtherFee>(criteria).ToPageAsync<OtherFeeInfo>();
    }

    public async Task<OtherFeeInfo> GetOtherFeeAsync(string id)
    {
        OtherFeeInfo info = null;
        await Database.QueryActionAsync(async db =>
        {
            info = await db.Query<JxOtherFee>().FirstAsync<OtherFeeInfo>(d => d.Id == id);
            info ??= new OtherFeeInfo
            {
                Type = "收入",
                Status = BizStatus.Save,
                FeeNo = await db.GetMaxOtherFeeNoAsync(),
                FeeDate = DateTime.Now
            };
        });
        return info;
    }

    public async Task<Result> DeleteOtherFeesAsync(List<OtherFeeInfo> infos)
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
                await db.DeleteFilesAsync(item.Id, oldFiles);
                await db.DeleteFlowAsync(item.Id);
                await db.DeleteAsync<JxPayment>(d => d.BizId == item.Id);
                await db.DeleteAsync<JxOtherFee>(item.Id);
            }
        });
        if (result.IsValid)
            AttachFile.DeleteFiles(oldFiles);
        return result;
    }

    public async Task<Result> UnVerifyOtherFeesAsync(List<OtherFeeInfo> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        return await Database.TransactionAsync("反审", async db =>
        {
            foreach (var item in infos)
            {
                var model = await db.QueryByIdAsync<JxOtherFee>(item.Id);
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

    public async Task<Result> SaveOtherFeeAsync(UploadInfo<OtherFeeInfo> info)
    {
        var database = Database;
        var model = await database.QueryByIdAsync<JxOtherFee>(info.Model.Id);
        model ??= new JxOtherFee();
        model.FillModel(info.Model);

        if (!model.IsNew && model.Status != BizStatus.Save)
            return Result.Error(AppLanguage.TipOperateSaveRecord);

        var vr = model.Validate(Context);
        if (!vr.IsValid)
            return vr;

        var fileFiles = info.Files?.GetAttachFiles(nameof(JxOtherFee.Files), "OtherFeeFiles");
        return await database.TransactionAsync(Language.Save, async db =>
        {
            if (model.IsNew)
            {
                model.FeeNo = await db.GetMaxOtherFeeNoAsync();
                await db.CreateFlowAsync(OtherFeeFlow.GetBizInfo(model));
            }
            if (info.Model.IsVerify)
            {
                model.Status = BizStatus.Verified;
                await db.AddPaymentAsync(model);
                await db.AddFlowLogAsync(model.Id, "单证审核", "审核", "单证已审核");
            }
            await db.AddFilesAsync(fileFiles, model.Id, key => model.Files = key);
            await db.SaveAsync(model);
            info.Model.Id = model.Id;
        }, info.Model);
    }
    #endregion

    #region Payment
    public Task<PagingResult<JxPayment>> QueryPaymentsAsync(PagingCriteria criteria)
    {
        return Database.QueryPageAsync<JxPayment>(criteria);
    }

    public async Task<JxPayment> GetPaymentAsync(string id)
    {
        JxPayment info = null;
        await Database.QueryActionAsync(async db =>
        {
            info = await db.QueryByIdAsync<JxPayment>(id);
            info ??= new JxPayment
            {
                Type = id,
                Status = BizStatus.Save,
                PaymentNo = await db.GetMaxPaymentNoAsync(id),
                PaymentDate = DateTime.Now
            };
        });
        return info;
    }

    public async Task<Result> DeletePaymentsAsync(List<JxPayment> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        var database = Database;
        var result = await database.TransactionAsync(Language.Delete, async db =>
        {
            foreach (var item in infos)
            {
                var model = await db.QueryByIdAsync<JxPayment>(item.Id);
                if (model == null)
                    continue;

                if (model.Records != null && model.Records.Count > 0)
                    throw new Exception($"收付款单[{model.PaymentNo}]已发生收付款记录，不能删除！");

                await db.DeleteAsync(model);
            }
        });
        return result;
    }

    public async Task<Result> SavePaymentAsync(UploadInfo<JxPayment> info)
    {
        var database = Database;
        var model = await database.QueryByIdAsync<JxPayment>(info.Model.Id);
        model ??= new JxPayment();
        model.FillModel(info.Model);

        if (!model.IsNew && model.Status != BizStatus.Save)
            return Result.Error(AppLanguage.TipOperateSaveRecord);

        var vr = model.Validate(Context);
        if (!vr.IsValid)
            return vr;

        var fileFiles = info.Files?.GetAttachFiles(nameof(JxPayment.Files), "PaymentFiles");
        return await database.TransactionAsync(Language.Save, async db =>
        {
            if (model.IsNew)
                model.PaymentNo = await db.GetMaxPaymentNoAsync(model.Type);

            // 服务端重算已收付/剩余金额,防止超收超付
            var paid = model.Records?.Sum(x => x.Amount ?? 0) ?? 0;
            if (paid > (model.TotalAmount ?? 0))
                throw new Exception("已收付金额不能大于总金额！");

            model.PaidAmount = paid;
            model.RemainAmount = (model.TotalAmount ?? 0) - paid;
            await db.AddFilesAsync(fileFiles, model.Id, key => model.Files = key);
            await db.SaveAsync(model);
            info.Model.Id = model.Id;
        }, info.Model);
    }

    /// <summary>
    /// 对账单添加对账明细(关联业务单据)。
    /// </summary>
    public async Task<Result> SaveAccountBillAsync(UploadInfo<AccountBillInfo> info)
    {
        var model = info.Model;
        if (string.IsNullOrWhiteSpace(model.AccountId))
            return Result.Error("请先保存对账单表头！");

        if (string.IsNullOrWhiteSpace(model.BillId))
            return Result.Error("请选择要关联的单据！");

        var database = Database;
        if (await database.ExistsAsync<JxAccountList>(d => d.HeadId == model.AccountId && d.BillId == model.BillId))
            return Result.Error("该单据已在对账明细中！");

        return await database.TransactionAsync(Language.Save, async db =>
        {
            await db.SaveAsync(new JxAccountList { HeadId = model.AccountId, BillId = model.BillId });
        });
    }

    /// <summary>
    /// 对账单移除对账明细(仅移除关联关系,不删除业务单据)。
    /// </summary>
    public async Task<Result> DeleteAccountBillsAsync(string accountId, List<BillInfo> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        var database = Database;
        return await database.TransactionAsync(Language.Delete, async db =>
        {
            foreach (var item in infos)
                await db.DeleteAsync<JxAccountList>(d => d.HeadId == accountId && d.BillId == item.Id);
        });
    }
    #endregion
}