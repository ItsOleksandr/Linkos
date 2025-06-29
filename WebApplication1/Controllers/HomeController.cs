using System.Text;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Uttillities;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index([FromQuery] string? area_name = null)
    {
        PowerOff? powerOff;
        Exception? exception = null;
        try
        {
            powerOff = Schedule.GetByArea(area_name);
        }
        catch
        {
            powerOff = null;
            if(area_name != null)  exception = new ArgumentException("Invalid area name");
        }
        
        return View(new IndexViewModel{ PowerOff = powerOff,Exception = exception});
    }

    public IActionResult Admin()
    {
        string? lastException = TempData["Exception"] as string;
        return View(new AdminViewModel{ ExceptionMessage = lastException });
    }

    [HttpPost("/admin/switch-area-electricity/{area_name}")]
    public IActionResult SwitchAreaElectricity(string? area_name = null)
    {
        string lastUrl = Request.Headers["Referer"].ToString();
        try
        {   
            var power = Schedule.GetByArea(area_name);
            power.UnscheduledOutage = !power.UnscheduledOutage;
        }
        catch
        {
            
        }
        
        return Redirect(lastUrl);
    }
    
    [HttpPost("/admin/import-excel")]
    public IActionResult ImportExcel(IFormFile file)
    {
        string lastUrl = Request.Headers["Referer"].ToString();

        try
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/uploads", "Schedule.xlsx");
            using FileStream fs = new(path, FileMode.OpenOrCreate);
            file.CopyTo(fs);
            ExcelImport.Import(path);
        }
        catch (Exception e)
        {
            TempData["Exception"] = $"Invalid Format excel:\n{e.Message}";
        }
        
        return Redirect(lastUrl);
    }

    [HttpGet("/admin/export-json")]
    public IActionResult Export()
    {
        string json = JsonSerializer.Serialize(Schedule.PowerOffs);
        return File(Encoding.UTF8.GetBytes(json),"application/json","Schedule.json");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}