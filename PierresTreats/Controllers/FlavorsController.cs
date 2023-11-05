using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using PierresTreats.Models;


namespace PierresTreats.Controllers
{
	public class FlavorsController : Controller
	{
		private readonly PierresTreatsContext _db;

		public FlavorsController(PierresTreatsContext db)
		{
			_db = db;
		}

		public ActionResult Index()
		{
			List<Flavor> model = _db.Flavors.ToList();
			return View(model);
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