using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;

namespace MyCMS.Web.ViewComponents
{
    public class LoginPartialViewComponent : ViewComponent
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginPartialViewComponent(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            return View(user);
        }
    }
}
