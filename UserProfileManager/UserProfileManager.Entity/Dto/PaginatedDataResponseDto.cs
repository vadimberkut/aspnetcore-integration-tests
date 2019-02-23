using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Entity.Dto
{
    public class PaginatedDataResponseDto<T> where T : new()
    {
        /// <summary>
        /// Parameters that were passed with request. Can be changed by server if invalid ones were provided
        /// </summary>
        public PaginatedDataRequestDto RequestParams { get; set; }

        /// <summary>
        /// Additional data for request. E.g. some calculated fields
        /// </summary>
        public PaginatedDataResponseParamsDto ResponseParams { get; set; }

        public T Data { get; set; }
    }
}
