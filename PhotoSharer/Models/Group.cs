using PhotoSharer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models
{
    public class Group : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string InviteCode { get; set; }
        public virtual Guid CreatorId { get; set; }
        public virtual IList<AppUser> Users { get; set; }
    }
}