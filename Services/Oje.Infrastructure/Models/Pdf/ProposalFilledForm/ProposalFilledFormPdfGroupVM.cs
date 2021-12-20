using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models.Pdf.ProposalFilledForm
{
    public class ProposalFilledFormPdfGroupVM
    {
        public string title { get; set; }
        public List<ProposalFilledFormPdfGroupItem> ProposalFilledFormPdfGroupItems { get; set; }
    }
}
