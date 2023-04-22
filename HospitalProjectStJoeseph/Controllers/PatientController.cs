using HospitalProjectStJoeseph.Models;
using HospitalProjectStJoeseph.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace HospitalProjectStJoeseph.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false,
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44368/api/");
        }

        // GET: Patient
        public ActionResult Index()
        {
            return Redirect("/Patient/List");
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Add(Patient Patient)
        {
            GetApplicationCookie();
            string url = "PatientData/AddPatient";

            HttpContent content = Prepare(jss.Serialize(Patient));
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            return Redirect("/Patient/List");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            GetApplicationCookie();//get token credentials
            string url = "PatientData/List";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Patient> Patients = response.Content.ReadAsAsync<IEnumerable<Patient>>().Result;
            return View(Patients);
        }

        [Authorize(Roles = "Patient,Admin")]
        public ActionResult Show(int id)
        {
            GetApplicationCookie();

            if(User.IsInRole("Patient"))
            {
                string userId = User.Identity.GetUserId();
                HttpResponseMessage res = client.GetAsync("PatientData/GetPatientForUser/" + userId).Result;
                if (!res.IsSuccessStatusCode) return Redirect("/Patient/List");
                Patient _userPatient = res.Content.ReadAsAsync<Patient>().Result;
                id = _userPatient.PatientId;
            }
            
            string url = "BestWishesData/ListBestWishesForPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto Patient = response.Content.ReadAsAsync<PatientDto>().Result;

            if (response.IsSuccessStatusCode)
            {
                return View(Patient);
            }

            return Redirect("/Patient/List");

        }

        [Authorize]
        public ActionResult Update(int id)
        {
            GetApplicationCookie();
            string url = "PatientData/Find/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Patient Patient = response.Content.ReadAsAsync<Patient>().Result;

            if (response.IsSuccessStatusCode)
            {

                return View(Patient);
            }



            return Redirect("/Patient/Show/"+id);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Save(int id, Patient Patient)
        {
            GetApplicationCookie();
            string url = "PatientData/UpdatePatient/" + id;

            Patient.PatientId = id;
            HttpContent content = Prepare(jss.Serialize(Patient));
            HttpResponseMessage response = client.PostAsync(url,content).Result;
            return Redirect("/Patient/Show/"+ id);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "PatientData/DeletePatient/" + id;
            HttpResponseMessage response = client.PostAsync(url, null).Result;
            if (response.IsSuccessStatusCode)
                return Redirect("/Patient/List");
            else
                return Redirect("/Patient/Show/" + id);
        }

        public ActionResult DeleteConfirm(int id)
        {
            string url = "PatientData/FindPatientWithBestWishes/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto PatientDto = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(PatientDto);
        }


        // Assign a User Account to a Patient
        [Authorize(Roles ="Admin")]
        public ActionResult Assign(int id)
        {
            var url = "UserData/ListUsers";

            var response = client.PostAsync(url, null).Result;
            IEnumerable<UserDto> Users = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;

            url = "PatientData/Find/" + id;
            response = client.GetAsync(url).Result;
            Patient Patient = response.Content.ReadAsAsync<Patient>().Result;

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Users = Users;
                ViewBag.Patient = Patient;

                return View();
            }

            return Redirect("/Patient/List");
        }


        // REgisters a user to a patient
        public ActionResult Register(int id, string UserId)
        {
            var url = "UserData/AssignPatientToUser/"+id+"/"+UserId;
            var response = client.GetAsync(url).Result;

            return Redirect("/Patient/Show/" + id);
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