using HospitalProjectStJoeseph.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HospitalProjectStJoeseph.Controllers
{
    public class ClinicDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ClinicData/ListClinics
        [HttpGet]
        public IEnumerable<ClinicDto> ListClinics()
        {
            List<Clinic> clinics =  db.Clinics.ToList();
            List <ClinicDto> clinicDtos = new List<ClinicDto>();

            clinics.ForEach(c => clinicDtos.Add(new ClinicDto(){
                ClinicId = c.ClinicId,
                ClinicName = c.ClinicName,
                ClinicDescription = c.ClinicDescription,
                ClinicTime = c.ClinicTime
            }));

            return clinicDtos;

        }

        // GET: api/ClinicData/FindClinic/5
        [ResponseType(typeof(ClinicDto))]
        [HttpGet]
        public IHttpActionResult FindClinic(int id)
        {

            Clinic clinic = db.Clinics.Find(id);
            ClinicDto clinicDto = new ClinicDto()
            {
                ClinicId = clinic.ClinicId,
                ClinicName = clinic.ClinicName,
                ClinicDescription = clinic.ClinicDescription,
                ClinicTime = clinic.ClinicTime
            };
            if (clinic == null)
            {
                return NotFound();
            }

            return Ok(clinicDto);
        }

        // POST: api/ClinicData/updateClinic/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateClinic(int id, Clinic clinic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clinic.ClinicId)
            {
                return BadRequest();
            }

            db.Entry(clinic).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ClinicData/AddClinic
        [ResponseType(typeof(Clinic))]
        [HttpPost]
        public IHttpActionResult AddClinic(Clinic clinic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clinics.Add(clinic);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = clinic.ClinicId }, clinic);
        }

        // POST: api/ClinicData/DeleteClinic/5
        [ResponseType(typeof(Clinic))]
        [HttpPost]
        public IHttpActionResult DeleteClinic(int id)
        {
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return NotFound();
            }

            db.Clinics.Remove(clinic);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClinicExists(int id)
        {
            return db.Clinics.Count(e => e.ClinicId == id) > 0;
        }
    }
}