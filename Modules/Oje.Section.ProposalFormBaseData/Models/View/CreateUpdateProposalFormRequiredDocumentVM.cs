using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class CreateUpdateProposalFormRequiredDocumentVM
    {
        public int? id { get; set; }
        public int? typeId { get; set; }
        public string typeId_Title { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public bool? isRequired { get; set; }
        public IFormFile downloadFile { get; set; }
        public string downloadFile_address { get; set; }
    }
}
