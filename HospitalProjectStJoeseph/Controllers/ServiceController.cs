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
        public ActionResult Create(Service service)
        {
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
        public ActionResult Edit(int id)
        {
            string url = "servicedata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto SelectedService = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(SelectedService);
        }

        // POST: Service/Update/5
        [HttpPost]
        public ActionResult Update(int id, Service service)
        {
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
        public ActionResult Delete(int id)
        {
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
    }
}
