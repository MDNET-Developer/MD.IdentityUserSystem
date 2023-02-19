using MD.IdentityUserSystem.Entities;
using MD.IdentityUserSystem.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MD.IdentityUserSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            UserCreateModel userCreate = new();
            return View(userCreate);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel userCreate)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser =  new()
                {
                    UserName=userCreate.Username,
                    PasswordHash=userCreate.Password,
                    Email=userCreate.Email,
                    Gender=userCreate.Gender,
                };

                var identityResult = await _userManager.CreateAsync(/*new AppUser() { UserName=userCreate.Username,ardi gedir....}*/appUser, userCreate.Password);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View();
                }
                
            }
            else
            {
                return View(userCreate);
            }

        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel userSignIn)
        {
            if (ModelState.IsValid)
            {
                /*
                 isPersistent - istifadecini melumatlarini default zaman erzinde coockike de tutur
                lockoutOnFailure - yanlis girisler zamani hemin hesabi kilidleryir
                 */
              var signInResult =  await _signInManager.PasswordSignInAsync(userSignIn.UserName, userSignIn.PassWord, isPersistent: false, lockoutOnFailure: true);

                if (signInResult.Succeeded)
                {
                    //Giris ugurlu
                }
                else if (signInResult.IsLockedOut)
                {
                    //Hesab kilidli
                }
                else if (signInResult.IsNotAllowed)
                {
                    //mail veya telefon nomresi dogrulanmali
                }
            }
            return View();
        }
    }
}
