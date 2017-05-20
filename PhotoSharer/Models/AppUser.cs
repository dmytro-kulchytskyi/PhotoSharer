using PhotoSharer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models
{
    public class AppUser : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Group> Groups { get; set; }
    }
}