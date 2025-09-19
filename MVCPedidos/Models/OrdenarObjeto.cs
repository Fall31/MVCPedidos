namespace MVCPedidos.Models
{
    public class OrdenarObjeto
    {
        public int Id { get; set; }
        public int IdProducto {  get; set; }
        public ICollection<ProductoModel> Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
