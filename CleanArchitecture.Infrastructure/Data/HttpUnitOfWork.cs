using Microsoft.AspNetCore.Http;
using AspNet.Security.OpenIdConnect.Primitives;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure;

namespace CleanArchitecture.Infrastructure.Data
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(CleanArchitectureContext context, IHttpContextAccessor httpAccessor) : base(context)
        {
            context.CurrentUserId = httpAccessor.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
        }
    }
}
