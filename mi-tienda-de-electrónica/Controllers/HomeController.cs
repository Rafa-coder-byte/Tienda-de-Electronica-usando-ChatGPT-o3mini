using Microsoft.AspNetCore.Mvc;

namespace mi_tienda_de_electrónica.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

