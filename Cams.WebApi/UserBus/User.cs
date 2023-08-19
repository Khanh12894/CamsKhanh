using WorkSimple.Core.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSimple.Auth.UserBus
{
    public class CreateUserEvent : EventData
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? UserType { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid SsoIdentity { get; set; }

    }
    public class UpdateUserEvent : EventData
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
