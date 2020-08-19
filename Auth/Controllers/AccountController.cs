using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Auth.Models;
using ToDoApi.Auth.ViewModels;
using ToDoApi.Infrastructure.Helper;

namespace ToDoApi.Auth.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly JwtHelper jwtHelper;
        public UserManager<AppUser> userManager { get; }
        public AccountController(JwtHelper jwtHelper, UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            this.jwtHelper = jwtHelper;

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user=new AppUser(){Email=model.Email,UserName=model.Username};
            var result=await userManager.CreateAsync(user,model.Password);
            if(result.Succeeded)
            {
                var againUser=await userManager.FindByEmailAsync(model.Email);
                string token=jwtHelper.GenerateJwtToken(againUser.UserName,againUser.Id);
                Response.Headers.Add("jwt-access-token", token);
                return Ok();
            }else
            {
                return BadRequest(new Message(){Report="Something Went Wrong."});
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await userManager.FindByNameAsync(model.Username);
            if(user == null) return Unauthorized(new Message() { Report = "Wrong Username or Password." }); 
            var result = await userManager.CheckPasswordAsync(user,model.Password);
            if(result)
            {
                string token=jwtHelper.GenerateJwtToken(user.UserName,user.Id);
                Response.Headers.Add("jwt-access-token",token);
                return Ok();
            }else
            {
                return Unauthorized(new Message(){Report="Wrong Username or Password."});
            }
        }
    }

    class Message
    {
        public string Report { get; set; }
    }
}