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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PierresTreats.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly PierresTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> userManager, PierresTreatsContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Treat> userTreats = _db.Treats.
                               Where(entry => entry.User.Id == currentUser.Id)
                               .ToList();
      return View(userTreats);  
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create (Treat treat)
    {
      if(!ModelState.IsValid)
      {
        ModelState.AddModelError("", "Please correct the errors and try again.");
        return View(treat);
      }
      else
      {
         string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        treat.User = currentUser;
        _db.Treats.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
                                  .Include(treat => treat.JoinEntities)
                                  .ThenInclude(join => join.Flavor)
                                  .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

   [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      if (!User.Identity.IsAuthenticated)
      {
        ErrorViewModel error = new ErrorViewModel();
        error.ErrorMessage = "You must be logged in to do that.";
        FlavorTreat joinEntry = _db.FlavorTreat.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
        int treatId = joinEntry.TreatId;
        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("error", error);
        model.Add("treatId", treatId);
        return View("Error", model);
      }
      else
      {
        FlavorTreat joinEntry = _db.FlavorTreat.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
        _db.FlavorTreat.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = joinEntry.TreatId });
      }
    }

    [Authorize]
    public ActionResult Edit (int id)
    {
      Treat model = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(model);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      Treat model = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(model);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddFlavors(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavors(Treat treat, int flavorId)
    {
      FlavorTreat joinEntity = _db.FlavorTreat.FirstOrDefault(join => (join.FlavorId == flavorId && join.TreatId == treat.TreatId));
      if (joinEntity == null && flavorId != 0)
      {
        _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = flavorId, TreatId = treat.TreatId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }




  }

      
}