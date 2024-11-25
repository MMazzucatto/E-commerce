using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DesenvolvimentoWebCRUD.Models;

namespace DesenvolvimentoWebCRUD.Controllers;

public class VerificacaoUsuarioController : Controller
{
    private readonly ILogger<VerificacaoUsuarioController> _logger;

    public VerificacaoUsuarioController(ILogger<VerificacaoUsuarioController> logger)
    {
        _logger = logger;
    }


            public IActionResult AdministrarPerfil(){

                return View();
            }

             public IActionResult TelaCadastroUsuario(){

                return View();
            }

            public IActionResult TelaEsqueceuSenha(){

                return View();
            }

            public IActionResult TelaLogin(){

                return View();
            }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}