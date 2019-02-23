using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeTypes.Core;
using UserProfileManager.Data.Repositories.Base;
using UserProfileManager.Entity.Dto;
using UserProfileManager.Entity.Entities;

namespace UserProfileManager.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class UserProfilesController : BaseApiController
    {
        private readonly IHostingEnvironment HostingEnvironment = null;
        private readonly IUnitOfWork UnitOfWork = null;

        public UserProfilesController(
            IHostingEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork
        )
        {
            this.HostingEnvironment = hostingEnvironment;
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<string> Test()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery]UserProfilesRequestDto data)
        {
            var result = this.UnitOfWork.UserProfileRepository.GetAll(data);
            return this.JsonResponse(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody]UserProfileCreateOrUpdateDto data)
        {
            if(!String.IsNullOrEmpty(data.ImageBase64))
            {
                data.ImageUrl = this.SaveImageFromBase64(data.ImageBase64);
            }
            var result = this.UnitOfWork.UserProfileRepository.Create(data);
            this.UnitOfWork.Commit();
            return this.JsonResponse(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody]UserProfileCreateOrUpdateDto data)
        {
            string oldImagePath = null;

            // Update file if present
            if (!String.IsNullOrEmpty(data.ImageBase64))
            {
                var userProfile = this.UnitOfWork.UserProfileRepository.Get(data.Id);

                // Delete old image when data will be saved to DB
                if(!String.IsNullOrEmpty(userProfile.ImageUrl))
                {
                    oldImagePath = userProfile.ImageUrl;
                }

                data.ImageUrl = this.SaveImageFromBase64(data.ImageBase64);
            }

            var result = this.UnitOfWork.UserProfileRepository.Update(data);
            this.UnitOfWork.Commit();

            // Delete old file
            if(oldImagePath != null)
            {
                this.DeleteUploadedFile(oldImagePath);
            }
            return this.JsonResponse(result);
        }

        [HttpPut]
        public IActionResult UpdateRole([FromBody]UpdateUserProfileRoleDto data)
        {
            var result = this.UnitOfWork.UserProfileRepository.UpdateRole(data);
            this.UnitOfWork.Commit();
            return this.JsonResponse(result);
        }

        [HttpPut]
        public IActionResult UpdateSettings([FromBody]UserProfileSettingsEntity data)
        {
            var result = this.UnitOfWork.UserProfileRepository.UpdateSettings(data);
            this.UnitOfWork.Commit();
            return this.JsonResponse(result);
        }

        private string SaveImageFromBase64(string base64String)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var rootPath = this.HostingEnvironment.WebRootPath;
            var uploadsDir = "uploads";
            var uploadsPath = Path.Combine(rootPath, uploadsDir);
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var header = Regex.Match(base64String, "^data:image/[a-zA-Z]+;base64,").Value;
            var content = Regex.Replace(base64String, "^data:image/[a-zA-Z]+;base64,", String.Empty);
            var mimeType = this.Base64MimeType(header);
            var extension = MimeTypeMap.GetExtension(mimeType);

            var fileName = $"{unixTimestamp}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            var bytes = Convert.FromBase64String(content);

            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            var relativePath = Path.Combine(uploadsDir, fileName);
            relativePath = relativePath.Replace(Path.DirectorySeparatorChar, '/'); // use Unix style path
            return relativePath;
        }

        private string Base64MimeType(string encoded)
        {
            string result = null;

            if (String.IsNullOrEmpty(encoded))
            {
                return result;
            }

            Regex regex = new Regex("^data:([a-zA-Z0-9]+/[a-zA-Z0-9-.+]+).*,*");
            var match = regex.Match(encoded);
            if (match.Success)
            {
                result = match.Groups[1].Value;
            }
            return result;
        }

        private void DeleteUploadedFile(string fileRelativePath)
        {
            var rootPath = this.HostingEnvironment.WebRootPath;
            var filePath = Path.Combine(rootPath, fileRelativePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private string ConvertPathToAbsolute(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }
            string separator = path.StartsWith('/') ? "" : "/";
            string result = $"{this.Request.Scheme}://{this.Request.Host}{separator}{path}";
            return result;
        }
    }
}
