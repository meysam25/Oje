using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User() 
        {
            UploadedFiles = new ();    
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Lastname { get; set; }

        [InverseProperty("CreateByUser")]
        public List<UploadedFile> UploadedFiles { get; set; }
    }
}
