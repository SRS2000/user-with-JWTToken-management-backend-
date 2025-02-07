using crud.Data;
using crud.Interface;

namespace crud.Services
{
    public class UnitofWork: IUnitOfWork
    {
        private readonly appdbcontext _context;
        private IUserRepository _userRepository;

        public UnitofWork(appdbcontext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            // Typically, rollbacks are handled automatically in Entity Framework during transaction scope.
            // If you're using a transaction manually, you'd do something like:
            _context.Database.CurrentTransaction.Rollback();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
