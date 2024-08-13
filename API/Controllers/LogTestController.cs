using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogTestController: ControllerBase{

    private readonly ILogger<LogTestController> _logger;

    public LogTestController(ILogger<LogTestController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get(){

        _logger.LogInformation("Teste");
        return Ok(new{ id = 1 });
    }

}