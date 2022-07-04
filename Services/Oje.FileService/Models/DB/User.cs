using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.FileService.Models.DB
{
    [Table("Users")]
    public class User : EntityWithParent<User>
    {
        public User()
        {
            Childs = new();
            UploadedFiles = new();
            SecoundUploadedFiles = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Lastname { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("Childs")]
        public User Parent { get; set; }

        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }
        [InverseProperty("CreateByUser")]
        public List<UploadedFile> UploadedFiles { get; set; }
        [InverseProperty("User")]
        public List<UploadedFile> SecoundUploadedFiles { get; set; }
    }
}
