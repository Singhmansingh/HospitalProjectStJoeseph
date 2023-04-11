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
using Microsoft.Owin.Security;

namespace HospitalProjectStJoeseph.Controllers
{
    public class ClinicDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all clinics in the database
        /// </summary>
        /// <returns>
        /// CONTENT: all clinics in the system
        /// </returns>
        /// <example>
        /// GET: api/ClinicDaa/ListClinics
        /// </example>
        [HttpGet]
        public IEnumerable<ClinicDto> ListClinics()
        {
            List<Clinic> clinics = db.Clinics.ToList();
            List<ClinicDto> clinicDtos = new List<ClinicDto>();

            clinics.ForEach(c => clinicDtos.Add(new ClinicDto() {
                ClinicId = c.ClinicId,
                ClinicName = c.ClinicName,
                ClinicDescription = c.ClinicDescription,
                ClinicTime = c.ClinicTime
            }));

            return clinicDtos;

        }

        /// <summary>
        /// Contain all info about any clinics related to a particular service
        /// </summary>
        /// <param name="id">Service ID.</param>
        /// <returns>
        /// CONTENT: all clinics in the database, including thier associated service
        /// </returns>
        /// <example>
        /// GET: api/ClinicData/ListClinicsForService/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ClinicDto))]
        public IHttpActionResult ListClinicsforService(int id)
        {
            List<Clinic> clinics = db.Clinics.Where(
                c=>c.Services.Any(
                    s=>s.ServiceId == id
                    )).ToList();
            List<ClinicDto> clinicDtos = new List<ClinicDto>();

            clinics.ForEach(c => clinicDtos.Add(new ClinicDto()
            {
                ClinicId = c.ClinicId,
                ClinicName = c.ClinicName,
                ClinicDescription = c.ClinicDescription,
                ClinicTime= c.ClinicTime
            }));

            return Ok(clinicDtos);

        }

        /// <summary>
        /// Associate a particular service with a particular clinic
        /// </summary>
        /// <param name="ClinicId">The clinic ID primary key</param>
        /// <param name="ServiceId">The service ID primary key</param>
        ///<example>
        /// POST: api/ClinicData/AssociateClinicWithService/9/2
        ///</example>
        [HttpPost]
        [Route("api/ClinicData/AssociateClinicWithService/{clinicid}/{serviceid}")]
        public IHttpActionResult AssociateClinicWithService(int ClinicId, int ServiceId)
        {
            Clinic SelectedClinic = db.Clinics.Include(c => c.Services).Where(c => c.ClinicId == ClinicId).FirstOrDefault();
            Service SelectedService = db.Services.Find(ServiceId);

            if (SelectedClinic == null || SelectedService == null)
            {
                return NotFound();
            }

            SelectedClinic.Services.Add(SelectedService);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an associate between a particular service and a particular clinic
        /// </summary>
        /// <param name="ClinicId">The clinic ID primary Key</param>
        /// <param name="ServiceId">The service ID primary Key</param>
        /// <example>
        /// POST: api/ClinicData/UnAssociateClinicWithService/9/2
        /// </example>
        [HttpPost]
        [Route("api/ClinicData/UnAssociateClinicWithService/{clinicid}/{serviceid}")]
        public IHttpActionResult UnAssociateClinicWithService(int ClinicId, int ServiceId)
        {
            Clinic SelectedClinic = db.Clinics.Include(c=> c.Services).Where(c=> c.ClinicId==ClinicId).FirstOrDefault();
            Service SelectedService = db.Services.Find(ServiceId);

            if(SelectedClinic == null || SelectedService == null)
            {
                return NotFound();
            }

            SelectedClinic.Services.Remove(SelectedService);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Returns all clinics in the system
        /// </summary>
        /// <param name="id">The primary key of the clinic</param>
        /// <returns>
        /// CONTENT: An clinic in the system matching up to the clinic ID primary key
        /// </returns>
        /// <example>
        /// GET: api/ClinicData/FindClinic/5
        /// </example>
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

        /// <summary>
        /// Updates a particular clinic in the system with POST Data input
        /// </summary>
        /// <param name="id">Represent the clinic ID primary key</param>
        /// <param name="clinic">JSON FORM DATA of an clinic</param>
        /// <example>
        /// POST: api/ClinicData/updateClinic/5
        /// </example>
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


        /// <summary>
        /// Adds an clinic to the system
        /// </summary>
        /// <param name="clinic">JSON FORM DATA of an clinic</param>
        /// <returns>
        /// CONTENT: Clinic ID, Clinic Data
        /// </returns>
        /// <example>
        /// POST: api/ClinicData/AddClinic
        /// </example>
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

        /// <summary>
        /// Deletes an clinic from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the clinic</param>
        /// <example>
        /// POST: api/ClinicData/DeleteClinic/5
        /// </example>
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