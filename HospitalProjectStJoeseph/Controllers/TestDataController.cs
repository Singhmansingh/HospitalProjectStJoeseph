using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalProjectStJoeseph.Models;
using System.Diagnostics;

namespace HospitalProjectStJoeseph.Controllers
{
    public class TestDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [ResponseType(typeof(TestDto))]
        public IHttpActionResult ListTests()
        {
            List<Test> Tests = db.Tests.ToList();
            List<TestDto> TestDtos = new List<TestDto>();

            Tests.ForEach(t => TestDtos.Add(new TestDto()
            {
                TestID = t.TestID,
                test_category = t.test_category,
                test_date = t.test_date
            }));

            return Ok(TestDtos);
        }


        [ResponseType(typeof(TestDto))]
        [HttpGet]
        public IHttpActionResult FindTest(int id)
        {

            Test Test = db.Tests.Find(id);
            TestDto TestDto = new TestDto()
            {
                TestID = Test.TestID,
                test_category = Test.test_category,
                test_date = Test.test_date
            };
            if (Test == null)
            {
                return NotFound();
            }

            return Ok(TestDto);
        }

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTest(int id, Test Test)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Test.TestID)
            {

                return BadRequest();
            }

            db.Entry(Test).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
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


        [ResponseType(typeof(Test))]
        [HttpPost]
        public IHttpActionResult AddTest(Test Test)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tests.Add(Test);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Test.TestID }, Test);
        }


        [ResponseType(typeof(Test))]
        [HttpPost]
        public IHttpActionResult DeleteTest(int id)
        {
            Test Test = db.Tests.Find(id);
            if (Test == null)
            {
                return NotFound();
            }

            db.Tests.Remove(Test);
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

        private bool TestExists(int id)
        {
            return db.Tests.Count(e => e.TestID == id) > 0;
        }
    }
}