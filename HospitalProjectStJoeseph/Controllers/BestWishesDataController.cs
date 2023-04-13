using HospitalProjectStJoeseph.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HospitalProjectStJoeseph.Controllers
{
    public class BestWishesDataController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List all Best Wishes
        /// </summary>
        /// <returns>List of Best Wishes</returns>
        [HttpGet]
        [ResponseType(typeof(List<BestWish>))]
        [Route("api/BestWishesData/ListAllBestWishes")]
        public IHttpActionResult ListAllBestWishes()
        {
            List<BestWish> bestWishes = db.BestWishes.ToList();

            return Ok(bestWishes);
        }
        

        /// <summary>
        /// Find a Best Wish by ID
        /// </summary>
        /// <param name="BestWishId">Integer. Best Wish ID</param>
        /// <returns>Best Wish</returns>
        /// <example>
        /// GET: api/BestWishesData/FindBestWish/1 -> Best wish of ID 1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(BestWish))]
        [Route("api/BestWishesData/FindBestWish/{BestWishId}")]
        public IHttpActionResult FindBestWish(int BestWishId)
        {
            BestWish bestWish = db.BestWishes.Find(BestWishId);
            if(bestWish != null)
            {
                return Ok(bestWish);
            }

            return BadRequest();
        }

        /// <summary>
        /// Gets a Patient with their associated Best Wishes
        /// </summary>
        /// <param name="PatientId">Patient ID</param>
        /// <returns>Patient with Best Wishes</returns>
        /// <example>
        /// GET: api/BestWishesData/ListBestWishesForPatient/1 -> Patient of ID 1 Bets Wishes for Patient 1
        /// </example>

        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        [Route("api/BestWishesData/ListBestWishesForPatient/{PatientId}")]
        public IHttpActionResult ListBestWishesForPatient(int PatientId)
        {
            List<BestWish> BestWishes = db.BestWishes.Where(bw => bw.PatientId == PatientId).OrderByDescending(bw=> bw.BestWishSendDate).ToList();
            Patient Patient = db.Patients.Find(PatientId);
            PatientDto PatientDto = new PatientDto()
            {
                Patient = Patient,
                BestWishes = BestWishes
            };
            return Ok(PatientDto);
        }


        /// <summary>
        /// Adds a new Best Wish
        /// </summary>
        /// <param name="BestWish">BestWish. Data to add</param>
        /// <returns>Response Code</returns>
        /// <example>
        /// POST: api/BestWishesData/AddBestWish
        /// CONTENT: Patient DATA
        /// RESPONSE: 200 OK
        /// </example>
        [HttpPost]
       // [Authorize]
        [Route("api/BestWishesData/AddBestWish")]

        public IHttpActionResult AddBestWish([FromBody] BestWish BestWish)
        {
            var res = db.BestWishes.Add(BestWish);
            db.SaveChanges();
            if (res != null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return BadRequest();

        }

        /// <summary>
        /// Updates a single Best Wish
        /// </summary>
        /// <param name="id">Integer. Best Wish ID</param>
        /// <param name="BestWish">BestWish. Data to update</param>
        /// <returns>Response Code</returns>
        /// <example>
        /// POST: api/BestWishesData/UpdateBestWish/1 
        /// CONTENT: BestWish
        /// RESPONSE: 200 OK
        /// </example>
        [HttpPost]
        // [Authorize]
        [Route("api/BestWishesData/UpdateBestWish/{id}")]

        public IHttpActionResult UpdateBestWish(int id, [FromBody] BestWish BestWish)
        {
            if(!BestWishExists(id))
            {
                return NotFound();
            }

            db.BestWishes.AddOrUpdate(BestWish);

            try
            {
                db.SaveChanges();

            } catch
            {
                return BadRequest();
            }

            return Ok();

        }

        /// <summary>
        /// Removes a Best Wish from the Database
        /// </summary>
        /// <param name="id">Integer. Best Wish ID</param>
        /// <returns>Response Code</returns>
        /// <example>
        /// POST: api/BestWishesData/DeleteBestWish/1 -> 200 OK
        /// </example>
        [HttpPost]
        [ResponseType(typeof(BestWish))]
        // [Authorize]
        [Route("api/BestWishesData/DeleteBestWish/{id}")]

        public IHttpActionResult DeleteBestWish(int id)
        {
            BestWish bestWish = db.BestWishes.Find(id);
            if(bestWish == null)
            {
                return NotFound();
            }
            db.BestWishes.Remove(bestWish);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Sets a Best Wish to Read
        /// </summary>
        /// <param name="BestWishId">Integer. Best Wish ID</param>
        /// <returns>Response code</returns>
        /// <example>
        /// GET: api/BestWishesData/SetBestWishRead/1 -> 200 OK
        /// </example>
        [HttpGet]
        [Route("api/BestWishesData/SetBestWishRead/{BestWishId}")]
        public IHttpActionResult SetBestWishRead(int BestWishId)
        {
            BestWish bestWish = db.BestWishes.Find(BestWishId);
            bestWish.BestWishIsRead = true;
            db.SaveChanges();
            return Ok();
        }

        private bool BestWishExists(int id)
        {
            return db.BestWishes.Count(bw => bw.BestWishId == id) > 0;
        }
    }
}
