﻿using HospitalProjectStJoeseph.Models;
using HospitalProjectStJoeseph.Models.ViewModels;
using System;
using System.Collections.Generic;
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
            client.BaseAddress = new Uri("https://localhost:44368/api/clinicdata/");
        }

        // GET: Clinic/List
        public ActionResult List()
        {
            string url = "listclinics";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ClinicDto> clinics = response.Content.ReadAsAsync<IEnumerable<ClinicDto>>().Result;

          return View(clinics);
        }

        // GET: Clinic/Details/5
        public ActionResult Details(int id)
        {
            DetailsClinic ViewModel = new DetailsClinic();
            string url = "findclinic/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ClinicDto SelectedClinic = response.Content.ReadAsAsync<ClinicDto>().Result;

            ViewModel.SelectedClinic = SelectedClinic;
                       
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
        public ActionResult Create(Clinic clinic)
        {
            string url = "addclinic";
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
        public ActionResult Edit(int id)
        {

            string url = "findclinic/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ClinicDto SelectedClinic = response.Content.ReadAsAsync<ClinicDto>().Result;
            return View(SelectedClinic);
        }

        // POST: Clinic/Update/5
        [HttpPost]
        public ActionResult Update(int id, Clinic clinic)
        {
            string url = "updateclinic/" + id;
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
        public ActionResult Delete(int id)
        {
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
    }
}
