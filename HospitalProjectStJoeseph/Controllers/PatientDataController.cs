﻿using HospitalProjectStJoeseph.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace HospitalProjectStJoeseph.Controllers
{
    public class PatientDataController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List all Patients
        /// </summary>
        /// <returns>List of all Patients</returns>
        /// <example>
        /// GET: api/PatientData/List
        /// </example>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(List<Patient>))]
        [Route("api/PatientData/List")]
        public IHttpActionResult List()
        {
            List<Patient> Patients = db.Patients.ToList();

            return Ok(Patients);
        }

        /// <summary>
        /// Find a single Patient
        /// </summary>
        /// <param name="PatientId">Integer. Patient ID</param>
        /// <returns>Patient</returns>
        /// <example>
        /// GET: api/PatientData/Find/1 -> Patient
        /// </example>

        [HttpGet]
        [ResponseType(typeof(Patient))]
        [Route("api/PatientData/Find/{PatientId}")]
        public IHttpActionResult Find(int PatientId)
        {
            Patient Patient = db.Patients.Find(PatientId);
            if(Patient == null) return NotFound();
            return Ok(Patient);
        }

        /// <summary>
        /// Add a new Patient
        /// </summary>
        /// <param name="Patient">Patient Data</param>
        /// <returns>Response Code</returns>
        /// <example>
        /// POST: api/PatientData/AddPatient
        /// CONTENT: Patient DATA
        /// RESPONSE: 200 OK
        /// </example>
        [HttpPost]
        // [Authorize]
        [Route("api/PatientData/AddPatient")]

        public IHttpActionResult AddPatient([FromBody] Patient Patient)
        {
            var res = db.Patients.Add(Patient);
            db.SaveChanges();
            if (res != null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return BadRequest();

        }

        /// <summary>
        /// Update a single Patient
        /// </summary>
        /// <param name="id">Integer. Patient ID</param>
        /// <param name="Patient">Patient. Patient Data</param>
        /// <returns>Response code</returns>
        /// <example>
        /// POST: api/PatientData/UpdatePatient/1
        /// CONTENT: new Patient Data
        /// RESPONSE: 
        /// </example>
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


        /// <summary>
        /// Retrieves the Patient Data for the specified Registered User. If it is the first time retrieving the data
        /// the Patient is confirmed to be Registered
        /// </summary>
        /// <param name="id">String. User ID</param>
        /// <returns>Patient Data</returns>
        public IHttpActionResult GetPatientForUser(string id)
        {
            UserPatient up = db.UserPatients.Where(userPatient => userPatient.UserId == id).First();

            if(up==null)
            {
                return NotFound();
            }

            Patient Patient = up.Patient;

            if (!Patient.PatientIsRegistered)
            {
                db.Patients.Attach(Patient);
                Patient.PatientIsRegistered = true;
                db.SaveChanges();

            }

            return Ok(Patient);
        }

        


        private bool PatientExists(int id)
        {
            return db.Patients.Count(p => p.PatientId == id) > 0;
        }
    }
}
