using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable enable
namespace PierresTreats.Models
{
    public class Flavor
    {
        public int FlavorId { get; set; }

        [Required(ErrorMessage = "This field can't be left empty")]
        public string Name { get; set; } = "";

        // public int TreatId { get; set; }
        // public virtual Treat Treats { get; set; } = null!;
        public List<FlavorTreats> JoinEntities { get; } = new List<FlavorTreats>();
    }
#nullable disable
}

