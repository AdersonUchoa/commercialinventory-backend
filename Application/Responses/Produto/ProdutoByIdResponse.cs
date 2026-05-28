using Application.Responses.Categoria;

namespace Application.Responses.Produto
{
    public record ProdutoByIdResponse(
        int Id,
        string Nome,
        string Descricao,
        string Preco,
        CategoriaResponse Categoria
    );
}
