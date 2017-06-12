using PhotoSharer.Business.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Entities
{
    public class AppGroup : IEntity
    {
        public virtual Guid Id { get; set; }


        public virtual string Name { get; set; }

        public virtual string Link { get; set; }
        public virtual Guid CreatorId { get; set; }

      
        public virtual IList<AppUser> Users { get; set; }

    }
}