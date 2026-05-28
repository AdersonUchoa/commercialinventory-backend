using Domain.Interfaces.SeedWorks;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedWorks
{
    public sealed class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly CommercialInventoryDbContext _entityFrameworkContext;
        private bool _isDisposed;

        public UnitOfWork(CommercialInventoryDbContext entityFrameworkContext)
        {
            _entityFrameworkContext = entityFrameworkContext;
            _isDisposed = false;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _entityFrameworkContext.SaveChangesAsync(cancellationToken);

            if (_entityFrameworkContext.Database.CurrentTransaction != null)
                await _entityFrameworkContext.Database.CommitTransactionAsync(cancellationToken);
        }

        private void Disposing(bool disposing)
        {
            if (_isDisposed && disposing)
                _entityFrameworkContext.Dispose();

            _isDisposed = true;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _entityFrameworkContext.Database.RollbackTransactionAsync(cancellationToken);
        }

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default) =>
            _entityFrameworkContext.Database.BeginTransactionAsync(cancellationToken);

        public DbContext GetEfDbContext() => _entityFrameworkContext;

        public void Dispose() => Disposing(true);

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _entityFrameworkContext.SaveChangesAsync(cancellationToken);
    }
}
