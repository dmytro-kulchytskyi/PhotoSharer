using System;

namespace PhotoSharer.Business.Entities
{
    public class Photo
    {
        public virtual Guid Id { get; set; }
        public virtual string Provider { get; set; }
        public virtual string ExternalId { get; set; }
        public virtual string Link { get; set; }
        public virtual AppUser User { get; set; }
    }
}