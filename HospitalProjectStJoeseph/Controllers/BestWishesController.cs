using HospitalProjectStJoeseph.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalProjectStJoeseph.Controllers
{
    public class BestWishesController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BestWishesController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false,
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44368/api/");
        }

        // GET: BestWishes
        public ActionResult Index()
        {
            return Redirect("/BestWishes/List");
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult List()
        {
            string url = "BestWishesData/ListAllBestWishes";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<BestWish> BestWishes = response.Content.ReadAsAsync<IEnumerable<BestWish>>().Result;
            return View(BestWishes);
        }

        public ActionResult Show(int id)
        {
            string url = "BestWishesData/FindBestWish/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BestWish BestWish = response.Content.ReadAsAsync<BestWish>().Result;

            if(response.IsSuccessStatusCode)
            {
                return View(BestWish);
            }

            return Redirect("/BestWishes/List");

        }

        public ActionResult Delete(int id)
        {
            Debug.WriteLine(id);
            string url = "BestWishesData/DeleteBestWish/"+id;
            HttpResponseMessage response = client.PostAsync(url,null).Result;
            if (response.IsSuccessStatusCode)
                return Redirect("/BestWishes/List");
            else 
                return Redirect("/BestWishes/Show/"+id);
        }
    }
}