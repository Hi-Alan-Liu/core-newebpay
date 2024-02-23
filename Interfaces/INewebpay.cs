using core_newebpay.ViewModels;

namespace core_newebpay.Interfaces
{
  public interface INewebpay
  {
    /// <summary>
    /// 建立Newebpay範例資料
    /// </summary>
    /// <returns></returns>
    NewebpayDetailView CreateNewebpayDetail();
  }
}
