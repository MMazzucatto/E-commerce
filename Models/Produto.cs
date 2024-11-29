namespace ProjetoEcommerce.Models
{
    public class Produto
    {
        public int id_produto { get; set; }
        public string? Nome { get; set; }
        public decimal  Preco { get; set; }
        public int id_usuario { get; set; }
        public string? Descricao { get; set; }

    }
}
