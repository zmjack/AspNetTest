using AspNetTest.Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetTest.Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly INowService _nowService;

        public HomeController(INowService nowService)
        {
            _nowService = nowService;
        }

        public IActionResult Index()
        {
            return Json(new { User = User.Identity.Name, Now = _nowService.Now });
        }
    }
}
