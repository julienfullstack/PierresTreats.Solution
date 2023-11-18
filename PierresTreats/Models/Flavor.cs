using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;


namespace PierresTreats.Models
{
    public class Flavor
    {
        public int FlavorId { get; set; }

        [Required(ErrorMessage = "This field can't be left empty")]
        public string Name { get; set; } = "";
        public List<FlavorTreat> JoinEntities { get; set; }
        public ApplicationUser User { get; set; } 
    }
}