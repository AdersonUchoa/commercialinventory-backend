namespace Application.Requests.Produto
{
    public class GetProdutosRequest
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? SearchName { get; set; }
        public int? CategoriaId { get; set; }
    }
}
