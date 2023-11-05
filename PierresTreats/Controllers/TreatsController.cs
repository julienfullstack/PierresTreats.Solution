using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PierresTreats.Controllers
{
  public class TreatsController : Controller
  {
    private readonly PierresTreatsContext _db;

    public TreatsController(PierresTreatsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Treat> model = _db.Treats.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

   [HttpPost]
public ActionResult Create(Treat Treat, int FlavorId)
{
    if (ModelState.IsValid)
    {
        _db.Treats.Add(Treat);
        _db.SaveChanges();

        if (FlavorId > 0)
        {
        }

        return RedirectToAction("Index");
    }
    else
    {
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
        return View(Treat);
    }
}


    public ActionResult Show(int id)
    {
      Treat thisTreat = _db.Treats
                              .Include(Treat => Treat.JoinEntities)
                              .ThenInclude(join => join.Flavor)
                              .FirstOrDefault(Treat => Treat.TreatId == id);
      return View(thisTreat);
    }

    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(Treats => Treats.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat Treat, int FlavorId)
    {
      #nullable enable
      FlavorTreats? joinEntity = _db.FlavorTreats.FirstOrDefault(join => (join.TreatId == Treat.TreatId && join.FlavorId == FlavorId));
      #nullable disable
      if (joinEntity == null && FlavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreats() { FlavorId = FlavorId, TreatId = Treat.TreatId });
        _db.SaveChanges();
      }
      else
      {
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
        return View(Treat);
      }
      return RedirectToAction("Show", new { id = Treat.TreatId });
    }

public ActionResult Edit(int id)
{
	var thisTreat = _db. Treats.FirstOrDefault(Treat => Treat.TreatId == id);
	return View(thisTreat);
}

[HttpPost]
public ActionResult Edit(int id, string name)
{
	var thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
	thisTreat.Name = name;
	_db.SaveChanges();
	return RedirectToAction("Index");
}

    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
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
