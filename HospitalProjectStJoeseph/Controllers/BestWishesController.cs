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

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
    }
}