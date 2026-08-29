namespace JxcLite.Imports;

class JxPartnerImport(ImportContext context) : ImportBase<JxPartner>(context)
{
    public override void InitColumns()
    {
        AddColumn(c => c.Type);
        AddColumn(c => c.Name);
        AddColumn(c => c.ShortName);
        AddColumn(c => c.SccNo);
        AddColumn(c => c.Contact);
        AddColumn(c => c.Phone);
        AddColumn(c => c.Address);
        AddColumn(c => c.InvAddress);
        AddColumn(c => c.InvPhone);
        AddColumn(c => c.Bank);
        AddColumn(c => c.Account);
        AddColumn(c => c.Note);
    }

    public override async Task<Result> ExecuteAsync(AttachInfo file)
    {
        var models = new List<JxPartner>();
        var result = ImportHelper.ReadFile<JxPartner>(Context, file, item =>
        {
            var model = new JxPartner
            {
                Type = item.GetValue(c => c.Type),
                Name = item.GetValue(c => c.Name),
                ShortName = item.GetValue(c => c.ShortName),
                SccNo = item.GetValue(c => c.SccNo),
                Contact = item.GetValue(c => c.Contact),
                Phone = item.GetValue(c => c.Phone),
                Address = item.GetValue(c => c.Address),
                InvAddress = item.GetValue(c => c.InvAddress),
                InvPhone = item.GetValue(c => c.InvPhone),
                Bank = item.GetValue(c => c.Bank),
                Account = item.GetValue(c => c.Account),
                Note = item.GetValue(c => c.Note)
            };
            var vr = model.Validate(Context);
            if (!vr.IsValid)
                item.ErrorMessage = vr.Message;
            else
                models.Add(model);
        });

        if (!result.IsValid)
            return result;

        return await Database.TransactionAsync(Language.Import, async db =>
        {
            foreach (var item in models)
            {
                // 按类型+名称匹配已有记录:存在则更新,避免重复导入产生重复数据
                var model = await db.QueryAsync<JxPartner>(d => d.Type == item.Type && d.Name == item.Name);
                if (model != null)
                    item.Id = model.Id;
                await db.SaveAsync(item);
            }
        });
    }
}