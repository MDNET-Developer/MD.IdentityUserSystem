using MD.IdentityUserSystem.Entities;
using MD.IdentityUserSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MD.IdentityUserSystem.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
                    var memberRole = await _roleManager.FindByNameAsync("Member");
                    if (memberRole == null)
                    {
                        AppRole role = new()
                        {
                            Name = "Member",
                            CreatedDate = System.DateTime.Now
                        };
                        await _roleManager.CreateAsync(role);
                    }
                   

                    await _userManager.AddToRoleAsync(appUser, "Member");

                    return RedirectToAction("SignIn");
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
                lockoutOnFailure -  yanlis girisler zamani hemin hesabi kilidleryir
                 */
              var signInResult =  await _signInManager.PasswordSignInAsync(userSignIn.UserName, userSignIn.PassWord, isPersistent: false, lockoutOnFailure: false);
                
                if (signInResult.Succeeded)
                {

                    //Bu yanasma duzgun deyil cunki User.IsInRole isletmek ucun bizim IAction yuxarsindan yazdigimiz [Authorize] atributu lazimdir. Bizde de o atribut olmadigi ucun yaza bilmirik
                    //if (User.IsInRole("Admin"))
                    //{
                    //    return RedirectToAction("AdminPanel");
                    //}
                    //else
                    //{
                    //    return RedirectToAction("UserPanel");
                    //}
                    /*
                     O zaman biz userin Role melumatini cekek metod ile. Lakin role melumatini getirmek ucun bizden metodda userin adini isteyir. Evvelce onu cekek sonra role melumatini tapaq.
                     */
                    var userName = await _userManager.FindByNameAsync(userSignIn.UserName);
                    var userRole = await _userManager.GetRolesAsync(userName);
                    if (userRole.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else
                    {
                        return RedirectToAction("UserPanel");
                    }
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
            return View(userSignIn);
        }


        public IActionResult GetUserInfo()
        {
            var userName = User.Identity.Name;
            var userRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value  ;
            ViewData["UserName"] = $"{userName} / {userRole}";
            //ViewBag.UserName 
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult UserPanel()
        {
            return View();
        }
    }
}
