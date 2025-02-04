using mi_tienda_de_electrónica.Data;
using mi_tienda_de_electrónica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace mi_tienda_de_electrónica.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /api/products
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_db.Products.ToList());
        }

        // POST: /api/products (protegido)
        [HttpPost]
        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
            return Ok(product);
        }

        // PUT: /api/products/{id} (protegido)
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            var existing = _db.Products.Find(id);
            if (existing == null)
                return NotFound();

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.ImageUrl = product.ImageUrl;
            _db.SaveChanges();
            return Ok(existing);
        }

        // DELETE: /api/products/{id} (protegido)
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "CookieAuth")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            _db.SaveChanges();
            return Ok();
        }
    }
}
