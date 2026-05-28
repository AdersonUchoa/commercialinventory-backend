using Application.Responses.Categoria;

namespace Application.Responses.Produto
{
    public record ProdutoResponse(
        int Id,
        string Nome,
        string Descricao,
        string Preco,
        CategoriaResponse Categoria
    );
}
