namespace Domain.Entities
{
    public partial class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; } = [];

        public Categoria() { }

        public Categoria(string nome, string? descricao = null)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public void Update(string? novoNome, string? novaDescricao)
        {
            Nome = novoNome ?? Nome;
            Descricao = novaDescricao ?? Descricao;
        }
    }
}
