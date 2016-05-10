namespace Identity.SQLite
{
    using SQLiteIdentityEF7.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
        }
    
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
