using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractCompanyVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string shomareSabt { get; set; }
        public string codeEghtesadi { get; set; }
        public string shenaseMeli { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string rabeteSazmaniName { get; set; }
        public string modirAmelName { get; set; }
        public bool? isActive { get; set; }
    }
}
