using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractMainGrid: GlobalGrid
    {
        public int? contractCompany { get; set; }
        public int? contractType { get; set; }
        public string ppfTitle { get; set; }
        public string title { get; set; }
        public string createUser { get; set; }
        public string createDate { get; set; }
        public long? code { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public bool? isActive { get; set; }
    }
}
