using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IProdutoRepository
    {
        Task<Produto> AddAsync(Produto produto, CancellationToken cancellationToken = default);
        Task<Produto> UpdateAsync(Produto produto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Produto?> GetByIdNoTrackingAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<Produto> GetAllAsync(string? search = null, CancellationToken cancellationToken = default);
    }
}
