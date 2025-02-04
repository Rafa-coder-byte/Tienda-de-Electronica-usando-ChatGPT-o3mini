// Controllers/ProductsDetailsController.cs
using mi_tienda_de_electrónica.Data;
using mi_tienda_de_electrónica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace mi_tienda_de_electrónica.Controllers
{
    public class ProductsDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Reviews) // Incluye las reseñas del producto
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int productId, string Author, int Rating, string Comment)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return NotFound(); // Si el producto no existe, muestra un error
            }

            var review = new Review
            {
                ProductId = productId,
                Author = Author,
                Rating = Rating,
                Comment = Comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(); // Guarda la reseña en la BD

            return RedirectToAction("Details", new { id = productId });
        }

    }
}
