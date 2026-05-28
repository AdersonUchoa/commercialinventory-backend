using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> AddAsync(Categoria categoria, CancellationToken cancellationToken = default);
        Task<Categoria> UpdateAsync(Categoria categoria);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Categoria?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Categoria?> GetByIdNoTrackingAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<Categoria> GetAllAsync(string? search = null);
    }
}
