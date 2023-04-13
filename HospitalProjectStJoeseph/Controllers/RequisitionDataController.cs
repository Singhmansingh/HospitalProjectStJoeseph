using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
using HospitalProjectStJoeseph.Models;
using System.Diagnostics;

namespace HospitalProjectStJoeseph.Controllers
{
    public class RequisitionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [ResponseType(typeof(RequisitionDto))]
        public IHttpActionResult ListRequisitions()
        {
            List<Requisition> Requisitions = db.Requisitions.ToList();
            List<RequisitionDto> RequisitionDtos = new List<RequisitionDto>();

            // loop through the database Requisition table to get all information
            Requisitions.ForEach(a => RequisitionDtos.Add(new RequisitionDto()
            {
                RequisitionID = a.RequisitionID,
                record_date = a.record_date,
                test_result = a.test_result,
                PatientId = a.Patient.PatientId,
                PatientName = a.Patient.PatientName,
                physician_id = a.Physician.physician_id,
                physician_first_name = a.Physician.first_name,
                physician_last_name = a.Physician.last_name,
                ClinicId = a.Clinic.ClinicId,
                ClinicName = a.Clinic.ClinicName,
                TestID = a.Test.TestID,
                test_category = a.Test.test_category,
                test_date = a.Test.test_date
            }));

            return Ok(RequisitionDtos);
        }


        [HttpGet]
        [ResponseType(typeof(RequisitionDto))]
        public IHttpActionResult ListRequisitionsForTest(int id)
        {
            List<Requisition> Requisitions = db.Requisitions.Where(a => a.TestID == id).ToList();
            List<RequisitionDto> RequisitionDtos = new List<RequisitionDto>();

            Requisitions.ForEach(a => RequisitionDtos.Add(new RequisitionDto()
            {
                RequisitionID = a.RequisitionID,
                record_date = a.record_date,
                test_result = a.test_result,
                PatientId = a.Patient.PatientId,
                PatientName = a.Patient.PatientName,
                physician_id = a.Physician.physician_id,
                physician_first_name = a.Physician.first_name,
                physician_last_name = a.Physician.last_name,
                ClinicId = a.Clinic.ClinicId,
                ClinicName = a.Clinic.ClinicName,
                TestID = a.Test.TestID,
                test_category = a.Test.test_category,
                test_date = a.Test.test_date
            }));

            return Ok(RequisitionDtos);
        }


        [ResponseType(typeof(RequisitionDto))]
        [HttpGet]
        public IHttpActionResult FindRequisition(int id)
        {
            Requisition Requisition = db.Requisitions.Find(id);
            RequisitionDto RequisitionDto = new RequisitionDto()
            {
                RequisitionID = Requisition.RequisitionID,
                record_date = Requisition.record_date,
                test_result = Requisition.test_result,
                PatientId = Requisition.Patient.PatientId,
                PatientName = Requisition.Patient.PatientName,
                physician_id = Requisition.Physician.physician_id,
                physician_first_name = Requisition.Physician.first_name,
                physician_last_name = Requisition.Physician.last_name,
                ClinicId = Requisition.Clinic.ClinicId,
                ClinicName = Requisition.Clinic.ClinicName,
                TestID = Requisition.Test.TestID,
                test_category = Requisition.Test.test_category,
                test_date = Requisition.Test.test_date
            };
            if (Requisition == null)
            {
                return NotFound();
            }

            return Ok(RequisitionDto);
        }

  
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRequisition(int id, Requisition Requisition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Requisition.RequisitionID)
            {
                return BadRequest();
            }

            db.Entry(Requisition).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequisitionExists(id))
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


        [ResponseType(typeof(Requisition))]
        [HttpPost]
        public IHttpActionResult AddRequisition(Requisition Requisition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Requisitions.Add(Requisition);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Requisition.RequisitionID }, Requisition);
        }


        [ResponseType(typeof(Requisition))]
        [HttpPost]
        public IHttpActionResult DeleteRequisition(int id)
        {
            Requisition Requisition = db.Requisitions.Find(id);
            if (Requisition == null)
            {
                return NotFound();
            }

            db.Requisitions.Remove(Requisition);
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

        private bool RequisitionExists(int id)
        {
            return db.Requisitions.Count(e => e.RequisitionID == id) > 0;
        }
    }
}