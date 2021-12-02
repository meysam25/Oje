using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Models.DB
{
    [Table("UploadedFiles")]
    public class UploadedFile
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public long? CreateByUserId { get; set; }
        [ForeignKey("CreateByUserId")]
        [InverseProperty("UploadedFiles")]
        public User CreateByUser { get; set; }
        public bool? IsFileAccessRequired { get; set; }
        public long? ObjectId { get; set; }
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
