using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

// #nullable enable
namespace PierresTreats.Models
{
    public class Treat
    {
        public int TreatId { get; set; }

        [Required(ErrorMessage = "This field can't be left empty")]
        public string Name { get; set; } = "";

        public List<FlavorTreat> JoinEntities { get; set; } 
        public ApplicationUser User { get; set; }
    }
// #nullable disable
}