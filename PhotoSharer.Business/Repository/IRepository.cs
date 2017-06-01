using System;

namespace PhotoSharer.Business.Repository
{
    public  interface IRepository<T> where T :  class
    {
        T GetById(Guid id);
        Guid Save(T instance);
        void Update(T instance);
        void Delete(T instance);
    }
}
