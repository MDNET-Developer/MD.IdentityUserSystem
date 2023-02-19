using Microsoft.Data.SqlClient.Server;
using System.ComponentModel.DataAnnotations;

namespace MD.IdentityUserSystem.Model
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "İstifadəçi adını daxil edin")]
        public string Username { get; set; }
        [EmailAddress(ErrorMessage = "Mail formatını düzgün daxil edin")]
        [Required(ErrorMessage = "Mail adresi daxil etmək vacibdir")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifrəni daxil edin")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Daxil etdiyiniz şifrə, təkrar şifrə ilə eyni deyil")]
        public string ConfirmPassword { get; set; }
        [Required (ErrorMessage = "Cinsiyyəti daxil edin")]
        public string Gender{get; set; }

    }
}
