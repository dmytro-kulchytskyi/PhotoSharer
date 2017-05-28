using PhotoSharer.Business.Entities;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Repository
{
    public interface ILoginRepository : IRepository<Login>
    {
        IList<Login> GetByUserId(Guid userId);
        Login GetByLoginInfo(Guid userId, string loginProvider, string providerKey);
    }
}
