using PhotoSharer.Business.Entities;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Repository
{
    public interface IGroupRepository : IRepository<AppGroup>
    {
        IList<AppGroup> GetByUserId(Guid userId, int skip = 0, int take = 0);
    }
}
