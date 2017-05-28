using System;

namespace PhotoSharer.Business.Entities
{
    public class Photo
    {
        public virtual Guid Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string Source { get; set; }
        public virtual string ExternalId { get; set; }
        public virtual string Link { get; set; }
        public virtual AppUser User { get; set; }
    }
}