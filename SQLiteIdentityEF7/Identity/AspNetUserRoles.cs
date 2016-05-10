using Identity.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLiteIdentityEF7.Identity
{
    public class AspNetUserRoles
    {
        public AspNetUsers User { get; set; }

        public string UserId { get; set; }

        public AspNetRoles Role {get;set;}

        public string RoleId { get; set; }
    }
}