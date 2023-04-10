using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using VenegasIntl.PowershellKitchen.Models;
using VenegasIntl.PowershellKitchen.Repositories;

namespace VenegasIntl.PowershellKitchen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly PowershellKitchenBlogRepository _blogRepository;

		public HomeController(
            PowershellKitchenBlogRepository blogRepository,
            ILogger<HomeController> logger)
        {
            _blogRepository = blogRepository;
			_logger = logger;
        }

        public IActionResult Index()
        {
			return View(_blogRepository.ReadAllBlogEntries());
		}

		public IActionResult BlogPost(string id)
		{
			return View(_blogRepository.ReadBlogEntry(id));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}