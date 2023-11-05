using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PierresTreats.Controllers
{
	public class HomeController : Controller
	{
		private readonly PierresTreatsContext _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(UserManager<ApplicationUser> userManager, PierresTreatsContext db)
		{
			_userManager = userManager;
			_db = db;
		}

		[HttpGet("/")]
		public async ActionResult Index()
		{
			Flavor[] Flavors = _db.Flavors.ToArray();
			   Dictionary<string,object[]> model = new Dictionary<string, object[]>();
        model.Add("Flavors", Flavors);
				string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
				if (currentUser != null)
				{
					Treat[] Treats = _db.Treats
					.Where(entry => entry.User.Id == currentUser.Id)
					.ToArray();
				model.Add("Treats", Treats);
				}
				return View(model);
		}
	}
}