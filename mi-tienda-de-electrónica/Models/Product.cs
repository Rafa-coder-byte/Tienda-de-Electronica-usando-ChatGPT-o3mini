namespace mi_tienda_de_electrónica.Models
{
    
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
        // Nueva propiedad para reseñas
        public ICollection<Review> Reviews { get; set; } = new List<Review>();


        // Constructor personalizado (puedes inicializar valores por defecto si lo necesitas)
        public Product(string name, string description, decimal price, string imageUrl)
        {
            Name = name;
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
        }

        // Constructor sin parámetros (para que EF Core funcione sin problemas)
        public Product() { }



    }
 

}
