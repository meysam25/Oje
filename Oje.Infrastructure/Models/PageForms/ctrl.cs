using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models.PageForms
{
    public class ctrl
    {
        public ctrl()
        {
            showHideCondation = new List<ctrlShowHideCondation>();
            validations = new List<validation>();
        }

        public string id { get; set; }
        public string @class { get; set; }
        public string parentCL { get; set; }
        public ctrlType? type { get; set; }
        public string textfield { get; set; }
        public string valuefield { get; set; }
        public string dataurl { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public List<ctrlShowHideCondation> showHideCondation { get; set; }
        public bool? isRequired { get; set; }
        public string acceptEx { get; set; }
        public ctrlSchema schema { get; set; }
        public bool? nationalCodeValidation { get; set; }
        public bool? disabled { get; set; }
        public List<validation> validations { get; set; }

        [JsonIgnore]
        public string defV { get; set; }

    }
}
