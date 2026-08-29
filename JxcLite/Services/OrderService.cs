namespace JxcLite.Services;

[WebApi, Service]
class OrderService(Context context) : ServiceBase(context)
{
    public Task<PagingResult<OrderInfo>> QueryOrdersAsync(PagingCriteria criteria)
    {
        var sql = "select * from JxOrder where CompNo=@CompNo";
        criteria.Fields[nameof(OrderInfo.OrderNo)] = "OrderNo";
        criteria.Fields[nameof(OrderInfo.CustomerName)] = "CustomerName";
        criteria.Fields[nameof(OrderInfo.OrderDate)] = "OrderDate";
        return Database.QueryPageAsync<OrderInfo>(sql, criteria);
    }

    public async Task<OrderInfo> GetOrderAsync(string id)
    {
        OrderInfo info = null;
        await Database.QueryActionAsync(async db =>
        {
            var sql = "select * from JxOrder where Id=@id";
            info = await db.QueryAsync<OrderInfo>(sql, new { id });
            if (info == null)
            {
                var maxNo = await db.GetMaxRuleNoAsync<JxOrder>(AppNoRule.Order, nameof(JxOrder.OrderNo));
                info = new OrderInfo
                {
                    OrderNo = maxNo,
                    OrderDate = DateTime.Now
                };
            }
            else
            {
                info.Lists = await db.QueryListAsync<OrderListInfo>("select * from JxOrderList where HeadId=@id", new { id = info.Id });
            }
        });
        return info;
    }

    public async Task<Result> DeleteOrdersAsync(List<OrderInfo> infos)
    {
        if (infos == null || infos.Count == 0)
            return Result.Error(Language.SelectOneAtLeast);

        var database = Database;
        return await database.TransactionAsync(Language.Delete, async db =>
        {
            foreach (var item in infos)
            {
                await db.DeleteAsync<JxOrderList>(d => d.HeadId == item.Id);
                await db.DeleteAsync<JxOrder>(item.Id);
            }
        });
    }

    public async Task<Result> SaveOrderAsync(UploadInfo<OrderInfo> info)
    {
        var database = Database;
        var model = await database.QueryByIdAsync<JxOrder>(info.Model.Id);
        model ??= new JxOrder();
        model.FillModel(info.Model);

        var vr = model.Validate(Context);
        if (!vr.IsValid)
            return vr;

        return await database.TransactionAsync(Language.Save, async db =>
        {
            if (model.IsNew)
            {
                var maxNo = await db.GetMaxRuleNoAsync<JxOrder>(AppNoRule.Order, nameof(JxOrder.OrderNo));
                model.OrderNo = maxNo;
                model.OrderDate ??= DateTime.Now;
            }
            model.Status ??= BizStatus.Save;
            await db.SaveAsync(model);
            info.Model.Id = model.Id;
            info.Model.OrderNo = model.OrderNo;

            // 保存明细
            await db.DeleteAsync<JxOrderList>(d => d.HeadId == model.Id);
            var lists = info.Model.Lists ?? [];
            var seq = 1;
            foreach (var item in lists)
            {
                if (string.IsNullOrWhiteSpace(item.GoodsName) && string.IsNullOrWhiteSpace(item.GoodsCode))
                    continue; // 跳过空行
                var list = new JxOrderList();
                list.FillModel(item);
                list.HeadId = model.Id;
                list.SeqNo = seq++;
                list.Qty ??= 0;
                list.Price ??= 0;
                list.Amount ??= 0;
                list.Unit ??= "件";
                await db.SaveAsync(list);
            }
        });
    }

    /// <summary>
    /// 取得订单明细(供出货单关联导入用)。
    /// </summary>
    public async Task<List<OrderListInfo>> GetOrderListsAsync(string orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            return [];
        return await Database.QueryListAsync<OrderListInfo>("select * from JxOrderList where HeadId=@id", new { id = orderId });
    }

    /// <summary>
    /// 按名称查询客户Id(供出货单关联导入用)。
    /// </summary>
    public async Task<string> GetCustomerIdByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;
        var partner = await Database.QueryAsync<JxPartner>("select * from JxPartner where CompNo=@compNo and Type=@type and Name=@name",
            new { compNo = CurrentUser?.CompNo ?? "1", type = PartnerType.Customer, name });
        return partner?.Id;
    }

    /// <summary>
    /// 按单据编号取得订单信息(供出货单关联导入用)。
    /// </summary>
    public async Task<OrderInfo> GetOrderByCustomerNoAsync(string customerNo)
    {
        if (string.IsNullOrWhiteSpace(customerNo))
            return null;
        var order = await Database.QueryAsync<JxOrder>("select * from JxOrder where CustomerNo=@customerNo", new { customerNo });
        if (order == null)
            return null;
        var info = new OrderInfo();
        info.FillModel(order);
        info.Lists = await Database.QueryListAsync<OrderListInfo>("select * from JxOrderList where HeadId=@id", new { id = order.Id });
        return info;
    }

    /// <summary>
    /// 按订单号取得订单信息(供出货单关联导入用)。
    /// </summary>
    public async Task<OrderInfo> GetOrderByNoAsync(string orderNo)
    {
        if (string.IsNullOrWhiteSpace(orderNo))
            return null;
        var order = await Database.QueryAsync<JxOrder>("select * from JxOrder where OrderNo=@orderNo", new { orderNo });
        if (order == null)
            return null;
        var info = new OrderInfo();
        info.FillModel(order);
        info.Lists = await Database.QueryListAsync<OrderListInfo>("select * from JxOrderList where HeadId=@id", new { id = order.Id });
        return info;
    }

    /// <summary>
    /// 按订单号取得订单明细(供出货单关联导入用)。
    /// </summary>
    public async Task<List<OrderListInfo>> GetOrderListsByNoAsync(string orderNo)
    {
        if (string.IsNullOrWhiteSpace(orderNo))
            return [];
        var order = await Database.QueryAsync<JxOrder>("select * from JxOrder where OrderNo=@orderNo", new { orderNo });
        if (order == null)
            return [];
        return await Database.QueryListAsync<OrderListInfo>("select * from JxOrderList where HeadId=@id", new { id = order.Id });
    }
}
