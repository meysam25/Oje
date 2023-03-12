using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("Actions")]
    public class Action: SignatureEntity
    {
        public Action()
        {
            RoleActions = new ();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Icon { get; set; }
        [Required]
        public int ControllerId { get; set; }
        public bool IsMainMenuItem { get; set; }

        [InverseProperty("Action")]
        public List<RoleAction> RoleActions { get; set; }
    }
}
