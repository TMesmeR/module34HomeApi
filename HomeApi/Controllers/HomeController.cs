using AutoMapper;
using HomeApi.Configuration;
using HomeApi.Contracts.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HomeApi.Controllers;
[ApiController]
[Route("[controller]")]
public class HomeController: ControllerBase
{
    private IOptions<HomeOptions> _options;
    private IMapper _mapper;

    public HomeController(IOptions<HomeOptions> options, IMapper mapper)
    {
        _options = options;
        _mapper = mapper;
    }
    /// <summary>
    /// Получает информацию о доме
    /// </summary>
    /// <returns>Данные конфигурации дома</returns>
    [HttpGet]
    [Route("/info")]
    public IActionResult GetInfo()
    {
        var infResponse = _mapper.Map<HomeOptions, InfoResponse>(_options.Value);
        return StatusCode(200, infResponse);
    }
}