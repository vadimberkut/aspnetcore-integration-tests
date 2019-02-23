using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Entity.Dto
{
    public class UserProfilesRequestDto : PaginatedDataRequestDto
    {
        public bool? Enabled { get; set; }
    }
}
