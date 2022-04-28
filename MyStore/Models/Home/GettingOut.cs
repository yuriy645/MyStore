using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models.Home
{
    public class GettingOut
    {
        public void GetOutClient(HttpContext httpContext)
        {
            Client client = null;
            httpContext.Session.Set<Client>("authorizedClient", client);
        }
    }
}
