using System.ComponentModel.DataAnnotations;
namespace MVCPedidos.Models
{
    public class OrdenarObjeto
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public ProductoModel Producto { get; set; }
        [Range(0,int.MaxValue,ErrorMessage ="La cantidad no puede ser negativa")]
        public int Cantidad { get; set; }
        [Range(0.1,int.MaxValue,ErrorMessage ="El subtotal no puede ser negativo")]
        public decimal Subtotal { get; set; }
    }
}
