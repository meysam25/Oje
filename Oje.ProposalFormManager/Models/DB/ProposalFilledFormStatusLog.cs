using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFilledFormStatusLogs")]
    public class ProposalFilledFormStatusLog
    {
        [Key]
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId")]
        [InverseProperty("ProposalFilledFormStatusLogs")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public ProposalFilledFormStatus Type { get; set; }
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ProposalFilledFormStatusLogs")]
        public User User { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
    }
}
