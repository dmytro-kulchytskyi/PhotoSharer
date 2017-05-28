using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharer.Models.Repository.Interface
{
    public interface IGroupRepository : IRepository<AppGroup>
    {
        IList<AppGroup> GetAll();
        IList<AppGroup> GetUserGroups(Guid UserId);
        AppGroup GetGroupById(Guid Id);
    }
}
