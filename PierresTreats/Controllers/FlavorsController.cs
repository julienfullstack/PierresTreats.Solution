using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PierresTreats.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly PierresTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public FlavorsController(UserManager<ApplicationUser> userManager, PierresTreatsContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    // [AllowAnonymous]
    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Flavor> userItems = _db.Flavors
                          .Where(entry => entry.User.Id == currentUser.Id)
                          .Include(flavor => flavor.FlavorId)
                          .ToList();
      return View(userItems);
    }

    public ActionResult Create()
    {
      return View();
    }

     [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor, int CategoryId)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
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

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Flavor thisFlavor = _db.Flavors
                                    .Include(flavor => flavor.JoinEntities)
                                    .ThenInclude(join => join.Treat)
                                    .FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    public ActionResult Edit(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult Edit(Flavor flavor)
    {
      if(!ModelState.IsValid)
      {
        return View(flavor);
      }
      else
      {
        _db.Flavors.Update(flavor);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = flavor.FlavorId });
      }
    }

    public ActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTreat(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);

      List<Treat> treats = _db.Treats.ToList();
      ViewBag.IsNoTreats = (treats.Count == 0) ? true : false;
      ViewBag.TreatId = new SelectList(treats, "TreatId", "Type");

      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult AddTreat(Flavor flavor, int treatId)
    {
      #nullable enable
      FlavorTreats? joinEntity = _db.FlavorTreats.FirstOrDefault(join => (join.TreatId == treatId && join.FlavorId == flavor.FlavorId));
      #nullable disable
      if(joinEntity == null && treatId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreats() { FlavorId = flavor.FlavorId, TreatId = treatId });
        _db.SaveChanges();
      }

      return RedirectToAction("Details", new { id = flavor.FlavorId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      FlavorTreats joinEntity = _db.FlavorTreats.FirstOrDefault(join => join.FlavorTreatsId == joinId);
      _db.FlavorTreats.Remove(joinEntity);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntity.FlavorId });
    }
  }
}