using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlexApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlexApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<MyUser> userManager;
        private readonly SignInManager<MyUser> signInManager;

        public UserController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Route("GetClaims")]
        [Authorize]
        public IActionResult GetClaims()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
        [HttpPost]
        [Route("CreateNewUser")]
        public async Task<IActionResult> CreateNewUserAsync(UserSubscriptionModel newUser)
        {
            IdentityResult result = new IdentityResult();
            result = await userManager.CreateAsync(new MyUser
            {
                UserName = newUser.Mail,
                Email = newUser.Mail,
                Nom = newUser.Nom,
                Prenom = newUser.Prenom,
                Mobile = newUser.Mobile,
                Sexe = newUser.Sexe,
                DateDeNaissance = newUser.DateDeNaissance,
                Professionnel = newUser.Professionnel

            }, newUser.Password);


            if (result.Succeeded)
            {
                return Created("mysite.com", result);
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var result = await signInManager.PasswordSignInAsync(
                userName: login.Email,
                password: login.Password,
                isPersistent: true,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                ModelState.AddModelError("Login impossible", "Unne erreur s'est produite lors de la tentative de connexion");
                return BadRequest(ModelState);
            }
        }
    }
}