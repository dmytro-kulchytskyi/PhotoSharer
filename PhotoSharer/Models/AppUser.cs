using Microsoft.AspNet.Identity;
using PhotoSharer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models
{
    public class AppUser : IEntity, IUser<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }

       
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }

        private IList<AppUser> _Groups;
        public virtual IList<AppUser> Groups
        {
            get
            {
                return _Groups ?? (_Groups = new List<AppUser>());
            }
            set { _Groups = value; }
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