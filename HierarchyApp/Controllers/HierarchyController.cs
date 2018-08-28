using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace HierarchyApp.Controllers
{
    public class HierarchyController : ApiController
    {

        [HttpGet]
        [Authorize(Roles = "admin")]
        public string Greetings()
        {
            var identity = User.Identity as ClaimsIdentity;

            return "hello world admin";
        }


        [HttpPost]
        [Authorize(Roles = "user")]
        public string Greets()
        {
            var identity = User.Identity as ClaimsIdentity;

            return "hello world user" ;
        }
    }
}