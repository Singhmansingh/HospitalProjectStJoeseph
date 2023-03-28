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
        [Route("api/PatientData/List")]
        public IHttpActionResult List()
        {
            List<Patient> Patients = db.Patients.ToList();

            return Ok(Patients);
        }

        [HttpGet]
        [ResponseType(typeof(Patient))]
        [Route("api/PatientData/Find/{PatientId}")]
        public IHttpActionResult Find(int PatientId)
        {
            Patient Patient = db.Patients.Find(PatientId);
            if(Patient == null) return NotFound();
            return Ok(Patient);
        }

        //TODO: Add Patient
        [HttpPost]
        // [Authorize]
        [Route("api/PatientData/Add")]

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
        // [Authorize]
        [Route("api/PatientData/UpdatePatient/{id}")]

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
        // [Authorize]
        [Route("api/PatientData/DeletePatient/{id}")]

        public IHttpActionResult DeletePatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            if (Patient == null)
            {
                return NotFound();
            }
            db.Patients.Remove(Patient);
            db.SaveChanges();
            return Ok();
        }

        //TOD: List Best Wishes for Patient
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        [Route("api/PatientData/FindPatientWithBestWishes/{PatientId}")]

        public IHttpActionResult FindPatientWithBestWishes(int PatientId)
        {
            Patient Patient = db.Patients.Find(PatientId);
            if (Patient == null) return NotFound();

            List<BestWish> bestWishes = db.BestWishes.Where(bw => bw.PatientId == PatientId).ToList();

            PatientDto dto = new PatientDto();
            dto.Patient = Patient;

            if (bestWishes.Count > 0)
            {
                dto.BestWishes = bestWishes;
            }
            return Ok(dto);

        }


        private bool PatientExists(int id)
        {
            return db.Patients.Count(p => p.PatientId == id) > 0;
        }
    }
}
