using System.ComponentModel.DataAnnotations;
namespace MVCPedidos.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es requerido")]
        [StringLength (int.MaxValue,MinimumLength =2)]
        public string Nombre { get; set; }
        [Required(ErrorMessage ="El email es requerido")]
        public string Email { get; set; }
        [Required (ErrorMessage ="La contraseña es necesaria")]
        [StringLength(100,MinimumLength =8, ErrorMessage ="La contraseña no es menor de 8 caracteres")]
        public string Password { get; set; }
        [Required(ErrorMessage ="El rol es necesario")]
        public string Rol {  get; set; }
        public ICollection<OrdenModel> Orden { get; set; }
    }
}
