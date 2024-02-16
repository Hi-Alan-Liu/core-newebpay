using core_newebpay.Commons;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace core_newebpay.Controllers
{
  /// <summary>
  /// Newebpay 相關 API
  /// </summary>
  [ApiController]
  [Route("api/newebpay")]
  public class NewebpayController : ControllerBase
  {
    private readonly ILogger<NewebpayController> _logger;
    private readonly IConfiguration _configuration;

    private readonly string _hashKey;
    private readonly string _hashIv;
    private readonly string _merchantId;

    public NewebpayController(ILogger<NewebpayController> logger, IConfiguration configuration)
    {
      _logger = logger;
      _configuration = configuration;
      _merchantId = _configuration.GetValue<string>("Newebpay:MerchantId");
      _hashKey = _configuration.GetValue<string>("Newebpay:HashKey");
      _hashIv = _configuration.GetValue<string>("Newebpay:HashIv");
    }

    [HttpPost]
    public IActionResult SetNewebpayData()
    {
      var timeStamp = ((int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
      var notifyUrl = "";
      var returnUrl = "";
      var merchantOrderNo = timeStamp + "_" + "訂單ID"; // 底線後方為訂單ID，解密比對用，不可重覆(規則參考文件)
      var version = "2.0"; // 版本
      var amt = 500; // 訂單金額
      var itemDesc = "500 元禮包"; // 商品資訊
      var langType = "zh-tw"; // 語系
      var loginType = "0"; // 0不須登入藍新金流會員
      var tradeLimit = "600"; // 交易限制秒數
      var email = ""; // 消費者信箱，通知付款完成用

      List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>()
      {
        new("MerchantID", _merchantId),
        new("MerchantOrderNo", merchantOrderNo),
        new("TimeStamp", timeStamp),
        new("RespondType", "JSON"),
        new("LangType", langType),
        new("Version", version),
        new("Amt", amt.ToString()),
        new("ItemDesc", itemDesc),
        new("NotifyURL", notifyUrl),
        new("ReturnURL", returnUrl),
        new("LoginType", loginType),
        new("CREDIT", "1"),
        new("TradeLimit", tradeLimit),
        new("Email", email),
      };

      var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
      var tradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, _hashKey, _hashIv);
      var tradeSha = CryptoUtil.EncryptSHA256($"HashKey={_hashKey}&{tradeInfo}&HashIV={_hashIv}");

      // 送出金流串接用資料，給前端送藍新用
      return Ok(new
      {
        Status = true,
        PaymentData = new
        {
          MerchantID = _merchantId,
          TradeInfo = tradeInfo,
          TradeSha = tradeSha,
          Version = version
        }
      });
    }
  }
}
