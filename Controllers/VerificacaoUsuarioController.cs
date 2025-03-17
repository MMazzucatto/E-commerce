using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DesenvolvimentoWebCRUD.Models;
using ProjetoEcommerce.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DesenvolvimentoWebCRUD.Controllers;

public class VerificacaoUsuarioController : Controller
{
    private readonly ILogger<VerificacaoUsuarioController> _logger;

    public VerificacaoUsuarioController(ILogger<VerificacaoUsuarioController> logger)
    {
        _logger = logger;
    }

            public IActionResult AdministrarPerfil(int idUsuario, string nomeUsuario){



                return View();
            }

             public IActionResult TelaCadastroUsuario(){

                return View();
            }
            [HttpPost]
            public IActionResult IncluirUsuario(string nome, string email, string senha){

                 try
                {
                    CadastrarUsuario(nome, email, senha);

                    return Json(new { sucesso = true, mensagem = "ok" });

                }
                catch (Exception ex)
                {

                    return Json(new { sucesso = false, mensagem = ex.Message });
                }

            }
            public IActionResult TelaEsqueceuSenha(){

                return View();
            }

            public IActionResult TelaLogin(){
                
                


                return View();
            }
            [HttpPost]
            public IActionResult TelaLogin(string email, string senha){


                var verificacao = VerificacaoUsuario(email, senha);
                var emailusuario = verificacao.Where(w => w.email == email).ToList();
                var verificar = emailusuario.Any();
                var id = verificacao.Select(s => s.id_usuario);
                var usuario = verificacao.FirstOrDefault();
                

               if (verificar == false){
                
                return Json(new { inexistente = true, mensagem = "inexistente" });

               }

               if (verificar == true){
                        HttpContext.Session.SetString("NomeUsuario", usuario.nome);
                        HttpContext.Session.SetInt32("idUsuario", usuario.id_usuario);
                        
               return Json(new { existente = true, mensagem = "existente" });

               }

                

                return View(verificacao);

            }


        private IEnumerable<Usuario> VerificacaoUsuario(string email, string senha)

    
        {
            var _connectionString = "Server=DESKTOP-OB6NSEL;Database=E-commerce;Trusted_Connection=True;Encrypt=False;";  
            var items = new List<Usuario>();  
    
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();  
                    using (var comm = connection.CreateCommand())
                    {
                        comm.CommandText = @"
                                select * from Usuario 
                                where email = @email and senha = @senha;"; 

                        comm.Parameters.AddWithValue("@email", email);
                        comm.Parameters.AddWithValue("@senha", senha);


            
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new Usuario()
                                {
                                    id_usuario = reader.GetInt32(0),
                                    nome = reader.GetString(1), 
                                    email = reader.GetString(2),
                                    senha = reader.GetString(3), 
                                };
                                items.Add(item);
                            }
                        }
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return new List<Usuario>();
            }
        }       
   

  private void CadastrarUsuario(string nome, string email, string senha)
{
    var _connectionString = "Server=DESKTOP-OB6NSEL;Database=E-commerce;Trusted_Connection=True;Encrypt=False;";
    
    try
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            insert into Usuario (nome, email, senha)
                            values (@nome, @email, @senha)";

                        command.Parameters.AddWithValue("@nome", nome);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@senha", senha);

                       

                        command.ExecuteNonQuery();
                    }

                    // Confirmar transação
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Reverter em caso de erro
                    transaction.Rollback();
                    Console.WriteLine("Erro ao editar o produto: " + ex.Message);
                    throw;
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro na conexão ou transação: " + ex.Message);
        throw;
    }
}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}