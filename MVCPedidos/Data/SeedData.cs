using MVCPedidos.Models;

namespace MVCPedidos.Data
{
    public static class SeedData
    {
        public static void Initialize(PedidosDBContext context)
        {
            context.Database.EnsureCreated();
            // Verificar si ya hay datos en la base de datos
            if (context.Usuario.Any())
            {
                return; // La base de datos ya ha sido sembrada
            }
            // Sembrar usuarios
            var usuarios = new UsuarioModel[]
            {
                new UsuarioModel 
                {
                    Email = "admin@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Nombre = "Administrador Sistema",
                    FechaNacimiento = new DateTime(1995, 1, 1),
                    FechaRegistro = new DateTime(),
                    Rol = "Admin",
                    Activo = true
                }
            };
            context.Usuario.AddRange(usuarios);
            context.SaveChanges();
            if (!context.Producto.Any())
            {
                return;
            }
            var productos = new ProductoModel[]
            {
                new ProductoModel { Nombre = "Producto A", Descripcion = "Descripción del Producto A", Precio = 10.99M, Stock = 100 },
                new ProductoModel { Nombre = "Producto B", Descripcion = "Descripción del Producto B", Precio = 15.49M, Stock = 150 },
                new ProductoModel { Nombre = "Producto C", Descripcion = "Descripción del Producto C", Precio = 7.99M, Stock = 200 }
            };
            context.Producto.AddRange(productos);
            context.SaveChanges();

        }
    }
}
