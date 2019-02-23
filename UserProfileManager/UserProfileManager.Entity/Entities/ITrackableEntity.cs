using System;
using System.Collections.Generic;
using System.Text;

namespace UserProfileManager.Entity.Entities
{
    public interface ITrackableEntity
    {
        DateTime CreatedOnUtc { get; set; }
        DateTime UpdatedOnUtc { get; set; }
    }
}
