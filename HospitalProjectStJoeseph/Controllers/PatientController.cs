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
        public ActionResult Add(Patient Patient)
        {
            string url = "PatientData/Add";

            HttpContent content = Prepare(jss.Serialize(Patient));
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return Redirect("/Patient/List");
        }

        public ActionResult List()
        {
            string url = "PatientData/List";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Patient> Patients = response.Content.ReadAsAsync<IEnumerable<Patient>>().Result;
            return View(Patients);
        }

        public ActionResult Show(int id)
        {
            string url = "PatientData/FindPatientWithBestWishes/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto PatientDto = response.Content.ReadAsAsync<PatientDto>().Result;

            if (response.IsSuccessStatusCode)
            {
                return View(PatientDto);
            }

            return Redirect("/Patient/List");

        }

        public ActionResult Update(int id)
        {
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
        public ActionResult Save(int id, Patient Patient)
        {
            string url = "PatientData/UpdatePatient/" + id;

            Patient.PatientId = id;
            HttpContent content = Prepare(jss.Serialize(Patient));
            HttpResponseMessage response = client.PostAsync(url,content).Result;
            return Redirect("/Patient/Show/"+ id);
        }

        public ActionResult Delete(int id)
        {
            Debug.WriteLine(id);
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

        private HttpContent Prepare(string payload)
        {
            HttpContent content = new StringContent(payload);
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }
    }
}