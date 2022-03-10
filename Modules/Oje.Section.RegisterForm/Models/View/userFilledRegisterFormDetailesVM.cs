using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<userFilledRegisterFormDetailesGroupVM> groups { get; set; }
    }
}
