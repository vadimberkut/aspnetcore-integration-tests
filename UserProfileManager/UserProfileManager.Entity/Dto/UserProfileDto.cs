using System;
using System.Collections.Generic;
using System.Text;
using UserProfileManager.Entity.Entities;

namespace UserProfileManager.Entity.Dto
{
    public class UserProfileDto : UserProfileEntity
    {
        public string ImageUrlAbsolute { get; set; }
    }
}
