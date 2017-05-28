﻿using PhotoSharer.Business.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace PhotoSharer.Business.Entities
{
    public class AppGroup : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string InviteCode { get; set; }
        public virtual string Url { get; set; }
        public virtual Guid CreatorId { get; set; }

        private IList<AppUser> users;
        public virtual IList<AppUser> Users
        {
            get
            {
                return users ?? (users = new List<AppUser>());
            }
            set { users = value; }
        }
    }
}