using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractUserMainGrid: GlobalGrid
    {
        public int? contract { get; set; }
        public string fistname { get; set; }
        public string lastName { get; set; }
        public string nationalcode { get; set; }
        public string mainPersonNationalcode { get; set; }
        public string birthDate { get; set; }
        public InsuranceContractUserFamilyRelation? familyRelation { get; set; }
        public string createUser { get; set; }
        public string eCode { get; set; }
        public string mainECode { get; set; }
        public bool? isActive { get; set; }
}
}
