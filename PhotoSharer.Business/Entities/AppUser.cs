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
        public virtual string ExternalId { get; set; }

        private IList<AppGroup> groups;
        public virtual IList<AppGroup> Groups
        {
            get
            {
                return groups ?? (groups = new List<AppGroup>());
            }
            set { groups = value; }
        }

        private IList<Login> _Logins;
        public virtual IList<Login> Logins
        {
            get
            {
                return _Logins ?? (_Logins = new List<Login>());
            }
            set { _Logins = value; }
        }
    }
}