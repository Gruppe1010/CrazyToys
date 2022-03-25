using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Text;

namespace CrazyToys.Services
{
    public class SessionService : ISessionService
    {
        //Tjekker om der allerede er en SessionUser, og returnererer enten den eller opretter en ny, som returneres
        public SessionUser GetNewOrExistingSessionUser(HttpContext httpContext)
        {
            var sessionUser = httpContext.Session.GetString("SessionUser");
            if (String.IsNullOrWhiteSpace(sessionUser))
            {
                SessionUser newSessionUser = new SessionUser();
                httpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(newSessionUser));
                return newSessionUser;
            }
            return JsonConvert.DeserializeObject<SessionUser>(sessionUser);
        }

        public void Update(HttpContext httpContext, SessionUser sessionUser)
        {
            httpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(sessionUser));
        }


    }
}
