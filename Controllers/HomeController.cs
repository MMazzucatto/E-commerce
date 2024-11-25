using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DesenvolvimentoWebCRUD.Models;

namespace DesenvolvimentoWebCRUD.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


       public IActionResult Home()
    {
        return View();
    }

    public IActionResult CatalogoProdutos(){

        return View();
    }

        public IActionResult AlterarProduto(){
        
        return View();
    }

        public IActionResult IncluirProduto(){
        
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
