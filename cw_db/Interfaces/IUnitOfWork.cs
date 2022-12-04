namespace cw_db.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> Repo<T>() where T : class;

        Task<int> Complete();
    }
}
