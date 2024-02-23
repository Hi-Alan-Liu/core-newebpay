using core_newebpay.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    private readonly INewebpay _service;

    /// <summary>
    /// �غc�l
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public NewebpayController(ILogger<NewebpayController> logger, INewebpay service)
    {
      _logger = logger;
      _service = service;
    }

    /// <summary>
    /// �إ�Newebpay�d�Ҹ��
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
