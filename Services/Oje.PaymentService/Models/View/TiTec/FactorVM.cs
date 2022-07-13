
namespace Oje.PaymentService.Models.View.TiTec
{
    public class FactorVM
    {
        public FactorVM()
        {
            Shares = new();
        }

        public string FactorNumber { get; set; }
        public string ContractNumber { get; set; }
        public long Amount { get; set; }
        public long AmountPayable { get; set; }
        public string CallbackUrl { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerNationalCode { get; set; }
        public string CustomerPostalCode { get; set; }
        public bool SendFactorToCustomer { get; set; }
        public string Description { get; set; }
        //Persian
        public string FactorDate { get; set; }
        public long WalletCharge { get; set; }

        public List<FactorShare> Shares { get; set; }
    }
}
