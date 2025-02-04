using System.Security.Claims;
using System.Threading.Tasks;
using mi_tienda_de_electrónica.Data;
using mi_tienda_de_electrónica.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace mi_tienda_de_electrónica.Controllers
{
    public class AdminController : Controller
    {
        // Credenciales fijas para la demo
        private const string AdminUsername = "admin";
        private const string AdminPassword = "password";

        private readonly AppDbContext _db;
        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (username == AdminUsername && password == AdminPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("CookieAuth", principal);
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult Index()
        {
            var products = _db.Products.ToList();
            return View(products);
        }

        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var existing = _db.Products.Find(product.Id);
                if (existing == null)
                    return NotFound();

                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.ImageUrl = product.ImageUrl;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}

