using System.ComponentModel.DataAnnotations;
namespace MVCPedidos.Models
{
    public class ProductoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es requerido")]
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }
        [Range(0.1,int.MaxValue,ErrorMessage ="El precio no puede ser negativo")]
        public decimal Precio { get; set; }
        [Range(0,int.MaxValue,ErrorMessage ="El stock no puede ser negativo")]
        public int Stock {  get; set; }
        public ICollection<OrdenarObjeto> OrdenarObjeto { get; set; }
    }
}
