using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserProfileManager.Data.DbContexts;
using UserProfileManager.Entity.Dto;
using UserProfileManager.Entity.Entities;

namespace UserProfileManager.Data.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext DbContext = null;

        public UserProfileRepository(
               ApplicationDbContext dbContext
           )
        {
            this.DbContext = dbContext;
        }

        public PaginatedDataResponseDto<List<UserProfileEntity>> GetAll(UserProfilesRequestDto data)
        {
            var query = this.DbContext.UserProfiles
                //.Include(x => x.Role)
                .Include(x => x.Settings)
                .AsQueryable();

            if(data.Enabled != null)
            {
                query = query.Where(x => x.Settings.Enabled == data.Enabled);
            }
            if (!String.IsNullOrEmpty(data.SearchText))
            {
                string search = data.SearchText.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(search) || x.Email.ToLower().Contains(search));
            }
            int totalCount = query.Count();
            query = query
                .OrderByDescending(x => x.CreatedOnUtc)
                .Skip(data.PageSize.Value * data.Page.Value)
                .Take(data.PageSize.Value);
            var items = query.ToList();

            var result = new PaginatedDataResponseDto<List<UserProfileEntity>>()
            {
                RequestParams = data,
                ResponseParams = new PaginatedDataResponseParamsDto
                {
                    TotalCount = totalCount,
                    Pages = (int)Math.Ceiling((double)totalCount / (double)data.PageSize.Value)
                },
                Data = items
            };
            return result;
        }

        public UserProfileEntity Get(Guid id)
        {
            var result = this.DbContext.UserProfiles
                .Include(x => x.Role)
                .Include(x => x.Settings)
                .Single(x => x.Id == id);
            return result;
        }

        public UserProfileEntity Create(UserProfileCreateOrUpdateDto data)
        {
            var entity = new UserProfileEntity
            {
                Name = data.Name,
                Email = data.Email,
                SkypeLogin = data.SkypeLogin,
                Signature = data.Signature,
                ImageUrl = data.ImageUrl
            };
            // Set role to User
            entity.Role = this.GetDefaultRole();

            // Create settings
            entity.Settings = new UserProfileSettingsEntity
            {
                Enabled = true
            };

            var result = this.DbContext.UserProfiles
               .Add(entity);
            return result.Entity;
        }

        public UserProfileEntity Update(UserProfileCreateOrUpdateDto data)
        {
            var entity = this.Get(data.Id);

            entity.Name = data.Name;
            entity.Email = data.Email;
            entity.SkypeLogin = data.SkypeLogin;
            entity.Signature = data.Signature;
            entity.ImageUrl = data.ImageUrl;

            var result = this.DbContext.UserProfiles
                  .Update(entity);
            return result.Entity;
        }

        public UserProfileRoleEntity UpdateRole(UpdateUserProfileRoleDto data)
        {
            var entity = this.Get(data.UserProfileId);
            var role = this.DbContext.UserProfileRoles.Single(x => x.Id == data.UserProfileRoleId);
            entity.Role = role;
            return role;
        }

        public UserProfileSettingsEntity UpdateSettings(UserProfileSettingsEntity data)
        {
            var entity = this.Get(data.UserProfileId);
            entity.Settings.Enabled = data.Enabled;
            return entity.Settings;
        }

        #region Roles

        public IEnumerable<UserProfileRoleEntity> GetAllRoles()
        {
            var result = this.DbContext.UserProfileRoles.ToList();
            return result;
        }

        #endregion

        private UserProfileRoleEntity GetDefaultRole()
        {
            var result = this.DbContext.UserProfileRoles.First(x => x.Type == Entity.Enums.RoleType.User);
            return result;
        }
    }
}
