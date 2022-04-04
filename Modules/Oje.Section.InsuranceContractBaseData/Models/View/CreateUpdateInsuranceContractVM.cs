using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public long? code { get; set; }
        public List<int> insuranceContractTypeIds { get; set; }
        public int? insuranceContractCompanyId { get; set; }
        public int? proposalFormId { get; set; }
        public string proposalFormId_Title { get; set; }
        public long? monthlyPrice { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public IFormFile contractDocument { get; set; }
        public string contractDocument_address { get; set; }
        public bool? isActive { get; set; }
        public string description { get; set; }
        public int? rPFId { get; set; }
        public string rPFId_Title { get; set; }
    }
}
