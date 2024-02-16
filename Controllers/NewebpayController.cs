using core_newebpay.Commons;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace core_newebpay.Controllers
{
  /// <summary>
  /// Newebpay ���� API
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
      var merchantOrderNo = timeStamp + "_" + "�q��ID"; // ���u��謰�q��ID�A�ѱK���ΡA���i����(�W�h�ѦҤ��)
      var version = "2.0"; // ����
      var amt = 500; // �q����B
      var itemDesc = "500 ��§�]"; // �ӫ~��T
      var langType = "zh-tw"; // �y�t
      var loginType = "0"; // 0�����n�J�ŷs���y�|��
      var tradeLimit = "600"; // ���������
      var email = ""; // ���O�̫H�c�A�q���I�ڧ�����

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

      // �e�X���y�걵�θ�ơA���e�ݰe�ŷs��
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
