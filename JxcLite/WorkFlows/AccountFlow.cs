namespace JxcLite.WorkFlows;

public class AccountFlow(Context context) : FlowBase(context)
{
    private const string FlowCode = "AccountFlow";
    private const string FlowName = "对账单流程";

    public static FlowBizInfo GetBizInfo(JxAccount model)
    {
        return new FlowBizInfo
        {
            FlowCode = FlowCode,
            FlowName = FlowName,
            BizId = model.Id,
            BizName = model.AccountNo,
            BizUrl = "",
            BizStatus = BizStatus.Save
        };
    }
}