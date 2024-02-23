namespace core_newebpay.ViewModels
{
  public class NewebpayDetailView
  {
    public bool Status { get; set; }
    public NewebpayDetail? PaymentData { get; set; }
  }
  public class NewebpayDetail
  {
    public string? MerchantID { get; set; }
    public string? TradeInfo { get; set; }
    public string? TradeSha { get; set; }
    public string? Version { get; set; }
  }
}