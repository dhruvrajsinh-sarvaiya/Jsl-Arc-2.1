using System.Collections.Generic;
using CleanArchitecture.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.API
{
    public class AppUtils
    {
        internal static IActionResult SignIn(ApplicationUser user, IList<string> roles)
        {
            var userResult = new { User = new { DisplayName = user.UserName, Roles = roles } };
            return new ObjectResult(userResult);
        }

    }
}