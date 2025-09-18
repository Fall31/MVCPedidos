using Microsoft.EntityFrameworkCore;
using MVCPedidos.Models;
namespace MVCPedidos.Data
{
    public class PedidosDBContext: DbContext
    {
        public PedidosDBContext(DbContextOptions<PedidosDBContext> options) : base(options) { }
        public DbSet<UsuarioModel> Usuario { get; set; }
    }
}
