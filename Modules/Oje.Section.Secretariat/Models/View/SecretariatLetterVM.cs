using System;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterVM
    {
        public string header { get; set; }
        public string footer { get; set; }
        public string headerDescription { get; set; }
        public string footerDescription { get; set; }
        public bool hasLink { get; set; }
        public DateTime createDate { get; set; }
        public string number { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string subject { get; set; }
        public string sUserTitle { get; set; }
        public string sUserRole { get; set; }
        public string sUserSignature { get; set; }
        public string sUserFirstname { get; set; }
        public string sUserLastname { get; set; }
        public string description { get; set; }
    }
}
