namespace Domain.Entities
{
    public partial class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; }
        public decimal? Preco { get; set; }
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; } = null!;

        public Produto() { }

        public void Update(string? novoNome, string? novaDescriaco, decimal? novoPreco, int? novaCategoriaId)
        {
            Nome = novoNome ?? Nome;
            Descricao = novaDescriaco ?? Descricao;
            Preco = novoPreco ?? Preco;
            CategoriaId = novaCategoriaId ?? CategoriaId;
        }
    }
}
