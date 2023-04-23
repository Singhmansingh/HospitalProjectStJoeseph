using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalProjectStJoeseph.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Net;

namespace HospitalProjectStJoeseph.Controllers
{
    public class AppointmentsController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static AppointmentsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44368/api/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Appointments
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: Appointments/List
        public ActionResult List()
        {
            string url = "AppointmentsData/ListAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Appointment> appointments = response.Content.ReadAsAsync<IEnumerable<Appointment>>().Result;
                return View(appointments);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int id)
        {
            string url = "AppointmentsData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Appointment appointment = response.Content.ReadAsAsync<Appointment>().Result;
                return View(appointment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointments/New
        public ActionResult New()
        {
            string url = "PatientData/List";

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Patient> patients = response.Content.ReadAsAsync<IEnumerable<Patient>>().Result;

                ViewData["patients"] = new SelectList(patients, "patientId", "fullName");
            }

            return View();
        }

        // POST: Appointments/Add
        [HttpPost]
        public ActionResult Add(Appointment appointment)
        {
            string url = "AppointmentsData/AddAppointment";

            HttpContent content = Prepare(jss.Serialize(appointment));
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                int appointmentId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = appointmentId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "AppointmentsData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Appointment appointment = response.Content.ReadAsAsync<Appointment>().Result;
                return View(appointment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Appointment appointment)
        {
            string url = "AppointmentsData/UpdateAppointment/" + id;
            HttpContent content = new StringContent(jss.Serialize(appointment));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "AppointmentsData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Appointment appointment = response.Content.ReadAsAsync<Appointment>().Result;
                return View(appointment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Appointments/DeleteConfirm/5
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AppointmentsData/DeleteAppointment/" + id;
            HttpContent content = new StringContent("");
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

        // GET: Appointments/Prepare
        private HttpContent Prepare(string payload)
        {
            HttpContent content = new StringContent(payload);
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }
    }
}
