using System.ComponentModel.DataAnnotations;
namespace MVCPedidos.Models
{
    public class OrdenModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
