using System.ComponentModel.DataAnnotations;
namespace MVCPedidos.Models
{
    public class OrdenModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        [Required(ErrorMessage ="La fecha es requerida")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage ="El estado es requerido")]
        public string Estado { get; set; }
        [Range(0.1,int.MaxValue,ErrorMessage ="El total no puede ser negativo")]
        public decimal Total { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
