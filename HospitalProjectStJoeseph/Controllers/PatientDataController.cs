using HospitalProjectStJoeseph.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HospitalProjectStJoeseph.Controllers
{
    public class PatientDataController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //TODO: List Patients
        [HttpGet]
        [ResponseType(typeof(List<Patient>))]
        public IHttpActionResult List()
        {
            List<Patient> Patients = db.Patients.ToList();

            return Ok(Patients);
        }

        //TODO: Add Patient
        [HttpPost]
        [Authorize]

        public IHttpActionResult Add([FromBody] Patient Patient)
        {
            var res = db.Patients.Add(Patient);
            db.SaveChanges();
            if (res != null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return BadRequest();

        }

        //TODO: Update Patient
        [HttpPost]
        [Authorize]

        public IHttpActionResult UpdatePatient(int id, [FromBody] Patient Patient)
        {
            if (!PatientExists(id))
            {
                return NotFound();
            }

            db.Patients.AddOrUpdate(Patient);

            try
            {
                db.SaveChanges();

            }
            catch
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);

        }

        //TODO: Delete Patient
        [HttpPost]
        [ResponseType(typeof(Patient))]
        [Authorize]

        public IHttpActionResult DeletePatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            if (Patient == null)
            {
                return NotFound();
            }
            db.Patients.Remove(Patient);
            return Ok();
        }


        private bool PatientExists(int id)
        {
            return db.Patients.Count(p => p.PatientId == id) > 0;
        }
    }
}
