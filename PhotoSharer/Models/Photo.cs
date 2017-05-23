using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharer.Models
{
    public class Photo
    {
        public virtual Guid Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string Source { get; set; }
        public virtual string ExternalId { get; set; }
        public virtual AppUser User { get; set; }
    }
}