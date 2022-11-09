using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using System.Collections.Generic;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFilledFormPdfDetailesVM
    {
        public long generalFilledFormId { get; set; }
        public string traceCode { get; set; }
        public string ppfTitle { get; set; }
        public string id { get; set; }
        public long? price { get; set; }
        public string ppfCreateDate { get; set; }

        public List<FilledFormPdfGroupVM> generalFilledFormPdfGroupVMs { get; set; }
        public List<IdTitle> nextStatuses { get;  set; }
    }
}
