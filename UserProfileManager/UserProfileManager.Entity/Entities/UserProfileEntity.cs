using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using UserProfileManager.Entity.ValidationAttributes;

namespace UserProfileManager.Entity.Entities
{
    public class UserProfileEntity : ITrackableEntity
    {
        public Guid Id { get; set; }

        [Required]
        [OnlyLetters]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(200)]
        public string SkypeLogin { get; set; }

        [MaxLength(200)]
        public string Signature { get; set; }

        public string ImageUrl { get; set; }


        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }

        // Role
        public Guid RoleId { get; set; }
        public UserProfileRoleEntity Role { get; set; }

        // Settings
        public UserProfileSettingsEntity Settings { get; set; }
    }
}
