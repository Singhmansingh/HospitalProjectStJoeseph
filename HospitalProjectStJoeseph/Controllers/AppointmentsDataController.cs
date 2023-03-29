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
    public class AppointmentsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AppointmentsData/ListAppointments
        [Route("api/AppointmentsData/ListAppointments")]
        [HttpGet]
        public IEnumerable<Appointment> ListAppointments()
        {
            return db.Appointments.ToList();
        }

        // GET: api/AppointmentsData/FindAppointment/5
        [Route("api/AppointmentsData/FindAppointment/{id}")]
        [ResponseType(typeof(Appointment))]
        [HttpGet]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // POST: api/AppointmentsData/UpdateAppointment/5
        [Route("api/AppointmentsData/UpdateAppointment/{id}")]
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.appointmentId)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // POST: api/AppointmentsData/AddAppointment
        [Route("api/AppointmentsData/AddAppointment")]
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddPhysician(Appointment appointment)
        {
            var res = db.Appointments.Add(appointment);
            db.SaveChanges();
            if (res != null)
            {
                return Ok(appointment);
            }

            return BadRequest();
        }

        // POST: api/AppointmentsData/DeleteAppointment/5
        [Route("api/AppointmentsData/DeleteAppointment/{id}")]
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.appointmentId == id) > 0;
        }
    }
}
