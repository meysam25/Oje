using Oje.Infrastructure.Enums;

namespace Oje.Section.RegisterForm.Models.View
{
    public class registerGetUserInfoVM
    {
        public int? company { get; set; }
        public long? licenceNumber { get; set; }
        public PersonType? realOrLegaPerson { get; set; }
    }
}
