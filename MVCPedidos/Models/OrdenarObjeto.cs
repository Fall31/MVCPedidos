namespace MVCPedidos.Models
{
    public class OrdenarObjeto
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
