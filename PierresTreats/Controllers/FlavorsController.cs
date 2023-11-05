using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using PierresTreats.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;


namespace PierresTreats.Controllers
{
	[Authorize] 
	public class FlavorsController : Controller
	{
		private readonly PierresTreatsContext _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public ItemsController(UserManager<ApplicationUser> userManager, ToDoListContext db)
    {
      _userManager = userManager;
      _db = db;
    }

		public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Item> userItems = _db.Items
                          .Where(entry => entry.User.Id == currentUser.Id)
                          .Include(item => item.Category)
                          .ToList();
      return View(userItems);
    }

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Flavor Flavor)
		{
			_db.Flavors.Add(Flavor);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

public ActionResult Show(int id)
{
    Flavor thisFlavor = _db.Flavors
                            .Include(Flavor => Flavor.JoinEntities)
														.ThenInclude(join => join.Treat)
                            .FirstOrDefault(Flavor => Flavor.FlavorId == id);
    return View(thisFlavor);
}

public ActionResult AddTreat(int id)
{
	Flavor thisFlavor = _db.Flavors.FirstOrDefault(Flavors => Flavors.FlavorId == id);
	ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
	return View(thisFlavor);
}

[HttpPost] 
public ActionResult AddTreat(Flavor Flavor, int TreatId)
{
    #nullable enable
    FlavorTreats? joinEntity = _db.FlavorTreats.FirstOrDefault(join => (join.TreatId == TreatId && join.FlavorId == Flavor.FlavorId));
    #nullable disable 
    if (joinEntity == null && TreatId != 0)
    {
        _db.FlavorTreats.Add(new FlavorTreats() { TreatId = TreatId, FlavorId = Flavor.FlavorId});
        _db.SaveChanges();
    }
    else
    {
        ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
        return View(Flavor);
    }
    return RedirectToAction("Show", new { id = Flavor.FlavorId });
}


public ActionResult Edit(int id)
{
	var thisFlavor = _db.Flavors.FirstOrDefault(Flavor => Flavor.FlavorId == id);
	return View(thisFlavor);
}

[HttpPost]
public ActionResult Edit(int id, string name)
{
	var thisFlavor = _db.Flavors.FirstOrDefault(Flavor => Flavor.FlavorId == id);
	thisFlavor.Name = name;
	_db.SaveChanges();
	return RedirectToAction("Index");
}

		public ActionResult Delete(int id)
		{
			Flavor thisFlavor = _db.Flavors.FirstOrDefault(Flavor => Flavor.FlavorId == id);
			return View(thisFlavor);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			Flavor thisFlavor = _db.Flavors.FirstOrDefault(Flavor => Flavor.FlavorId == id);
			_db.Flavors.Remove(thisFlavor);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult DeleteJoin(int joinId)
		{
			FlavorTreats joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatsId == joinId);
			_db.FlavorTreats.Remove(joinEntry);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}