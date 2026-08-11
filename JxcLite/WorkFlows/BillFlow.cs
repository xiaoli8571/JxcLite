namespace JxcLite.WorkFlows;

public class BillFlow(Context context) : FlowBase(context)
{
    private const string FlowCode = "BillFlow";
    private const string FlowName = "业务单流程";

    public static FlowBizInfo GetBizInfo(JxBill model)
    {
        return new FlowBizInfo
        {
            FlowCode = FlowCode,
            FlowName = FlowName,
            BizId = model.Id,
            BizName = model.BillNo,
            BizUrl = "",
            BizStatus = BizStatus.Save
        };
    }
}