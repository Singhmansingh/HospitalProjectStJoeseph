using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalProjectStJoeseph.Models;

namespace HospitalProjectStJoeseph.Controllers
{
    public class PhysicianDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PhysicianData/ListPhysicians
        [Route("api/PhysicianData/ListPhysicians")]
        [HttpGet]
        public IEnumerable<Physician> ListPhysicians()
        {
            return db.Physicians.ToList();
        }

        // GET: api/PhysicianData/FindPhysician/5
        [Route("api/PhysicianData/FindPhysician/{id}")]
        [ResponseType(typeof(Physician))]
        [HttpGet]
        public IHttpActionResult FindPhysician(int id)
        {
            Physician Physician = db.Physicians.Find(id);
            if (Physician == null)
            {
                return NotFound();
            }

            return Ok(Physician);
        }

        // POST: api/PhysicianData/UpdatePhysician/5
        [Route("api/PhysicianData/UpdatePhysician/{id}")]
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePhysician(int id, Physician physician)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != physician.physician_id)
            {
                return BadRequest();
            }

            db.Entry(physician).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhysicianExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/PhysicianData/AddPhysician
        [Route("api/PhysicianData/AddPhysician")]
        [ResponseType(typeof(Physician))]
        [HttpPost]
        public IHttpActionResult AddPhysician(Physician physician)
        {
            var res = db.Physicians.Add(physician);
            db.SaveChanges();
            if (res != null)
            {
                return Ok(physician);
            }

            return BadRequest();
        }

        // POST: api/PhysicianData/DeletePhysician/5
        [Route("api/PhysicianData/DeletePhysician/{id}")]
        [ResponseType(typeof(Physician))]
        [HttpPost]
        public IHttpActionResult DeletePhysician(int id)
        {
            Physician physician = db.Physicians.Find(id);
            if (physician == null)
            {
                return NotFound();
            }

            db.Physicians.Remove(physician);
            db.SaveChanges();

            return Ok(physician);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhysicianExists(int id)
        {
            return db.Physicians.Count(e => e.physician_id == id) > 0;
        }
    }
}
