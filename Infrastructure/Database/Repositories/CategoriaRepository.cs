using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly CommercialInventoryDbContext _context;
        private readonly DbSet<Categoria> _categorias;

        public CategoriaRepository(CommercialInventoryDbContext context)
        {
            _context = context;
            _categorias = _context.Set<Categoria>();
        }

        public async Task<Categoria> AddAsync(Categoria categoria, CancellationToken cancellationToken = default)
        {
            await _categorias.AddAsync(categoria, cancellationToken);
            return categoria;
        }

        public Task<Categoria> Update(Categoria categoria)
        {
            _categorias.Update(categoria);
            return Task.FromResult(categoria);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _categorias
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return deleted > 0;
        }

        public async Task<Categoria?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _categorias
                .AsTracking()
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Categoria?> GetByIdNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _categorias
                .AsNoTracking()
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<(IReadOnlyList<Categoria> categorias, int total)> GetAllAsync(int pageIndex, int pageSize, string? searchName = null, CancellationToken cancellationToken = default)
        {
            var query = _categorias.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(c => c.Nome.Contains(searchName));

            var count = await query.CountAsync(cancellationToken);

            var categorias = await query
                .OrderByDescending(c => c.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (categorias, count);
        }

        public async Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default)
        {
            return await _categorias.AnyAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<bool> HasProductsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _categorias.AnyAsync(c => c.Id == id && c.Produtos.Any(), cancellationToken);
        }
    }
}
