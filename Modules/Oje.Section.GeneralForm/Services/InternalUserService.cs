using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Services.EContext;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class InternalUserService : IInternalUserService
    {
        readonly GeneralFormDBContext db = null;
        public InternalUserService
            (
                GeneralFormDBContext db
            )
        {
            this.db = db;
        }

        public void UpdateUserInfoIfNeeded(IFormCollection form, long userId, int? siteSettingId)
        {
            var foundUser = db.Users.Where(t => t.Id == userId && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundUser != null)
            {
                var mobile = form.GetStringIfExist("mobile");
                if (!string.IsNullOrEmpty(mobile) && foundUser.Username == mobile)
                {
                    var address = form.GetStringIfExist("address");
                    var email = form.GetStringIfExist("email");
                    var firstName = form.GetStringIfExist("realOrLegaPerson") == "1" ? form.GetStringIfExist("firstName") : form.GetStringIfExist("firstAgentName");
                    var lastName = form.GetStringIfExist("realOrLegaPerson") == "1" ? form.GetStringIfExist("lastName") : form.GetStringIfExist("lastAgentName");
                    var nationalCode = form.GetStringIfExist("realOrLegaPerson") == "1" ? form.GetStringIfExist("nationalCode") : null;
                    var postalCode = form.GetStringIfExist("postalCode");
                    var tell = form.GetStringIfExist("tell");

                    if (string.IsNullOrEmpty(foundUser.Mobile))
                        foundUser.Mobile = mobile;
                    if (!string.IsNullOrEmpty(address) && string.IsNullOrEmpty(foundUser.Address) && address.Length <= 1000)
                        foundUser.Address = address;
                    if (!string.IsNullOrEmpty(firstName) && (string.IsNullOrEmpty(foundUser.Firstname) || foundUser.Firstname == " ") && firstName.Length <= 50)
                        foundUser.Firstname = firstName;
                    if (!string.IsNullOrEmpty(lastName) && (string.IsNullOrEmpty(foundUser.Lastname) || foundUser.Lastname == " ") && lastName.Length <= 50)
                        foundUser.Lastname = lastName;
                    if (!string.IsNullOrEmpty(postalCode) && string.IsNullOrEmpty(foundUser.PostalCode) && postalCode.Length == 10)
                        foundUser.PostalCode = postalCode;
                    if (!string.IsNullOrEmpty(tell) && string.IsNullOrEmpty(foundUser.Tell) && tell.Length < 50)
                        foundUser.Tell = tell;



                    db.SaveChanges();
                }
            }
        }
    }
}
