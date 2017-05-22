using NHibernate;
using PhotoSharer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharer.Models.Repository
{
   public  interface IRepository<T> where T :  class
    {
        T GetById(Guid id);
        Guid Save(T instance);
        void Delete(T instance);
        IQueryOver<T> GetAll();
    }
}
