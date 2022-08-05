using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RedisWork.Models;
using RedisWork.Services;

namespace RedisWork.Controllers;

public class HomeController : Controller
{
    private List<Personal> _personals;
    private readonly ILogger<HomeController> _logger;
    private readonly ICacheService _cacheService;

    public HomeController(ILogger<HomeController> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
        
        InitPersonal();
    }

    public IActionResult Index()
    {
        if (_cacheService.Any("Personals"))
        {
            var personals = _cacheService.Get<List<Personal>>("Personals");
            return View(personals);
        }
        _cacheService.Add("Personals", _personals);
        return View(_personals);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    private void InitPersonal()
    {
        if (_personals is null)
        {
            _personals = new List<Personal>()
            {
                new Personal() { id = 1, name = "Deniz", surname = "Yilmaz" },
                new Personal() { id = 2, name = "Burak", surname = "Kuyucu" }
            };
        }
    }
}