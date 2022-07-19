using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserFilledRegisterFormMainGrid: GlobalGrid
    {
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string createDate { get; set; }
        public string formTitle { get; set; }
        public long? price { get; set; }
        public bool? isPayed { get; set; }
        public string refferUser { get; set; }
        public string traceCode { get; set; }
        public bool? isDone { get; set; }
    }
}
