using Microsoft.EntityFrameworkCore;
using MVCPedidos.Models;
namespace MVCPedidos.Data
{
    public class PedidosDBContext: DbContext
    {
        public PedidosDBContext(DbContextOptions<PedidosDBContext> options) : base(options) { }
        public DbSet<UsuarioModel> Usuario { get; set; }
        public DbSet<ProductoModel> Producto { get; set; }
        public DbSet<OrdenModel> Orden { get; set; }
        public DbSet<OrdenarObjeto> OrdenarObjeto { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrdenModel>()
                .HasOne(u => u.Usuario)
                .WithMany(o => o.Orden)
                .HasForeignKey(u => u.IdUsuario);

            modelBuilder.Entity<OrdenarObjeto>()
                .HasOne(p => p.Producto)
                .WithMany(o => o.OrdenarObjeto)
                .HasForeignKey(p => p.IdProducto);

            base.OnModelCreating(modelBuilder);

            // Configurar índice único para el email
            modelBuilder.Entity<UsuarioModel>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
