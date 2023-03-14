using AttentionAxia.DTOs;
using AttentionAxia.Helpers;
using AttentionAxia.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AttentionAxia.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController()
        {

        }
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Account
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                var response = await new UserRepository().VerifyLogin(login);
                if (!response.IsSuccess)
                    return Json(new { response.IsSuccess, response.Message });

                var data = (UserDTO)response.Data;
                SaveCookies(data.FullName, data.Rol, data.Id.ToString());
                return Json(new { response.IsSuccess, response.Message });
            }
            return View(login);
        }

        [HttpPost]
        public async Task<JsonResult> ValidUser(string email)
        {
            var passwordHash = HashHelper.GenerateHashWithPassword("Password123!");
            var resp = HashHelper.VerifyPassword("Password123!", passwordHash);
            var response = await new UserRepository().ExistUser(email);
            return Json(new { response.IsSuccess, response.Message });
        }
    }
}