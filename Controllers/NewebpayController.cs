using core_newebpay.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    private readonly INewebpay _service;

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public NewebpayController(ILogger<NewebpayController> logger, INewebpay service)
    {
      _logger = logger;
      _service = service;
    }

    /// <summary>
    /// 建立Newebpay範例資料
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult SetNewebpayData()
    {
      var result = _service.CreateNewebpayDetail();
      return Ok(result);
    }
  }
}
