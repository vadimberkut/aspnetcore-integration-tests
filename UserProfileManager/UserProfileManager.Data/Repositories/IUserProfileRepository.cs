using System;
using System.Collections.Generic;
using System.Text;
using UserProfileManager.Entity.Dto;
using UserProfileManager.Entity.Entities;

namespace UserProfileManager.Data.Repositories
{
    public interface IUserProfileRepository
    {
        PaginatedDataResponseDto<List<UserProfileEntity>> GetAll(UserProfilesRequestDto data);
        UserProfileEntity Get(Guid id);
        UserProfileEntity Create(UserProfileCreateOrUpdateDto data);
        UserProfileEntity Update(UserProfileCreateOrUpdateDto data);
        UserProfileRoleEntity UpdateRole(UpdateUserProfileRoleDto data);
        UserProfileSettingsEntity UpdateSettings(UserProfileSettingsEntity data);
        //UserProfileEntity ToggleUserEnabled();

        IEnumerable<UserProfileRoleEntity> GetAllRoles();
    }
}
