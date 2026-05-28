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

        public async Task<Produto> AddAsync(Produto Produto, CancellationToken cancellationToken = default)
        {
            await _produtos.AddAsync(Produto, cancellationToken);
            return Produto;
        }

        public Task<Produto> UpdateAsync(Produto Produto)
        {
            _produtos.Update(Produto);
            return Task.FromResult(Produto);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var Produto = await _produtos.FindAsync([id], cancellationToken);
            if (Produto == null) return false;
            _produtos.Remove(Produto);
            return true;
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

        public IQueryable<Produto> GetAllAsync(string? search = null)
        {
            var query = _produtos.AsQueryable().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Nome.Contains(search));

            return query
                .Include(p => p.Categoria)
                .OrderByDescending(c => c.Id);
        }
    }
}
