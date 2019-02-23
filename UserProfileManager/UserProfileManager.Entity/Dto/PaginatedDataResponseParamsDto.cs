using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Entity.Dto
{
    public class PaginatedDataResponseParamsDto
    {
        public int TotalCount { get; set; }
        public int Pages { get; set; }
    }
}
