using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("Provinces")]
    public class Province
    {
        public Province()
        {
            TenderFilledForms = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }

        [InverseProperty("Province")]
        public List<TenderFilledForm> TenderFilledForms { get; set; }
    }
}
