using CrazyToys.Entities.DTOs;
using Microsoft.AspNetCore.Http;


namespace CrazyToys.Interfaces
{
    public interface ISessionService
    {
        SessionUser GetNewOrExistingSessionUser(HttpContext httpContext);

        void Update(HttpContext httpContext, SessionUser sessionUser);

    }
}
