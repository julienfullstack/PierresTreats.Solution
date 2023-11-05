using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PierresTreats.Controllers
{
	public class HomeController : Controller
	{
		private readonly PierresTreatsContext _db;

		public HomeController(PierresTreatsContext db)
		{
			_db = db;
		}

		[HttpGet("/")]
		public ActionResult Index()
		{
			Flavor[] Flavors = _db.Flavors.ToArray();
			Treat[] Treats = _db.Treats.ToArray();
			Dictionary<string,object[]> model = new Dictionary<string, object[]>();
			model.Add("Flavors", Flavors);
			model.Add("Treats", Treats);
			return View(model);
		}
	}
}