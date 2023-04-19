using HospitalProjectStJoeseph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HospitalProjectStJoeseph.Controllers
{
    public class AdminDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IHttpActionResult ListUsers()
        {
            return Ok();
        }
    }
}
