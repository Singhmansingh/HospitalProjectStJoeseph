using HospitalProjectStJoeseph.Models;
using HospitalProjectStJoeseph.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HospitalProjectStJoeseph.Controllers
{
    public class UserDataController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Lists all users in the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IHttpActionResult ListUsers()
        {
            List<ApplicationUser> Users = db.Users.ToList();
            List<UserDto> UserDtos = new List<UserDto>();

            foreach(ApplicationUser User in Users)
            {
                UserDto dto = new UserDto()
                {
                    UserId = User.Id,
                    UserEmail = User.Email
                };

                UserDtos.Add(dto);
            }

            return Ok(UserDtos);
        }


        /// <summary>
        /// Assigns a User Account to a Patient
        /// </summary>
        /// <param name="PatientId">Integer. Patient ID</param>
        /// <param name="_UserId">String. Application User ID</param>
        /// <returns>Status code (200 OK)</returns>
        [HttpGet]
        [Route("api/UserData/AssignPatientToUser/{PatientId}/{_UserId}")]
        public IHttpActionResult AssignPatientToUser(int PatientId, string _UserId)
        {
            Patient patient = db.Patients.Find(PatientId);
            if (patient == null) return NotFound();
            Debug.WriteLine("Patient Found!");


            ApplicationUser User = db.Users.Find(_UserId);
            Debug.WriteLine("UserID", _UserId);
            if (User == null) return NotFound();
            Debug.WriteLine("User Found!");


            UserPatient userPatient = new UserPatient()
            {
                UserId = _UserId,
                PatientId = PatientId
            };

            IdentityRole Role = db.Roles.Where(r => r.Name == "Patient").First();


            IdentityUserRole userRole = new IdentityUserRole() { UserId = _UserId, RoleId = Role.Id };

            string uid = _UserId;
            string rid = Role.Id;

            string query = "INSERT INTO dbo.AspNetUserRoles(UserId,RoleId) VALUES (@p0,@p1)";
            Debug.WriteLine(query);
            // User - Roles
            db.Database.ExecuteSqlCommand(query, uid, 3);

            // User - Patient
            db.UserPatients.Add(userPatient);


            db.SaveChanges();

            return Ok();
        }
    }
}
