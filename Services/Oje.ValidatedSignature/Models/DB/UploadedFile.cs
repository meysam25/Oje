﻿using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oje.Infrastructure.Services;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("UploadedFiles")]
    public class UploadedFile : SignatureEntity
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
        [MaxLength(100)]
        public string Title { get; set; }
        public long? UserId { get; set; }
        public long? FileSize { get; set; }
        [MaxLength(100)]
        public string FileContentType { get; set; }
        public int? SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("UploadedFiles")]
        public SiteSetting SiteSetting { get; set; }

        [NotMapped]
        public string FileNameOnServer
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName))
                    return GlobalConfig.WebHostEnvironment.ContentRootPath + FileName.Replace("/", "\\");

                return null;
            }
        }


    }
}
