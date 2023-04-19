using HospitalProjectStJoeseph.Models;
using HospitalProjectStJoeseph.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalProjectStJoeseph.Controllers
{
    public class ClinicController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static ClinicController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44368/api/");
        }

        // GET: Clinic/List
        public ActionResult List()
        {
            string url = "clinicdata/listclinics";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ClinicDto> clinics = response.Content.ReadAsAsync<IEnumerable<ClinicDto>>().Result;

          return View(clinics);
        }

        // GET: Clinic/Details/5
        public ActionResult Details(int id)
        {
            DetailsClinic ViewModel = new DetailsClinic();

            string url = "clinicdata/findclinic/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ClinicDto SelectedClinic = response.Content.ReadAsAsync<ClinicDto>().Result;

            ViewModel.SelectedClinic = SelectedClinic;

            //Show associated services with this clinic
            url = "servicedata/listservicesforclinic/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> ProvidedService = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;

            ViewModel.ProvidedServices = ProvidedService;

            //show unassociated serives with this clinic
            url = "servicedata/listservicesnotforclinic/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ServiceDto> UnprovidedServices = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;

            ViewModel.UnprovidedServices = UnprovidedServices; 
                       
            return View(ViewModel);
        }

        //POST: Clinic/Associate/{clinicid}
        [HttpPost]
        public ActionResult Associate(int id, int ServiceId)
        {
            string url = "clinicdata/associateclinicwithservice/" + id + "/" + ServiceId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //GET: Clinic/UnAssociate/{id}?ServiceId={ServiceId}
        [HttpGet]
        public ActionResult UnAssociate(int id, int ServiceId)
        {
            string url = "clinicdata/unassociateclinicwithservice/" + id + "/" + ServiceId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Clinic/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Clinic/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Clinic clinic)
        {
            GetApplicationCookie();
            string url = "clinicdata/addclinic";
            string jsonpayload = jss.Serialize(clinic);

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

        // GET: Clinic/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            string url = "clinicdata/findclinic/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ClinicDto SelectedClinic = response.Content.ReadAsAsync<ClinicDto>().Result;
            return View(SelectedClinic);
        }

        // POST: Clinic/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Clinic clinic)
        {
            GetApplicationCookie();
            string url = "clinicdata/updateclinic/" + id;
            string jsonpayload = jss.Serialize(clinic);
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

        // GET: Clinic/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "clinicdata/findclinic/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ClinicDto SelectedClinic = response.Content.ReadAsAsync<ClinicDto>().Result;

            return View(SelectedClinic);
        }


        // POST: Clinic/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "clinicdata/deleteclinic/" + id;
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
