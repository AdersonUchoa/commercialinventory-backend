using Application.Pagination;
using Application.Requests.Produto;
using Application.Responses;
using Application.Responses.Produto;

namespace Application.Interfaces
{
    public interface IProdutoService
    {
        Task<ApiResponse<ProdutoResponse>> AddAsync(CreateProdutoRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<ProdutoResponse>> UpdateAsync(int id, UpdateProdutoRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse<ProdutoByIdResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedResult<ProdutoResponse>>> GetAllAsync(GetProdutosRequest request, CancellationToken cancellationToken = default);
    }
}
