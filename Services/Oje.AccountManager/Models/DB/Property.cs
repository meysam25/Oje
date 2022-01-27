using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("Properties")]
    public class Property
    {
        public PropertyType Type { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Value { get; set; }
        public PropertyInputType InputType { get; set; }
        public int SiteSettingId { get; set; }
    }
}
