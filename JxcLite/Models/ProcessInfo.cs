namespace JxcLite.Models;

/// <summary>
/// 加工单信息类。
/// </summary>
public class ProcessInfo : EntityBase, IAppFlowInfo
{
    /// <summary>
    /// 取得或设置是否审核。
    /// </summary>
    public bool IsVerify { get; set; }

    /// <summary>
    /// 取得或设置是否审核表单。
    /// </summary>
    public bool IsVerifyForm { get; set; }

    /// <summary>
    /// 取得或设置是否新单。
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// 取得或设置单据状态。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("单据状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置单据类型(加工=Process,加工退货=ProcessReturn)。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单据类型")]
    public string Type { get; set; }

    /// <summary>
    /// 取得或设置内部单号。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120, IsQuery = true, IsViewLink = true)]
    [DisplayName("内部单号")]
    public string BillNo { get; set; }

    /// <summary>
    /// 取得或设置下单日期。
    /// </summary>
    [Column(Width = 100, Type = FieldType.Date)]
    [DisplayName("下单日期")]
    public DateTime? BillDate { get; set; }

    /// <summary>
    /// 取得或设置加工厂加工单号。
    /// </summary>
    [MaxLength(100)]
    [Column(Width = 120)]
    [DisplayName("加工厂加工单号")]
    public string FactoryNo { get; set; }

    /// <summary>
    /// 取得或设置加工工厂。
    /// </summary>
    [MaxLength(100)]
    [Column(Width = 120)]
    [DisplayName("加工工厂")]
    public string Factory { get; set; }

    /// <summary>
    /// 取得或设置加工涂料。
    /// </summary>
    [MaxLength(100)]
    [DisplayName("加工涂料")]
    public string Coating { get; set; }

    /// <summary>
    /// 取得或设置品名规格。
    /// </summary>
    [MaxLength(100)]
    [DisplayName("品名规格")]
    public string GoodsSpec { get; set; }

    /// <summary>
    /// 取得或设置胚布幅宽。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("胚布幅宽")]
    public string ClothWidth { get; set; }

    /// <summary>
    /// 取得或设置颜色。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("颜色")]
    public string Color { get; set; }

    /// <summary>
    /// 取得或设置投坯数量Y。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("投坯数量Y")]
    public string InputQty { get; set; }

    /// <summary>
    /// 取得或设置要求交期。
    /// </summary>
    [DisplayName("要求交期")]
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// 取得或设置加工工艺要求。
    /// </summary>
    [MaxLength(500)]
    [DisplayName("加工工艺要求")]
    public string ProcessReq { get; set; }

    /// <summary>
    /// 取得或设置对色光源。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("对色光源")]
    public string LightSource { get; set; }

    /// <summary>
    /// 取得或设置主灯。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("主灯")]
    public string MainLight { get; set; }

    /// <summary>
    /// 取得或设置主灯勾选状态。
    /// </summary>
    public bool MainLightChecked { get => MainLight == "✓"; set => MainLight = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置辅灯。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("辅灯")]
    public string AuxLight { get; set; }

    /// <summary>
    /// 取得或设置辅灯勾选状态。
    /// </summary>
    public bool AuxLightChecked { get => AuxLight == "✓"; set => AuxLight = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置对色色卡。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("对色色卡")]
    public string ColorCard { get; set; }

    /// <summary>
    /// 取得或设置对色色卡勾选状态。
    /// </summary>
    public bool ColorCardChecked { get => ColorCard == "✓"; set => ColorCard = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置干摩擦牢度。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("干摩擦牢度")]
    public string DryRubbing { get; set; }

    /// <summary>
    /// 取得或设置湿摩擦牢度。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("湿摩擦牢度")]
    public string WetRubbing { get; set; }

    /// <summary>
    /// 取得或设置PH值。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("PH值")]
    public string PhValue { get; set; }

    /// <summary>
    /// 取得或设置日晒牢度。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("日晒牢度")]
    public string SunRubbing { get; set; }

    /// <summary>
    /// 取得或设置耐黄度。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("耐黄度")]
    public string YellowResist { get; set; }

    /// <summary>
    /// 取得或设置皂洗牢度。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("皂洗牢度")]
    public string SoapRubbing { get; set; }

    /// <summary>
    /// 取得或设置本光(勾选框,勾选=✓)。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("本光")]
    public string BenGuang { get; set; }

    /// <summary>
    /// 取得或设置本光勾选状态。
    /// </summary>
    public bool BenGuangChecked { get => BenGuang == "✓"; set => BenGuang = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置丝光(勾选框,勾选=✓)。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("丝光")]
    public string SiGuang { get; set; }

    /// <summary>
    /// 取得或设置丝光勾选状态。
    /// </summary>
    public bool SiGuangChecked { get => SiGuang == "✓"; set => SiGuang = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置硫化(勾选框,勾选=✓)。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("硫化")]
    public string LiuHua { get; set; }

    /// <summary>
    /// 取得或设置硫化勾选状态。
    /// </summary>
    public bool LiuHuaChecked { get => LiuHua == "✓"; set => LiuHua = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置活性(勾选框,勾选=✓)。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("活性")]
    public string HuoXing { get; set; }

    /// <summary>
    /// 取得或设置活性勾选状态。
    /// </summary>
    public bool HuoXingChecked { get => HuoXing == "✓"; set => HuoXing = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置碧纹洗水(勾选框,勾选=✓)。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("碧纹洗水")]
    public string BiJiXiShui { get; set; }

    /// <summary>
    /// 取得或设置碧纹洗水勾选状态。
    /// </summary>
    public bool BiJiXiShuiChecked { get => BiJiXiShui == "✓"; set => BiJiXiShui = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置涂料。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("涂料")]
    public string TuLiao { get; set; }

    /// <summary>
    /// 取得或设置涂料勾选状态。
    /// </summary>
    public bool TuLiaoChecked { get => TuLiao == "✓"; set => TuLiao = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置预缩。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("预缩")]
    public string YuSuo { get; set; }

    /// <summary>
    /// 取得或设置预缩勾选状态。
    /// </summary>
    public bool YuSuoChecked { get => YuSuo == "✓"; set => YuSuo = value ? "✓" : ""; }

    /// <summary>
    /// 取得或设置成品宽幅。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("成品宽幅")]
    public string FinishWidth { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [MaxLength(500)]
    [DisplayName("备注")]
    public string Note { get; set; }

    /// <summary>
    /// 取得或设置审核。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("审核")]
    public string Auditor { get; set; }

    /// <summary>
    /// 取得或设置制单。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("制单")]
    public string Maker { get; set; }
}
