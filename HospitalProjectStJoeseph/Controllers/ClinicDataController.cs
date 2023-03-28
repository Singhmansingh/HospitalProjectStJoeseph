using HospitalProjectStJoeseph.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Diagnostics;
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

        [HttpGet]
        [ResponseType(typeof(ClinicDto))]
        public IHttpActionResult ListClinic()
        {
            List<Clinic> Clinics = db.Clinics.ToList();
            List<ClinicDto> clinicDtos = new List<ClinicDto>();

            Clinics.ForEach(c => clinicDtos.Add(new ClinicDto()
            {
                ClinicId = c.ClinicId,
                ClinicName = c.ClinicName,
                ClinicDescription = c.ClinicDescription,
                ClinicTime = c.ClinicTime
            }));

            return Ok(clinicDtos);
        }

        [HttpPost]
        [Route("api/ClinicData/AssociateClinicWithService/{clinicid}/{serviceid}")]
        public IHttpActionResult AssociateClinicWithService(int clinicid, int serviceid)
        {

            Clinic SelectedClinic = db.Clinics.Include(a => a.Services).Where(a => a.ClinicId == clinicid).FirstOrDefault();
            Service SelectedService = db.Services.Find(serviceid);

            if (SelectedClinic == null || SelectedService == null)
            {
                return NotFound();
            }

            SelectedClinic.Services.Add(SelectedService); 
            db.SaveChanges();

            return Ok();
        }


        [HttpPost]
        [Route("api/ClinicData/UnAssociateClinicWithService/{clinicid}/{serviceid}")]
        public IHttpActionResult UnAssociateClinicWithService(int clinicid, int serviceid)
        {

            Clinic SelectedClinic = db.Clinics.Include(a => a.Services).Where(a => a.ClinicId == clinicid).FirstOrDefault();
            Service SelectedService = db.Services.Find(serviceid);

            if (SelectedClinic == null || SelectedService == null)
            {
                return NotFound();
            }

            SelectedClinic.Services.Remove(SelectedService);
            db.SaveChanges();

            return Ok();
        }

        [ResponseType(typeof(ClinicDto))]
        [HttpGet]
        public IHttpActionResult FindClinic(int id)
        {
            Clinic Clinic = db.Clinics.Find(id);
            ClinicDto clinicDto = new ClinicDto()
            {
                ClinicId = Clinic.ClinicId,
                ClinicName = Clinic.ClinicName,
                ClinicDescription = Clinic.ClinicDescription,
                ClinicTime = Clinic.ClinicTime
               
            };
            if (Clinic == null)
            {
                return NotFound();
            }

            return Ok(clinicDto);
        }

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
