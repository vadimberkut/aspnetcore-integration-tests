using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Entity.Dto
{
    public class UpdateUserProfileRoleDto
    {
        public Guid UserProfileId { get; set; }
        public Guid UserProfileRoleId { get; set; }
    }
}
