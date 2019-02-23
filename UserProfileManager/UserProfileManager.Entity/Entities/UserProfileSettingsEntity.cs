using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserProfileManager.Entity.Entities
{
    public class UserProfileSettingsEntity
    {
        public Guid Id { get; set; }

        public bool Enabled { get; set; }

        // One to One
        public Guid UserProfileId { get; set; }
        public UserProfileEntity UserProfile { get; set; }
    }
}
