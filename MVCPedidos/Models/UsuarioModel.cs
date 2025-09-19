namespace MVCPedidos.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol {  get; set; }
        public ICollection<OrdenModel> Orden { get; set; }
    }
}
