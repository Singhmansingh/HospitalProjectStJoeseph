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
    public class ServiceDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all service in the system.
        /// </summary>
        /// <returns>
        ///  CONTENT: all service in the database
        /// </returns>
        /// <example>
        /// GET: api/servicedata/listservices
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ServiceDto))]
        public IHttpActionResult ListServices()
        {
            List<Service> services = db.Services.ToList();
            List<ServiceDto> serviceDtos = new List<ServiceDto>();

            services.ForEach(s => serviceDtos.Add(new ServiceDto()
            {
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                ServiceTime = s.ServiceTime
            }));

            return Ok(serviceDtos);
        }

        /// <summary>
        /// Returns all services in the system associated with a particular clinic.
        /// </summary>
        /// <param name="id">Clinic Primary Key</param>
        /// <returns>
        /// C0NTENT: all services in the database associate with a particular clinic.
        /// </returns>
        /// <example>
        /// GET: api/ServiceData/ListServicesforClinic/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ServiceDto))]
        public IHttpActionResult ListServicesforClinic(int id)
        {
            List<Service> services = db.Services.Where(
                s => s.Clinic.Any(
                    c => c.ClinicId == id)
                ).ToList();
            List<ServiceDto> serviceDtos = new List<ServiceDto>();

            services.ForEach(s => serviceDtos.Add(new ServiceDto()
            {
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                ServiceTime = s.ServiceTime
            }));

            return Ok(serviceDtos);
        }
        
        /// <summary>
        /// Returns Services in the system not associate for a particular clinic.
        /// </summary>
        /// <param name="id">Clinic Primary Key</param>
        /// <returns>
        /// CONTENT: all services in the database not taking care of  a particular clinic
        /// </returns>
        //GET: api/ServiceData/ListServicesNotForClinic/1
        [HttpGet]
        [ResponseType(typeof(ServiceDto))]
        public IHttpActionResult ListServicesNotForClinic(int id)
        {
            List<Service> services = db.Services.Where(
                s => !s.Clinic.Any(
                    c => c.ClinicId == id)
                ).ToList();
            List<ServiceDto> serviceDtos = new List<ServiceDto>();

            services.ForEach(s => serviceDtos.Add(new ServiceDto()
            {
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                ServiceTime = s.ServiceTime
            }));

            return Ok(serviceDtos);
        }

        /// <summary>
        /// Returns all services in the system.
        /// </summary>
        /// <param name="id">The primary key of the service</param>
        /// <returns>
        /// CONTENT: An service in the system matching up to the service ID primary key
        /// </returns>
        //GET: api/ServiceData/FindService/5
        [ResponseType(typeof(ServiceDto))]
        [HttpGet]
        public IHttpActionResult FindService(int id)
        {
            Service services = db.Services.Find(id);
            ServiceDto serviceDtos = new ServiceDto()
            {
                ServiceId = services.ServiceId,
                ServiceName = services.ServiceName,
                ServiceTime = services.ServiceTime
            };
            if (services == null)
            {
                return NotFound();
            }

            return Ok(serviceDtos);
        }

        /// <summary>
        /// Updates a particular service in the system POST Data input
        /// </summary>
        /// <param name="id">Represent the service ID primary key</param>
        /// <param name="service">JSON FORM DATA of an service</param>
        //POST: api/ServiceData/UpdateService/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceId)
            {

                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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
        /// Adds an service to the system
        /// </summary>
        /// <param name="service">JSON form data of an service</param>
        /// <returns>
        /// CONTENT: service ID, Service Data
        /// </returns>
        //POST: api/ServiceData/AddService
        [ResponseType(typeof(Service))]
        [HttpPost]
        public IHttpActionResult AddService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Services.Add(service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.ServiceId }, service);
        }

        /// <summary>
        /// Deletes an Service from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the service</param>
=        //POST: api/ServiceData/DeleteService/5
        [ResponseType(typeof(Service))]
        [HttpPost]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
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

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceId == id) > 0;
        }

    }
}
