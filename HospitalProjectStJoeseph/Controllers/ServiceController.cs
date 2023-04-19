using HospitalProjectStJoeseph.Models;
using HospitalProjectStJoeseph.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalProjectStJoeseph.Controllers
{
    public class ServiceController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ServiceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44368/api/");
        }
        // GET: Service
        public ActionResult List()
        {
            string url = "servicedata/listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;

            return View(services);
        }


        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            DetailsService ViewModel = new DetailsService();
            string url = "servicedata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto SelectedService = response.Content.ReadAsAsync<ServiceDto>().Result;

            ViewModel.SelectedService = SelectedService;

            url = "clinicdata/listclinicsforservice/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ClinicDto> ProvidedClinic = response.Content.ReadAsAsync<IEnumerable<ClinicDto>>().Result;

            ViewModel.ProvidedClinic = ProvidedClinic;
                      
            return View(ViewModel);
        }

        //GET: service/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Service/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Service service)
        {
            GetApplicationCookie();
            string url = "servicedata/addservice";
            string jsonpayload = jss.Serialize(service);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Service/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            string url = "servicedata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto SelectedService = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(SelectedService);
        }

        // POST: Service/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Service service)
        {
            GetApplicationCookie();
            string url = "servicedata/updateservice/" + id;
            string jsonpayload = jss.Serialize(service);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    

        // GET: Service/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "servicedata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto SelectedService = response.Content.ReadAsAsync<ServiceDto>().Result;
            
            return View(SelectedService);
        }

        // POST: Service/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "servicedata/deleteservice/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
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
