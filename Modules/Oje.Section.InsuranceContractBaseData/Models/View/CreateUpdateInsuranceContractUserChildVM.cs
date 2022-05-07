using Oje.Infrastructure.Enums;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractUserChildVM
    {
        public int? insuranceContractId { get; set; }
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
        public string insuranceMiniBookNumber { get; set; }

    }
}
