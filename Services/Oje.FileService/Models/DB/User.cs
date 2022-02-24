using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FileService.Models.DB
{
    [Table("Users")]
    public class User : EntityWithParent<User>
    {
        public User()
        {
            Childs = new();
            UploadedFiles = new();
        }

        [Key]
        public long Id { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("Childs")]
        public User Parent { get; set; }

        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }
        [InverseProperty("CreateByUser")]
        public List<UploadedFile> UploadedFiles { get; set; }
    }
}
