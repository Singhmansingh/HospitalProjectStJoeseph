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
            string url = "PatientData/List";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Patient> Patients = response.Content.ReadAsAsync<IEnumerable<Patient>>().Result;

            return View(Patients);
        }

        [HttpPost]
        public ActionResult Add(BestWish BestWish)
        {
            BestWish.BestWishSendDate = DateTime.Now;
            BestWish.BestWishIsRead = false;

            string url = "BestWishesData/AddBestWish";
            HttpContent content = Prepare(jss.Serialize(BestWish));

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
                return Redirect("/BestWishes/List");
            else return Redirect("/BestWishes/New");
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

        public ActionResult Update(int id)
        {
            string url = "BestWishesData/FindBestWish/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BestWish BestWish = response.Content.ReadAsAsync<BestWish>().Result;

            url = "PatientData/List";
            response = client.GetAsync(url).Result;
            IEnumerable<Patient> Patients = response.Content.ReadAsAsync<IEnumerable<Patient>>().Result;


            ViewBag.Patients = Patients;

            return View(BestWish);
        }

        [HttpPost]
        public ActionResult Save(int id, BestWish BestWish)
        {
            BestWish.BestWishSendDate = DateTime.Now;
            BestWish.BestWishIsRead = false;
            BestWish.BestWishId = id;

            string url = "BestWishesData/UpdateBestWish/" + id;
            HttpContent content = Prepare(jss.Serialize(BestWish));


            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(response.StatusCode);

            return Redirect("/BestWishes/Show/"+id);
        }

        public ActionResult DeleteConfirm(int id)
        {
            string url = "BestWishesData/FindBestWish/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BestWish BestWish = response.Content.ReadAsAsync<BestWish>().Result;

            return View(BestWish);
        }

        [HttpPost]
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

        private HttpContent Prepare(string payload)
        {
            HttpContent content = new StringContent(payload);
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }
    }
}