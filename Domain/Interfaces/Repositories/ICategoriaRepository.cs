using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> Add(Categoria categoria);
        Task<Categoria> Update(Categoria categoria);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Categoria?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Categoria?> GetByIdNoTrackingAsync(int id, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<Categoria> categorias, int total)> GetAllAsync(int pageIndex, int pageSize, string? searchName = null, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default);
        Task<bool> HasProductsAsync(int id, CancellationToken cancellationToken = default);
    }
}
