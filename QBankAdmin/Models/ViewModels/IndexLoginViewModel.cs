namespace QBankAdmin.Models.ViewModels
{
    public class IndexLoginViewModel
    {
        public string CorreoElectronico { get; set; } = null!;
        public string Contrasena { get; set; } = null!;

        public List<string>? Errores { get; set; }  
    }
}
