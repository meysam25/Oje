using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oje.Infrastructure.Interfac;

namespace Oje.FileService.Models.DB
{
    [Table("UploadedFiles")]
    public class UploadedFile: EntityWithCreateByUser<User, long>
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public long? CreateByUserId { get; set; }
        [ForeignKey("CreateByUserId"), InverseProperty("UploadedFiles")]
        public User CreateByUser { get; set; }
        public bool? IsFileAccessRequired { get; set; }
        public long? ObjectId { get; set; }
        [MaxLength(50)]
        public string ObjectIdStr { get; set; }
        public int? SiteSettingId { get; set; }

        [NotMapped]
        public string FileNameOnServer
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                    return GlobalConfig.WebHostEnvironment.WebRootPath + FileName.Replace("/", "\\");

                return null;
            }
        }
        [NotMapped]
        public string FileContentType
        {
            get
            {
                string result = "";
                if (!string.IsNullOrEmpty(FileName))
                    new FileExtensionContentTypeProvider().TryGetContentType(new FileInfo(FileName).Name, out result);


                return result;
            }
        }

    }
}
