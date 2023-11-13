using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PierresTreatsSolution.Modelss
{
  public class FlavorTreats
  {
    public int FlavorTreatsId { get; set; }
    public int TreatId { get; set; }
    public Treat Treat { get; set; }
    public int FlavorId { get; set; }
    public Flavor Flavor { get; set; }
  }
}