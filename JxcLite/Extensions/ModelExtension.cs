namespace JxcLite.Extensions;

static class ModelExtension
{
    internal static async Task<List<FactoryInfo>> GetFactoriesAsync(this IAdminService service)
    {
        var json = await service.GetConfigAsync(AppConstant.KeyFactory);
        return Utils.FromJson<List<FactoryInfo>>(json) ?? [];
    }

    internal static Task<Result> SaveFactoriesAsync(this IAdminService service, List<FactoryInfo> infos)
    {
        var info = new ConfigInfo { Key = AppConstant.KeyFactory, Value = infos };
        return service.SaveConfigAsync(info);
    }

    internal static void SetSaveVerify<T>(this BaseComponent form, FormModel<T> model)
        where T : class, IAppFlowInfo, new()
    {
        if (form.Context.HasButton(nameof(Language.Verify)) && model.Action != Language.Verify)
        {
            model.FooterRight = b => b.CheckBox(new InputModel<bool>
            {
                Label = "保存并审核",
                Value = model.Data.IsVerify,
                ValueChanged = form.Callback<bool>(v => model.Data.IsVerify = v)
            });
        }
    }
}