﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB.Reports
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserRoles")]
        public User User { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId"), InverseProperty("UserRoles")]
        public Role Role { get; set; }
    }
}
