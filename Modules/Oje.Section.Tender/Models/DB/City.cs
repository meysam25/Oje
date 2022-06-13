using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("Cities")]
    public class City
    {
        public City()
        {
            TenderFilledForms = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("City")]
        public List<TenderFilledForm> TenderFilledForms { get; set; }
    }
}
