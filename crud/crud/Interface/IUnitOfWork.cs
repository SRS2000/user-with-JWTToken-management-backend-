namespace crud.Interface
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }  // Property to access the User repository
        Task CommitAsync();  // Commit the changes to the database
        Task RollbackAsync();  // Rollback the changes (if needed)
    }
}
