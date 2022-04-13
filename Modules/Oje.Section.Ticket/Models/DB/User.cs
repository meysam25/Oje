using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Ticket.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            CreateUserTicketUsers = new();
            UpdateUserTicketUsers = new();
            CreateUserTicketUserAnswers = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(50)]
        public string Username { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Lastname { get; set; }

        [InverseProperty("CreateUser")]
        public List<TicketUser> CreateUserTicketUsers { get; set; }
        [InverseProperty("UpdateUser")]
        public List<TicketUser> UpdateUserTicketUsers { get; set; }
        [InverseProperty("CreateUser")]
        public List<TicketUserAnswer> CreateUserTicketUserAnswers { get; set; }
    }
}
