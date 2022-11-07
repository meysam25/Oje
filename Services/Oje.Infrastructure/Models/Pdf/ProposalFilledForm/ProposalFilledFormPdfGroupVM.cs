using System.Collections.Generic;

namespace Oje.Infrastructure.Models.Pdf.ProposalFilledForm
{
    public class FilledFormPdfGroupVM
    {
        public string title { get; set; }
        public List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupItems { get; set; }
    }
}
