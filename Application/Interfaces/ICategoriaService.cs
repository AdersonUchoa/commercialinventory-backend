using Application.Pagination;
using Application.Requests.Categoria;
using Application.Responses;
using Application.Responses.Categoria;

namespace Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<ApiResponse<CategoriaResponse>> AddAsync(CreateCategoriaRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<CategoriaResponse>> UpdateAsync(int id, UpdateCategoriaRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse<CategoriaByIdResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedResult<CategoriaResponse>>> GetAllAsync(GetCategoriasRequest request, CancellationToken cancellationToken = default);
    }
}
