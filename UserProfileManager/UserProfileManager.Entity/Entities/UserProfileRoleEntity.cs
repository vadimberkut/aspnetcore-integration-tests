using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserProfileManager.Entity.Enums;

namespace UserProfileManager.Entity.Entities
{
    public class UserProfileRoleEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public RoleType Type { get; set; }

        // One to Many
        public ICollection<UserProfileEntity> UserProfiles { get; set; }
    }
}
