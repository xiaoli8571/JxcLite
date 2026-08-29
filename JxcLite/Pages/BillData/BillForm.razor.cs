namespace JxcLite.Pages.BillData;

partial class BillForm
{
    private BillService Service;
    private KUpload upload;
    private AntTable<JxBillList> table;

    // 关联客户订单
    private string SelectedOrderId;
    private List<string> OrderOptions = [];
    private List<OrderListInfo> OrderLists = [];

    private bool IsView => Model.IsView || Model.Data.IsVerifyForm;

    /// <summary>
    /// 实时预览数据(同步表单当前内容)。
    /// </summary>
    private BillInfo PreviewModel => new()
    {
        BillNo = Model.Data.BillNo,
        BillDate = Model.Data.BillDate,
        PartnerName = Model.Data.PartnerName,
        PartnerAddress = Model.Data.PartnerAddress,
        PartnerContact = Model.Data.PartnerContact,
        CreateBy = Model.Data.CreateBy,
        CreateTime = Model.Data.CreateTime,
        Type = Model.Data.Type,
        Factory = Model.Data.Factory,
        Lists = ListItems.ToList()
    };

    protected override JxBillList DefaultList => new()
    {
        HeadId = Model.Data.Id,
        SeqNo = ListItems.Count + 1
    };

    protected override async Task OnInitFormAsync()
    {
        await base.OnInitFormAsync();
        Service = await CreateServiceAsync<BillService>();
        this.SetSaveVerify(Model);
        // 加载客户订单(供关联导入,按单据编号显示)
        try
        {
            var orderService = await CreateServiceAsync<OrderService>();
            var orders = await orderService.QueryOrdersAsync(new PagingCriteria { PageIndex = 1, PageSize = 100 });
            OrderOptions = (orders?.PageData ?? []).Where(o => !string.IsNullOrWhiteSpace(o.CustomerNo)).Select(o => o.CustomerNo).ToList();
        }
        catch
        {
            OrderOptions = [];
        }
        Model.OnSaving = async data =>
        {
            data.Lists = ListItems;
            // 空数字字段填默认值(数据库 NOT NULL 约束)
            foreach (var item in ListItems)
            {
                item.Qty ??= 0;
                item.Price ??= 0;
                item.Amount ??= 0;
                item.TaxRate ??= 0;
                item.TaxAmount ??= 0;
                item.TotalAmount ??= 0;
                item.PkgQty ??= 0;
                item.Category ??= "其他";
                item.Unit ??= "件";
            }
            // 自动为手输明细行创建商品(GoodsId 为空时)
            await EnsureGoodsAsync();
            return true;
        };
    }

    /// <summary>
    /// 为明细行中 GoodsId 为空(手输)的行自动创建商品记录。
    /// </summary>
    private async Task EnsureGoodsAsync()
    {
        if (ListItems == null || ListItems.Count == 0)
            return;

        var baseService = await CreateServiceAsync<BaseDataService>();
        foreach (var item in ListItems)
        {
            if (!string.IsNullOrWhiteSpace(item.GoodsId))
                continue;

            var goods = new JxGoods
            {
                Category = string.IsNullOrWhiteSpace(item.Category) ? "其他" : item.Category,
                Code = $"M{DateTime.Now:yyyyMMddHHmmssfff}{Random.Shared.Next(100, 999)}",
                Name = string.IsNullOrWhiteSpace(item.Name) ? "手工商品" : item.Name,
                Model = item.Model,
                Unit = string.IsNullOrWhiteSpace(item.Unit) ? "件" : item.Unit,
                Producer = item.Producer,
                Note = item.Note,
                SalePrice = item.Price
            };
            var result = await baseService.SaveGoodsAsync(new UploadInfo<JxGoods> { Model = goods });
            if (result.IsValid)
            {
                item.GoodsId = goods.Id;
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            var data = await Service.GetBillAsync(Model.Data.Id ?? Model.Data.Type);
            data.IsVerify = Model.Data.IsVerify;
            data.IsVerifyForm = Model.Data.IsVerifyForm;
            // 加载工厂(公司)信息,用于打印预览
            var factories = await Admin.GetFactoriesAsync();
            data.Factory = factories?.FirstOrDefault();
            Model.Data = data;
            ListItems.AddRange(data.Lists);
            StateChanged();
        }
    }

    /// <summary>
    /// 选择客户后自动带出地址和联系人。
    /// </summary>
    private async Task OnPartnerChange(JxPartner partner)
    {
        Model.Data.PartnerId = partner?.Id;
        if (partner != null)
        {
            Model.Data.PartnerAddress = partner.Address;
            Model.Data.PartnerContact = partner.Contact;
        }
        await Task.CompletedTask;
    }

    /// <summary>
    /// 点击"导入明细"按钮:按选中的订单(单据编号)导入明细。
    /// </summary>
    private async Task OnImportOrder()
    {
        await OnOrderChange(SelectedOrderId);
    }

    /// <summary>
    /// 按单据编号导入明细到出货单。
    /// </summary>
    private async Task OnOrderChange(string customerNo)
    {
        if (string.IsNullOrWhiteSpace(customerNo))
            return;

        try
        {
            var orderService = await CreateServiceAsync<OrderService>();
            var order = await orderService.GetOrderByCustomerNoAsync(customerNo);
            if (order == null || order.Lists == null || order.Lists.Count == 0)
            {
                await UI.NoticeAsync("提示", "该订单没有明细");
                return;
            }

            // 单据编号 -> 业务单号;带出客户信息
            if (!string.IsNullOrWhiteSpace(order.CustomerNo))
                Model.Data.BillNo = order.CustomerNo;
            if (!string.IsNullOrWhiteSpace(order.CustomerName))
            {
                Model.Data.PartnerName = order.CustomerName;
                Model.Data.PartnerAddress = order.Address;
                Model.Data.PartnerContact = order.Contact;
                // 按客户名称匹配 PartnerId(保存时校验不能为空)
                try
                {
                    var partnerId = await orderService.GetCustomerIdByNameAsync(order.CustomerName);
                    if (!string.IsNullOrWhiteSpace(partnerId))
                        Model.Data.PartnerId = partnerId;
                }
                catch
                {
                }
            }
            // 单证日期继承客户订单的交货日期
            var deliveryDate = order.Lists.FirstOrDefault()?.DeliveryDate;
            if (!string.IsNullOrWhiteSpace(deliveryDate) &&
                DateTime.TryParse(deliveryDate, out var delivery))
            {
                Model.Data.BillDate = delivery;
            }

            // 填充到出货单明细(品名/规格/颜色/数量/单价/金额)
            foreach (var item in order.Lists)
            {
                if (string.IsNullOrWhiteSpace(item.GoodsName))
                    continue;
                ListItems.Add(new JxBillList
                {
                    HeadId = Model.Data.Id,
                    SeqNo = ListItems.Count + 1,
                    Category = "其他",
                    Name = item.GoodsName,
                    Model = item.Spec,
                    Color = item.Color,
                    Unit = item.Unit ?? "件",
                    Qty = item.Qty ?? 0,
                    Price = item.Price ?? 0,
                    Amount = item.Amount ?? 0,
                    PkgQty = 0,
                    TaxRate = 0,
                    TaxAmount = 0,
                    TotalAmount = item.Amount ?? 0
                });
            }
            StateChanged();
            await UI.NoticeAsync("导入成功", $"已从订单导入 {order.Lists.Count} 行明细");
        }
        catch (Exception ex)
        {
            await UI.NoticeAsync("导入失败", ex.Message);
        }
    }

    /// <summary>
    /// 打印当前预览内容。
    /// </summary>
    private async Task OnPrint()
    {
        var row1 = PreviewModel;
        await JS.PrintAsync<BillPrint>(f => f.Set(c => c.Model, row1));
    }

    /// <summary>
    /// 刷新预览(重新同步表单数据)。
    /// </summary>
    private void OnRefreshPreview()
    {
        StateChanged();
    }

    /// <summary>
    /// 导出送货单为 Excel(严格按手工模板格式)。
    /// </summary>
    private async Task OnExportExcel()
    {
        var data = Model.Data;
        var factory = data.Factory;
        var lists = ListItems ?? [];

        // 计算出货单序号(该单据在所有出货单中的第几个,如第5张=000005)
        var seqNo = "000001";
        if (data.Type == "销货" || data.Type == "Export")
        {
            try
            {
                var sql = $"select count(1) from JxBill where CompNo=@CompNo and Type='{BillType.Export}' and CreateTime<=(select CreateTime from JxBill where Id=@id)";
                var count = await Service.Database.ScalarAsync<int>(sql, new { CompNo = CurrentUser?.CompNo ?? "1", id = data.Id });
                seqNo = count.ToString("D6");
            }
            catch
            {
                // 计算失败时回退: 从 BillNo 尾部提取序号
                var billNo = data.BillNo;
                if (billNo != null && billNo.Length >= 4 && int.TryParse(billNo.Substring(billNo.Length - 4), out var last))
                    seqNo = last.ToString("D6");
            }
        }

        var sb = new System.Text.StringBuilder();
        // 页面设置:横向 A4
        sb.Append("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"><head><meta charset=\"UTF-8\">");
        sb.Append("<xml><x:ExcelWorkbook><x:WindowHeight>8000</x:WindowHeight><x:WindowWidth>10000</x:WindowWidth></x:ExcelWorkbook></xml>");
        sb.Append("<style>@page Section1 {size:842pt 595pt;margin:72pt 54pt 72pt 54pt;mso-page-orientation:landscape;}</style>");
        sb.Append("</head><body><div class=Section1>");
        // 表格:9列(规格列加宽,颜色列压缩)
        sb.Append("<table border=\"0\" cellpadding=\"2\" cellspacing=\"0\" style=\"border-collapse:collapse;font-family:宋体;font-size:11px;\">");
        sb.Append("<col style=\"width:4.43pt\"><col style=\"width:20.25pt\"><col style=\"width:8pt\"><col style=\"width:22pt\"><col style=\"width:6.13pt\"><col style=\"width:6.5pt\"><col style=\"width:9pt\"><col style=\"width:8pt\"><col style=\"width:12.5pt\">");

        // R1:公司名+送货单标题+NO(模板固定公司信息,标题26号宋体)
        sb.Append("<tr style=\"height:34.4pt;vertical-align:middle;\">");
        sb.Append("<td colspan=\"3\" style=\"font-size:16px;font-weight:bold;font-family:宋体;\">东帆纺织品有限公司</td>");
        sb.Append("<td colspan=\"4\" style=\"font-size:26px;font-weight:bold;text-align:center;font-family:宋体;\">送 货 单</td>");
        sb.Append("<td style=\"font-family:宋体;\">NO:</td><td style=\"font-family:宋体;mso-number-format:'\\@';\">").Append(EncodeHtml(seqNo)).Append("</td></tr>");

        // R2:地址+订单号(订单号=出货单号)
        sb.Append("<tr style=\"height:15pt;\">");
        sb.Append("<td colspan=\"5\">地址：东莞市虎门镇富民皮料市场B区868#</td>");
        sb.Append("<td>订单号:</td><td colspan=\"3\">").Append(EncodeHtml(data.BillNo)).Append("</td></tr>");

        // R3:TEL+日期
        sb.Append("<tr style=\"height:13.5pt;\">");
        sb.Append("<td colspan=\"5\">TEL:0769-85011996</td>");
        sb.Append("<td>日期：</td><td colspan=\"3\">").Append(data.BillDate?.ToString("yyyy.M.d")).Append("</td></tr>");

        // R4:Fax+结算币别
        sb.Append("<tr style=\"height:13.5pt;\">");
        sb.Append("<td colspan=\"5\">Fax:0769-88733625</td>");
        sb.Append("<td colspan=\"4\">结算币别:RMB</td></tr>");

        // R5:客户名称+地址+联系人
        sb.Append("<tr style=\"height:13.5pt;\">");
        sb.Append("<td colspan=\"3\">客户名称:").Append(EncodeHtml(data.PartnerName)).Append("</td>");
        sb.Append("<td colspan=\"3\">客户地址：").Append(EncodeHtml(data.PartnerAddress)).Append("</td>");
        sb.Append("<td colspan=\"3\">客户联系人：").Append(EncodeHtml(data.PartnerContact)).Append("</td></tr>");

        // R6:表头(9列,无备注)
        sb.Append("<tr style=\"height:27pt;text-align:center;font-weight:bold;\">");
        sb.Append("<td style=\"border:0.5pt solid black;\">序<br>号</td>");
        sb.Append("<td style=\"border:0.5pt solid black;\">产品名称</td>");
        sb.Append("<td style=\"border:0.5pt solid black;\">规格</td>");
        sb.Append("<td style=\"border:0.5pt solid black;\">颜色</td>");
        sb.Append("<td style=\"border:0.5pt solid black;\">件数</td>");
        sb.Append("<td style=\"border:0.5pt solid black;\">单位</td>");
        sb.Append("<td style=\"border:0.5pt solid black;\">数量</td>");
        sb.Append("<td style=\"border:0.5pt solid black;\">单价</td>");
        sb.Append("<td style=\"border:0.5pt solid black;\">金额</td>");
        sb.Append("</tr>");

        // 明细行(行高26,固定6行,不足补空行)
        var totalRows = lists.Count;
        for (int i = 0; i < 6; i++)
        {
            sb.Append("<tr style=\"height:26.1pt;\">");
            if (i < totalRows)
            {
                var item = lists[i];
                sb.Append("<td style=\"border:0.5pt solid black;text-align:center;\">").Append(item.SeqNo).Append("</td>");
                sb.Append("<td style=\"border:0.5pt solid black;\">").Append(EncodeHtml(item.Name)).Append("</td>");
                sb.Append("<td style=\"border:0.5pt solid black;text-align:center;\">").Append(EncodeHtml(item.Model)).Append("</td>");
                sb.Append("<td style=\"border:0.5pt solid black;\">").Append(EncodeHtml(item.Color)).Append("</td>");
                sb.Append("<td style=\"border:0.5pt solid black;text-align:center;\">").Append(item.PkgQty).Append("</td>");
                sb.Append("<td style=\"border:0.5pt solid black;text-align:center;\">").Append(EncodeHtml(item.Unit)).Append("</td>");
                sb.Append("<td style=\"border:0.5pt solid black;text-align:center;\">").Append(item.Qty).Append("</td>");
                sb.Append("<td style=\"border:0.5pt solid black;text-align:center;\">").Append(item.Price?.ToString("F2")).Append("</td>");
                sb.Append("<td style=\"border:0.5pt solid black;text-align:center;\">").Append(item.Amount?.ToString("F2")).Append("</td>");
            }
            else
            {
                for (int c = 0; c < 9; c++)
                    sb.Append("<td style=\"border:0.5pt solid black;\">&nbsp;</td>");
            }
            sb.Append("</tr>");
        }

        // 注:品质异议期(整行跨列)
        sb.Append("<tr style=\"height:30pt;\">");
        sb.Append("<td colspan=\"9\">注：本批货物品质异议期为3-4天，品质若有问题，请在裁切加工之前书面通知我公司，如已加工处理或超出品质异议期，我公司一概不负责.</td></tr>");

        // 制单+签名(行高51)
        sb.Append("<tr style=\"height:51pt;\">");
        sb.Append("<td colspan=\"4\">制单：").Append(EncodeHtml(string.IsNullOrWhiteSpace(data.PrintUser) ? data.CreateBy : data.PrintUser)).Append("</td>");
        sb.Append("<td colspan=\"5\">客户签名及盖章：</td></tr>");

        sb.Append("</table></div></body></html>");
        var html = sb.ToString();

        var js = "(function() { var html = " + System.Text.Json.JsonSerializer.Serialize(html) + "; var blob = new Blob(['\\ufeff' + html], { type: 'application/vnd.ms-excel;charset=utf-8' }); var url = URL.createObjectURL(blob); var a = document.createElement('a'); a.href = url; a.download = '送货单.xls'; document.body.appendChild(a); a.click(); document.body.removeChild(a); URL.revokeObjectURL(url); })();";
        await JS.RunVoidAsync(js);
    }

    /// <summary>
    /// HTML 编码。
    /// </summary>
    private static string EncodeHtml(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";
        return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }

    /// <summary>
    /// 打印纸上的编辑内容保存回表单。
    /// </summary>
    private async Task OnPreviewChanged(BillInfo info)
    {
        if (info == null)
            return;

        Model.Data.BillNo = info.BillNo;
        Model.Data.BillDate = info.BillDate;
        Model.Data.PartnerName = info.PartnerName;
        Model.Data.PartnerAddress = info.PartnerAddress;
        Model.Data.PartnerContact = info.PartnerContact;
        StateChanged();
        await Task.CompletedTask;
    }

    /// <summary>
    /// 从纸张编辑模式发起打印(只读模式打印,保留调整后的列宽)。
    /// </summary>
    private async Task OnPreviewPrint(BillInfo info, List<double> widths)
    {
        if (info == null)
            return;

        await JS.PrintAsync<BillPrint>(f => f.Set(c => c.Model, info)
                                              .Set(c => c.IsEditMode, false)
                                              .Set(c => c.ColWidths, widths));
    }

    private void OnRefBillNoChange(BillInfo info)
    {
        Model.Data.RefBillId = info?.Id;
        ListItems.Clear();
    }

    private async Task OnFilesChangedAsync(List<FileDataInfo> files)
    {
        Model.Files[nameof(BillInfo.Files)] = files;
        if (!Model.Data.IsNew)
        {
            await Model.SaveAsync(async d =>
            {
                Model.Files.Clear();
                await upload.SetValueAsync(d.Files);
            }, false);
        }
    }

    private void OnIsTaxChange(bool isTax)
    {
        foreach (var item in ListItems)
        {
            item.IsTax = isTax;
            OnPriceChange(item);
        }
        table?.ReloadData();
    }

    private void OnGoodsChange(JxGoods item, JxBillList row)
    {
        row.GoodsId = item?.Id;
        row.Code = item?.Code;
        row.Name = item?.Name;
        row.Category = item?.Category;
        row.Model = item?.Model;
        row.Producer = item?.Producer;
        row.Unit = item?.Unit;
        row.RefListId = item?.ListId;
        if (Model.Data.Type == BillType.ImportReturn || Model.Data.Type == BillType.ExportReturn)
        {
            row.Price = item?.Price;
        }
        table?.ReloadData();
    }

    //private void OnGoodsChange(int field, JxBillList row)
    //{
    //    if (field == 1 || field == 2)
    //        row.Amount = Utils.Round((row.Qty * row.Price) ?? 0, 2);
    //    else if (field == 3 && row.Qty > 0)
    //        row.Price = Utils.Round((row.Amount / row.Qty) ?? 0, 2);
    //    Model.Data.TotalAmount = Model.Data.Lists.Sum(l => l.Amount);
    //    table?.ReloadData();
    //}

    private void OnQtyChange(JxBillList row)
    {
        if (row.Qty == null || row.Qty == 0)
            return;

        // 没有单价时不联动(保持手工录入金额的习惯)
        if (row.Price == null)
            return;

        row.Amount = row.Price * row.Qty;
        row.IsTax = Model.Data.IsTax;
        var taxRate = (row.TaxRate ?? 0) * 0.01;
        if (row.IsTax)
        {
            //税额 = 含税金额 * 税率% / (1 + 税率%)；不含税金额 = 含税金额 - 税额
            var totalAmount = row.Amount ?? 0;
            row.TaxAmount = totalAmount * taxRate / (1 + taxRate);
            row.Amount = totalAmount - row.TaxAmount;
            row.TotalAmount = totalAmount;
        }
        else
        {
            //税额 = 不含税金额 * 税率%
            row.TaxAmount = row.Amount * taxRate;
            row.TotalAmount = row.Amount + row.TaxAmount;
        }
        row.Amount = Utils.Round(row.Amount, 2);
        row.TaxAmount = Utils.Round(row.TaxAmount, 2);
        row.TotalAmount = Utils.Round(row.TotalAmount, 2);
        Model.Data.SumAmount = ListItems.Sum(l => l.Amount);
        Model.Data.SumTaxAmount = ListItems.Sum(l => l.TaxAmount);
        Model.Data.SumTotalAmount = ListItems.Sum(l => l.TotalAmount);
        table?.ReloadData();
    }

    private void OnPriceChange(JxBillList row)
    {
        if (row.Qty == null || row.Qty == 0)
            return;

        row.IsTax = Model.Data.IsTax;
        var taxRate = (row.TaxRate ?? 0) * 0.01;
        if (row.IsTax)
        {
            //税额 = 含税金额 * 税率% / (1 + 税率%)
            row.TaxAmount = row.TotalAmount * taxRate / (1 + taxRate);
            row.Amount = row.TotalAmount - row.TaxAmount;
            row.Price = Utils.Round(row.Amount / row.Qty, 5);
            row.Amount = Utils.Round(row.Amount, 2);
        }
        else
        {
            //税额 = 不含税金额 * 税率%
            row.TaxAmount = row.Amount * taxRate;
            row.TotalAmount = row.Amount + row.TaxAmount;
            row.Price = Utils.Round(row.Amount / row.Qty, 5);
            row.TotalAmount = Utils.Round(row.TotalAmount, 2);
        }
        row.TaxAmount = Utils.Round(row.TaxAmount, 2);
        Model.Data.SumAmount = ListItems.Sum(l => l.Amount);
        Model.Data.SumTaxAmount = ListItems.Sum(l => l.TaxAmount);
        Model.Data.SumTotalAmount = ListItems.Sum(l => l.TotalAmount);
        table?.ReloadData();
    }
}