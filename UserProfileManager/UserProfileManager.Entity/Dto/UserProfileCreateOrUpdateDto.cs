using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserProfileManager.Entity.ValidationAttributes;

namespace UserProfileManager.Entity.Dto
{
    public class UserProfileCreateOrUpdateDto
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

        public string ImageBase64 { get; set; }
    }
}
