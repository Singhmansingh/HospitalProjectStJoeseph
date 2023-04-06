using HospitalProjectStJoeseph.Models;
using Microsoft.Ajax.Utilities;
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
    public class PhysiciansController : Controller
    {
        // GET: Physician
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PhysiciansController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false,
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44368/api/");
        }

        // GET: Physician
        public ActionResult Index()
        {
            return Redirect("/Physicians/List");
        }

        public ActionResult New()
        {
            return View();
        }
        [Route("/Physicians/Add/")]
        [HttpPost]
        public ActionResult Add(Physician physician)
        {
            string url = "PhysicianData/AddPhysician";

            HttpContent content = Prepare(jss.Serialize(physician));
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Physicians/List");
        }

        public ActionResult List()
        {
            string url = "PhysicianData/ListPhysicians";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Physician> physicians = response.Content.ReadAsAsync<IEnumerable<Physician>>().Result;
            return View(physicians);
        }

        public ActionResult Show(int id)
        {
            string url = "PhysicianData/FindPhysician/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Physician physician = response.Content.ReadAsAsync<Physician>().Result;

            if (response.IsSuccessStatusCode)
            {
                return View(physician);
            }

            return Redirect("/Physicians/List");

        }
        public ActionResult Update(int id)
        {
            string url = "PhysicianData/FindPhysician/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Physician physician = response.Content.ReadAsAsync<Physician>().Result;

            if (response.IsSuccessStatusCode)
            {
                return View(physician);
            }

            return Redirect("/Physicians/Show/" + id);
        }
        [Route("/Physicians/Save/")]
        [HttpPost]
        public ActionResult Save(int id, Physician physician)
        {
            string url = "PhysicianData/UpdatePhysician/" + id;

            physician.physician_id = id;
            HttpContent content = Prepare(jss.Serialize(physician));
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Physicians/Show/" + id);
        }

        public ActionResult Delete(int id)
        {
            Debug.WriteLine(id);
            string url = "PhysicianData/DeletePhysician/" + id;
            HttpResponseMessage response = client.PostAsync(url, null).Result;
            Debug.WriteLine(url);
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
                return Redirect("/Physicians/List");
            else
                return Redirect("/Physicians/Show/" + id);
        }
        public ActionResult DeleteConfirm(int id)
        {
            string url = "PhysicianData/FindPhysician/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Physician physician = response.Content.ReadAsAsync<Physician>().Result;

            if (response.IsSuccessStatusCode)
            {
                return View(physician);
            }

            return Redirect("/Physicians/List");
        }

        private HttpContent Prepare(string payload)
        {
            HttpContent content = new StringContent(payload);
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }
    }
}
