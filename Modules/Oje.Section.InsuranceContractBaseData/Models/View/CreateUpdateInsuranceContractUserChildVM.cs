using Oje.Infrastructure.Enums;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractUserChildVM
    {
        public string mainPersonECode { get; set; }
        public InsuranceContractUserFamilyRelation? familyRelation { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fatherName { get; set; }
        public string birthDate { get; set; }
        public string shenasnameNo { get; set; }
        public string nationalCode { get; set; }
        public Custody? custody { get; set; }
        public MarrageStatus? marrageStatus { get; set; }
        public int? baseInsuranceId { get; set; }
        public string baseInsuranceCode { get; set; }
        public string hireExpiredDate { get; set; }

        public string bProvinceId { get; set; }
        public string mainPersonNationalCode { get; set; }
        public Gender? gender { get; set; }

    }
}
