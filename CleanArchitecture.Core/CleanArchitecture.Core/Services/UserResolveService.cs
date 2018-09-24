using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Services
{
   public class UserResolveService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolveService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUser()
        {
            return _context.HttpContext?.User?.Identity?.Name;
        }
    }
}
