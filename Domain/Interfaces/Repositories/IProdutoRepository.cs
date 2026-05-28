using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IProdutoRepository
    {
        Task<Produto> Add(Produto produto);
        Task<Produto> Update(Produto produto);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Produto?> GetByIdNoTrackingAsync(int id, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<Produto> produtos, int total)> GetAllAsync(int pageIndex, int pageSize, string? searchName = null, CancellationToken cancellationToken = default);
    }
}
