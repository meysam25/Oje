using Oje.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractUserVM
    {
        public long? id { get; set; }
        public int? insuranceContractId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nationalCode { get; set; }
        public string eCode { get; set; }
        public string birthDate { get; set; }
        public InsuranceContractUserFamilyRelation? familyRelation { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string bankAcount { get; set; }
        public string bankShaba { get; set; }
        public string mainPersonNationalCode { get; set; }
        public string mainPersonECode { get; set; }
        public IFormFile nationalcodeImage { get; set; }
        public string nationalcodeImage_address { get; set; }
        public IFormFile shenasnamePage1Image { get; set; }
        public string shenasnamePage1Image_address { get; set; }
        public IFormFile shenasnamePage2Image { get; set; }
        public string shenasnamePage2Image_address { get; set; }
        public IFormFile bimeImage { get; set; }
        public string bimeImage_address { get; set; }
        public bool? isActive { get; set; }
    }
}
