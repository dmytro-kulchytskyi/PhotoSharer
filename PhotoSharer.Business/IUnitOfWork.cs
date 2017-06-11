namespace PhotoSharer.Business
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
    }
}
