using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class CreateUpdateUserForUserVM
    {
        public CreateUpdateUserForUserVM()
        {
            cIds = new();
            roleIds = new();
        }

        public long? id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public bool? isActive { get; set; }
        public string tell { get; set; }
        public bool? isDelete { get; set; }
        public string nationalCode { get; set; }
        public bool? isMobileConfirm { get; set; }
        public bool? isEmailConfirm { get; set; }
        public string postalCode { get; set; }
        public string address { get; set; }
        public List<int> roleIds { get; set; }
        public IFormFile userPic { get; set; }
        public string userPic_address { get; set; }
        public long? agentCode { get; set; }
        public string companyTitle { get; set; }
        public string bankAccount { get; set; }
        public string bankShaba { get; set; }
        public string birthDate { get; set; }
        public string insuranceECode { get; set; }
        public List<int> cIds { get; set; }
        public int? provinceId { get; set; }
        public int? cityId { get; set; }
        public decimal? mapLat { get; set; }
        public decimal? mapLon { get; set; }
        public byte? mapZoom { get; set; }
    }
}
