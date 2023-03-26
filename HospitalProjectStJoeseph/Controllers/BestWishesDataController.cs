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

        //TODO: List Best Wishes
        [HttpGet]
        [ResponseType(typeof(List<BestWish>))]
        public IHttpActionResult List()
        {
            List<BestWish> bestWishes = db.BestWishes.ToList();

            return Ok(bestWishes);
        }

        //TOD: List Best Wishes for Patient
        [HttpGet]
        [ResponseType(typeof(List<BestWish>))]
        public IHttpActionResult ListFor(int PatientId)
        {
            List<BestWish> bestWishes = db.BestWishes.Where(bw => bw.PatientId == PatientId).ToList();

            if(bestWishes.Count > 0)
            {
                return Ok(bestWishes);
            }
            return BadRequest();

        }
        //TODO: Add Best Wish
        [HttpPost]
        [Authorize]

        public IHttpActionResult Add([FromBody] BestWish BestWish)
        {
            var res = db.BestWishes.Add(BestWish);
            db.SaveChanges();
            if (res != null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return BadRequest();

        }

        //TODO: Update Best Wish
        [HttpPost]
        [Authorize]

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

            return StatusCode(HttpStatusCode.NoContent);

        }

        //TODO: Delete Best Wish
        [HttpPost]
        [ResponseType(typeof(BestWish))]
        [Authorize]

        public IHttpActionResult DeleteBestWish(int id)
        {
            BestWish bestWish = db.BestWishes.Find(id);
            if(bestWish == null)
            {
                return NotFound();
            }
            db.BestWishes.Remove(bestWish);
            return Ok();
        }


        private bool BestWishExists(int id)
        {
            return db.BestWishes.Count(bw => bw.BestWishId == id) > 0;
        }
    }
}
