using Application.Responses.Produto;

namespace Application.Responses.Categoria
{
    public record CategoriaByIdResponse(
        int Id,
        string Nome,
        string Descricao,
        List<ProdutoResponse> Produtos
    );
}
