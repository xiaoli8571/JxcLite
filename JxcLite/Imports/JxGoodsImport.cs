namespace JxcLite.Imports;

class JxGoodsImport(ImportContext context) : ImportBase<JxGoods>(context)
{
    public override void InitColumns()
    {
        AddColumn(c => c.Code);
        AddColumn(c => c.Name);
        AddColumn(c => c.Category);
        AddColumn(c => c.Model);
        AddColumn(c => c.Producer);
        AddColumn(c => c.Unit);
        AddColumn(c => c.BuyPrice);
        AddColumn(c => c.SalePrice);
        AddColumn(c => c.SafeQty);
        AddColumn(c => c.Note);
    }

    public override async Task<Result> ExecuteAsync(AttachInfo file)
    {
        var models = new List<JxGoods>();
        var result = ImportHelper.ReadFile<JxGoods>(Context, file, item =>
        {
            var model = new JxGoods
            {
                Code = item.GetValue(c => c.Code),
                Name = item.GetValue(c => c.Name),
                Category = item.GetValue(c => c.Category),
                Model = item.GetValue(c => c.Model),
                Producer = item.GetValue(c => c.Producer),
                Unit = item.GetValue(c => c.Unit),
                BuyPrice = item.GetValueT(c => c.BuyPrice),
                SalePrice = item.GetValueT(c => c.SalePrice),
                SafeQty = item.GetValueT(c => c.SafeQty),
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
                await db.SaveAsync(item);
            }
        });
    }
}