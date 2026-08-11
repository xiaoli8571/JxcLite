namespace JxcLite.Services;

[WebApi, Service]
class BaseDataService(Context context) : ServiceBase(context)
{
    #region JxGoods
    public Task<PagingResult<JxGoods>> QueryGoodsesAsync(PagingCriteria criteria)
    {
        var sql = "select b.* from JxGoods b where b.CompNo=@CompNo";
        var type = criteria.GetParameter<string>("Type");
        if (!string.IsNullOrWhiteSpace(type) && type != BillType.Import)
        {
            if (type == BillType.Export || type == BillType.ImportReturn)
            {
                // 显示全部商品(现货贸易场景,不限制库存>0);有库存记录的带出库存数量
                sql = $@"
select '' as ListId,b.CreateTime,d.StockQty,b.SalePrice as Price,'' as BillNo
      ,b.Id,b.Category,b.Code,b.Name,b.Model,b.Producer,b.Unit 
from JxGoods b 
left join JxInventory d on d.GoodsId=b.Id
where b.CompNo=@CompNo";
            }
            else if (type == BillType.ExportReturn)
            {
                sql = $@"
select a.Id as ListId,a.CreateTime,a.StockQty,a.Price,c.BillNo
      ,b.Id,b.Category,b.Code,b.Name,b.Model,b.Producer,b.Unit 
from JxBillList a, JxGoods b, JxBill c 
where a.GoodsId=b.Id and a.HeadId=c.Id and a.CompNo=@CompNo and c.Type='{BillType.Export}'";
            }

            var billId = criteria.GetParameter<string>("BillId");
            if (!string.IsNullOrWhiteSpace(billId))
            {
                // 全部商品模式:不需要按单据过滤(避免引用不存在的 a 表)
                criteria.SetQuery("BillId", QueryType.Equal, billId);
            }
        }

        var key = criteria.GetParameter<string>("Key");
        if (!string.IsNullOrWhiteSpace(key))
        {
            sql += " and (b.Code like @Key or b.Name like @Key)";
            criteria.SetQuery("Key", $"%{key}%");
        }
        else
        {
            criteria.RemoveQuery("Key");
        }
        return Database.QueryPageAsync<JxGoods>(sql, criteria);
    }

    public Task<List<JxGoods>> GetGoodsesAsync()
    {
        return Database.QueryListAsync<JxGoods>(d => d.CompNo == CurrentUser.CompNo);
    }

    public async Task<Result> DeleteGoodsesAsync(List<JxGoods> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        var database = Database;
        var oldFiles = new List<string>();
        var result = await database.TransactionAsync(Language.Delete, async db =>
        {
            foreach (var item in infos)
            {
                await db.DeleteFilesAsync(item.Id, oldFiles);
                await db.DeleteAsync<JxGoods>(item.Id);
            }
        });
        if (result.IsValid)
            AttachFile.DeleteFiles(oldFiles);
        return result;
    }

    public async Task<Result> SaveGoodsAsync(UploadInfo<JxGoods> info)
    {
        var database = Database;
        var model = await database.QueryByIdAsync<JxGoods>(info.Model.Id);
        model ??= new JxGoods();
        model.FillModel(info.Model);

        var vr = model.Validate(Context);
        if (vr.IsValid)
        {
            if (await database.ExistsAsync<JxGoods>(d => d.Id != model.Id && d.Code == model.Code))
                vr.AddError($"商品[{model.Code}]已存在！");
        }
        if (!vr.IsValid)
            return vr;

        var fileFiles = info.Files?.GetAttachFiles(nameof(JxGoods.Files), "GoodsFiles");
        return await database.TransactionAsync(Language.Save, async db =>
        {
            await db.AddFilesAsync(fileFiles, model.Id, key => model.Files = key);
            await db.SaveAsync(model);
            info.Model.Id = model.Id;
        }, info.Model);
    }
    #endregion

    #region JxPartner
    public Task<PagingResult<JxPartner>> QueryPartnersAsync(PagingCriteria criteria)
    {
        var sql = "select * from JxPartner where CompNo=@CompNo and Type=@Type";
        var key = criteria.GetParameter<string>("Key");
        if (!string.IsNullOrWhiteSpace(key))
        {
            sql += " and Name like @Key";
            criteria.SetQuery("Key", $"%{key}%");
        }
        else
        {
            criteria.RemoveQuery("Key");
        }
        return Database.QueryPageAsync<JxPartner>(sql, criteria);
    }

    public async Task<Result> DeletePartnersAsync(List<JxPartner> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        var database = Database;
        var oldFiles = new List<string>();
        var result = await database.TransactionAsync(Language.Delete, async db =>
        {
            foreach (var item in infos)
            {
                await db.DeleteFilesAsync(item.Id, oldFiles);
                await db.DeleteAsync<JxPartner>(item.Id);
            }
        });
        if (result.IsValid)
            AttachFile.DeleteFiles(oldFiles);
        return result;
    }

    public async Task<Result> SavePartnerAsync(UploadInfo<JxPartner> info)
    {
        var database = Database;
        var model = await database.QueryByIdAsync<JxPartner>(info.Model.Id);
        model ??= new JxPartner();
        model.FillModel(info.Model);

        var vr = model.Validate(Context);
        if (vr.IsValid)
        {
            if (await database.ExistsAsync<JxPartner>(d => d.Id != model.Id && d.Name == model.Name))
                vr.AddError($"{model.Type}名称已存在！");
        }
        if (!vr.IsValid)
            return vr;

        var fileFiles = info.Files?.GetAttachFiles(nameof(JxPartner.Files), "PartnerFiles");
        return await database.TransactionAsync(Language.Save, async db =>
        {
            await db.AddFilesAsync(fileFiles, model.Id, key => model.Files = key);
            await db.SaveAsync(model);
            info.Model.Id = model.Id;
        }, info.Model);
    }
    #endregion
}