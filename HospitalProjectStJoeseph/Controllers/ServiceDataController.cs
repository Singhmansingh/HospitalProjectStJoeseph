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

        //GET: api/servicedata/listservices
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

        //GET: api/ServiceData/
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

        //GET: api/ServiceData/
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

        //POST: api/ServiceData/DeleteService/5
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
