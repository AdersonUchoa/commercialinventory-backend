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

        public Task<Categoria> UpdateAsync(Categoria categoria)
        {
            _categorias.Update(categoria);
            return Task.FromResult(categoria);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var categoria = await _categorias.FindAsync([id], cancellationToken);
            if (categoria == null) return false;
            _categorias.Remove(categoria);
            return true;
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

        public IQueryable<Categoria> GetAllAsync(string? search = null)
        {
            var query = _categorias.AsQueryable().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Nome.Contains(search));

            return query.OrderByDescending(c => c.Id);
        }
    }
}
