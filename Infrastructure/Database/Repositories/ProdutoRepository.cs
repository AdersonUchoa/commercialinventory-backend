using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CommercialInventoryDbContext _dbContext;
        private readonly DbSet<Produto> _produtos;

        public ProdutoRepository(CommercialInventoryDbContext dbContext)
        {
            _dbContext = dbContext;
            _produtos = _dbContext.Set<Produto>();
        }

        public async Task<Produto> AddAsync(Produto produto, CancellationToken cancellationToken = default)
        {
            await _produtos.AddAsync(produto, cancellationToken);
            return produto;
        }

        public Task<Produto> Update(Produto produto)
        {
            _produtos.Update(produto);
            return Task.FromResult(produto);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _produtos
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return deleted > 0;
        }

        public async Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _produtos
                .AsTracking()
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Produto?> GetByIdNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _produtos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<(IReadOnlyList<Produto> produtos, int total)> GetAllAsync(int pageIndex, int pageSize, string? searchName = null, CancellationToken cancellationToken = default)
        {
            var query = _produtos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(c => c.Nome.Contains(searchName));

            var count = await query.CountAsync(cancellationToken);

            var produtos = await query
                .Include(p => p.Categoria)
                .OrderByDescending(p => p.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (produtos, count);
        }
    }
}
