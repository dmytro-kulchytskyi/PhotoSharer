using Microsoft.AspNet.Identity;
using PhotoSharer.Business.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Entities
{
    public class AppUser : IEntity, IUser<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }

        private IList<AppGroup> groups;
        public virtual IList<AppGroup> Groups
        {
            get
            {
                return groups ?? (groups = new List<AppGroup>());
            }
            set { groups = value; }
        }
    }
}