using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models
{
    public class AppUser
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
}