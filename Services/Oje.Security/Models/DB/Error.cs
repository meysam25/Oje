using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("Errors")]
    public class Error
    {
        public Error()
        {
            ErrorParameters = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(50)]
        public string RequestId { get; set; }
        [Required]
        public string Message { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("Errors")]
        public User User { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public BMessages? BMessageCode { get; set; }
        public ApiResultErrorCode? Type { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [Required]
        public string LineNumber { get; set; }
        [Required]
        public string FileName { get; set; }
        public DateTime? LastTryDate { get; set; }
        public bool? IsSuccessEmail { get; set; }
        public int? CountTry { get; set; }
        public string LastEmailErrorMessage { get; set; }
        [MaxLength(200)]
        public string Url { get; set; }
        [MaxLength(200)]
        public string RefferUrl { get; set; }
        [MaxLength(50)]
        public string RequestType { get; set; }
        [MaxLength(200)]
        public string Browser { get; set; }
        


        [InverseProperty("Error")]
        public List<ErrorParameter> ErrorParameters { get; set; }
    }
}
