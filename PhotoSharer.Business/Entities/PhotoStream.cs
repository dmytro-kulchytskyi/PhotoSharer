using PhotoSharer.Business.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharer.Business.Entities
{
    public class PhotoStream : IEntity
    {
        public virtual Guid Id { get; set; }

        public virtual string Url { get; set; }
        public virtual string Provider { get; set; }
        public virtual string ExteralId { get; set; }
        public virtual string AccountName { get; set; }

        public virtual Guid GroupId { get; set; }
        public virtual Guid CreatorId { get; set; }
    }
}
