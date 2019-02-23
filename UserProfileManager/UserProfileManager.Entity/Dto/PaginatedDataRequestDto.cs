using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Entity.Dto
{
    public class PaginatedDataRequestDto
    {
        public PaginatedDataRequestDto()
        {

        }

        private int Page_ = 0;
        public int? Page {
            get {
                return this.Page_;
            }
            set
            {
                this.Page_ = value ?? 0;
            }
        }

        private int PageSize_ = 10;
        public int? PageSize
        {
            get
            {
                return this.PageSize_;
            }
            set
            {
                this.PageSize_ = value ?? 10;
            }
        }

        public string SearchText { get; set; }
    }
}
