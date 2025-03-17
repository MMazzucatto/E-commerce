using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DesenvolvimentoWebCRUD.Models;
using ProjetoEcommerce.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;

namespace DesenvolvimentoWebCRUD.Controllers;

public class HomeController : Controller
{

       public IActionResult Home()
        {

        var lista = ExibirProdutos();
        
        return View(lista);
        }

        public IActionResult CatalogoProdutos(){
        var lista = ExibirProdutos();

        return View(lista);
        }

        public IActionResult EditarProduto(int idProduto){
        
        var lista= ExibirProdutos();

        var ProdutoSelecionado = lista.Where(w=> w.id_produto == idProduto).ToList();

        return View("AlterarProduto",ProdutoSelecionado);
        }

        [HttpPost]
        public JsonResult AlterarProduto(string TituloEditado, string DescricaoEditada, string ValorEditado, int idProduto)
        {
            try
            {
                ProdutoEditar(TituloEditado, DescricaoEditada, ValorEditado, idProduto);

                return Json(new { sucesso = true, mensagem = "ok" });

            }
            catch (Exception ex)
            {

                return Json(new { Resultado = false, mensagem = ex.Message });
            }

        }


        public IActionResult AdicionarProduto(){          
        

        int? idUsuario = HttpContext.Session.GetInt32("idUsuario");

        ViewBag.idUsuario = idUsuario;



        return View("IncluirProduto");
        }
        [HttpPost]
            public JsonResult AdicionarProduto(string titulo, string descricao, string valor, int idUsuario)
            {
                try
                {
                    ProdutoAdicionar(titulo, descricao, valor, idUsuario);

                    return Json(new { sucesso = true, mensagem = "ok" });

                }
                catch (Exception ex)
                {

                    return Json(new { sucesso = false, mensagem = ex.Message });
                }

            }

    
        public JsonResult ExcluirProduto(int idProduto)
        {
            try
            {
                ProdutoExcluir(idProduto);

                return Json(new { sucesso = true, mensagem = "ok" });

            }
            catch (Exception ex)
            {

                return Json(new { Resultado = false, mensagem = ex.Message });
            }

        }

    #region conexaoBanco

private IEnumerable<Produto> ExibirProdutos()

    
{
    var _connectionString = "Server=DESKTOP-OB6NSEL;Database=E-commerce;Trusted_Connection=True;Encrypt=False;";  
    var items = new List<Produto>();  
    
    try
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();  
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM Produto"; 
            
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Produto()
                        {
                            id_produto = reader.GetInt32(0),
                            Nome = reader.GetString(1),  
                            Preco = reader.GetDecimal(2),
                            id_usuario = reader.GetInt32(3),   
                            Descricao = reader.GetString(4),  
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
        return new List<Produto>(); // Ou lide com o erro conforme necessário
    }
}
   
   private void ProdutoEditar(string TituloEditado, string descricaoEditada, string valorEditado, int idProduto)
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
                            UPDATE Produto 
                            SET Nome = @TituloEditado, 
                                Descricao = @descricaoEditada, 
                                Preco = @valorEditado 
                            WHERE id_produto = @id_produto";

                        command.Parameters.AddWithValue("@TituloEditado", TituloEditado);
                        command.Parameters.AddWithValue("@descricaoEditada", descricaoEditada);
                        command.Parameters.AddWithValue("@valorEditado", Convert.ToDecimal(valorEditado));
                        command.Parameters.AddWithValue("@id_produto", idProduto);

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

   private void ProdutoAdicionar(string titulo, string descricao, string valor, int idUsuario)
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
                            insert into Produto (nome, preco, id_usuario, Descricao)
                            values (@nome, @valor, @idUsuario, @descricao)";

                        command.Parameters.AddWithValue("@nome", titulo);
                        command.Parameters.AddWithValue("@descricao", descricao);
                        command.Parameters.AddWithValue("@valor", Convert.ToDecimal(valor));
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);

                       

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
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

  private void ProdutoExcluir(int idProduto)
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
                        command.CommandText = @" delete Produto where id_produto = @idProduto";

   
                        command.Parameters.AddWithValue("@idProduto", idProduto);

                       

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
#endregion



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

