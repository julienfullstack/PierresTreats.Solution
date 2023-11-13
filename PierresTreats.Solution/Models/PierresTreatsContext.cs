using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace PierresTreatsSolution.Modelss
{
  public class PierresTreatsContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Flavor> Flavors { get; set; }
    public DbSet<Treat> Treats { get; set; }
    public DbSet<FlavorTreats> FlavorTreats { get; set; }

    public PierresTreatsContext(DbContextOptions options) : base(options) { }

  }
}
