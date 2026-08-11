namespace JxcLite.WorkFlows;

public class OtherFeeFlow(Context context) : FlowBase(context)
{
    private const string FlowCode = "OtherFeeFlow";
    private const string FlowName = "其他费用流程";

    public static FlowBizInfo GetBizInfo(JxOtherFee model)
    {
        return new FlowBizInfo
        {
            FlowCode = FlowCode,
            FlowName = FlowName,
            BizId = model.Id,
            BizName = model.FeeNo,
            BizUrl = "",
            BizStatus = BizStatus.Save
        };
    }
}