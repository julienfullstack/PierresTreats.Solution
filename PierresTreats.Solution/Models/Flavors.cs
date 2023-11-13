using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

// #nullable enable
namespace PierresTreatsSolution.Modelss
{
    public class Flavor
    {
        public int FlavorId { get; set; }

        [Required(ErrorMessage = "This field can't be left empty")]
        public string Name { get; set; } = "";
        public List<FlavorTreats> JoinEntities { get; } = new List<FlavorTreats>();
        public ApplicationUser User { get; set; } 
    }
// #nullable disable
}