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
		public async Task<ActionResult> Index()
		{
			//Flavor logic//
				Flavor[] flavors = _db.Flavors.ToArray();
			   Dictionary<string,object[]> model = new Dictionary<string, object[]>();
        model.Add("Flavor", flavors);
				//Treat logic//
				string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
				if (currentUser != null)
				{
					Treat[] treats = _db.Treats
					.Where(entry => entry.User.Id == currentUser.Id)
					.ToArray();
				model.Add("Treat", treats);
				}
				return View(model);
		}
	}
}