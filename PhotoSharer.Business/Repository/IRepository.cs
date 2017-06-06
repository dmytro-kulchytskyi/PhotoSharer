using System;
using System.Threading.Tasks;

namespace PhotoSharer.Business.Repository
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid id);
        void Save(T instance);
        void Update(T instance);
        void Delete(T instance);
        void Delete(Guid id);
        bool IsExists(Guid id);
    }
}
