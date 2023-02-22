using System.Collections.Generic;

namespace Oje.Section.RegisterForm.Models.View
{
    public class userFilledRegisterFormDetailesVM
    {

        public string id { get; set; }
        public string ppfTitle { get; set; }
        public string ppfCreateDate { get; set; }
        public long proposalFilledFormId { get; set; }
        public string createUserFullname { get; set; }
        public string traceCode { get; set; }
        public long? price { get; set; }
        public string companyTitle { get; set; }
        public string companyImage { get; set; }
        public long? paymentUserId { get; set; }
        public int userRegisterFormId { get; set; }
        public string headerTemplate { get; set; }
        public string footerTemplate { get; set; }
        public bool isPayed { get; set; }
        public List<string> uploadFiles { get; set; }


        public List<userFilledRegisterFormDetailesGroupVM> groups { get; set; }
    }
}
