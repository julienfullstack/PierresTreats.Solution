using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PierresTreats.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace PierresTreats.Controllers
{
  public class FlavorsController : Controller
  {
    private readonly PierresTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public FlavorsController(UserManager<ApplicationUser> userManager, PierresTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Flavor> userFlavors = _db.Flavors
        .Where(entry => entry.User.Id == currentUser.Id)
        .ToList();
      return View(userFlavors);
    }

    [Authorize]
    public ActionResult Create()
    {
      return View();
    } 

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor)
    {
      if (!ModelState.IsValid)
      {
        ModelState.AddModelError("", "Please correct the errors and try again.");
        return View(flavor);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        flavor.User = currentUser;
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

    }
      

    public ActionResult Details(int id)
    {
      Flavor thisFlavor = _db.Flavors
        .Include(flavor => flavor.JoinEntities)
        .ThenInclude(join => join.Treat)
        .FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
      return View(thisFlavor);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Edit(Flavor flavor)
    {
      _db.Flavors.Update(flavor);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = flavor.FlavorId });
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
      return View(thisFlavor);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

  }
}