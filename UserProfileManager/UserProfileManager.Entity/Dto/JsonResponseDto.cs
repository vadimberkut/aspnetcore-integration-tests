using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Entity.Dto
{
    public class JsonResponseDto
    {
        public object Data { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }

    }
}
