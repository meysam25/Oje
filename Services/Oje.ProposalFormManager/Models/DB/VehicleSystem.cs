﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("VehicleSystems")]
    public class VehicleSystem
    {
        public VehicleSystem()
        {
            VehicleSystemVehicleTypes = new();
            VehicleSpecs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("VehicleSystem")]
        public List<VehicleSystemVehicleType> VehicleSystemVehicleTypes { get; set; }
        [InverseProperty("VehicleSystem")]
        public List<VehicleSpec> VehicleSpecs { get; set; }

    }
}
