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
        [Route("api/BestWishesData/ListAllBestWishes")]
        public IHttpActionResult ListAllBestWishes()
        {
            List<BestWish> bestWishes = db.BestWishes.ToList();

            return Ok(bestWishes);
        }

        //TODO: Find Best Wish by ID
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


        //TODO: Add Best Wish
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

        //TODO: Update Best Wish
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

            return StatusCode(HttpStatusCode.NoContent);

        }

        //TODO: Delete Best Wish
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


        private bool BestWishExists(int id)
        {
            return db.BestWishes.Count(bw => bw.BestWishId == id) > 0;
        }
    }
}
