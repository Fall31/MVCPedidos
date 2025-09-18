namespace MVCPedidos.Models
{
    public class OrdenModel
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
    }
}
