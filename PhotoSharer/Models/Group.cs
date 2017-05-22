using PhotoSharer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models
{
    public class AppGroup : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string InviteCode { get; set; }
        public virtual AppUser Creator { get; set; }

        private IList<AppUser> _Users;
        public virtual IList<AppUser> Users
        {
            get
            {
                return _Users ?? (_Users = new List<AppUser>());
            }
            set { _Users = value; }
        }
    }
}