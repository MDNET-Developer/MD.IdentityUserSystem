using System.ComponentModel.DataAnnotations;

namespace MD.IdentityUserSystem.Model
{
    public class UserSignInModel
    {
        [Required(ErrorMessage ="İstifadəçi adı daxil et ")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Şifrəni daxil et ")]
        public string PassWord { get; set; }
    }
}
