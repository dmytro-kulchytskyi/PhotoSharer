using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharer.Business.Entities
{
    public class GroupMember
    {
        public virtual Guid UserId { get; set; }
        public virtual Guid GroupId { get; set; }

        public override bool Equals(object obj)
        {
            if (this == null && obj == null)
            {
                return true;
            }

            if (this == null || obj == null)
            {
                return false;
            }

            var groupMember = obj as GroupMember;
            if (groupMember == null)
            {
                return false;
            }

            var result = groupMember.UserId == UserId && groupMember.GroupId == GroupId;

            return result;
        }

        public override int GetHashCode()
        {
            var hashCode = GetType().GetHashCode();
            unchecked
            {
                hashCode = (hashCode * 397) ^ UserId.GetHashCode();
                hashCode = (hashCode * 397) ^ GroupId.GetHashCode();
            }

            return hashCode;
        }
    }
}
