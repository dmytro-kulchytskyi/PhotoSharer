using PhotoSharer.Business.Entities.Interfaces;
using System;

namespace PhotoSharer.Business.Entities
{
    public class Login : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual AppUser User { get; set; }
    }
}