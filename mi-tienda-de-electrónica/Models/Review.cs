// Models/Review.cs
using mi_tienda_de_electrónica.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace mi_tienda_de_electrónica.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; } // Relación con el producto

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Propiedad de navegación
        public Product Product { get; set; }



        // Constructor sin parámetros (para que EF Core funcione sin problemas)
        public Review() { }

    }
}
