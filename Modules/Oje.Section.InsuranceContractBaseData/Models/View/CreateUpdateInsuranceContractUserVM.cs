using Oje.Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractUserVM : GlobalSiteSetting
    {
        public long? id { get; set; }
        public string eCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fatherName { get; set; }
        public string birthDate { get; set; }
        public string shenasnameNo { get; set; }
        public string nationalCode { get; set; }
        public string hireDate { get; set; }
        public Gender? gender { get; set; }
        public MarrageStatus? marrageStatus { get; set; }
        public int? subCatId { get; set; }
        public int? baseInsuranceId { get; set; }
        public string baseInsuranceCode { get; set; }
        public int? bankId { get; set; }
        public string accountCardNo { get; set; }
        public string tell { get; set; }
        public string mobile { get; set; }
        public string bankShaba { get; set; }
        public string hireExpiredDate { get; set; }
        public int? insuranceContractId { get; set; }



        public string bProvinceId { get; set; }
        public string provinceId { get; set; }
        public int? cityId { get; set; }
        public InsuranceContractUserFamilyRelation? familyRelation { get; set; }
        public Custody? custody { get; set; }
        public string email { get; set; }
        public string mainPersonECode { get; set; }
        public string mainPersonNationalCode { get; set; }
        public bool? isActive { get; set; }
        public string password { get; set; }

        public IFormFile nationalcodeImage { get; set; }
        public string nationalcodeImage_address { get; set; }
        public IFormFile shenasnamePage1Image { get; set; }
        public string shenasnamePage1Image_address { get; set; }
        public IFormFile shenasnamePage2Image { get; set; }
        public string shenasnamePage2Image_address { get; set; }
        public IFormFile bimeImage { get; set; }
        public string bimeImage_address { get; set; }

    }
}
