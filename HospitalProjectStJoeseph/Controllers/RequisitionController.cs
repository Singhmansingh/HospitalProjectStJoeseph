using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Net.Http;
using HospitalProjectStJoeseph.Models;
using HospitalProjectStJoeseph.Models.ViewModels;
using System.Web.Script.Serialization;

namespace HospitalProjectStJoeseph.Controllers
{
    // set up CRUD functions for Requisition  
    public class RequisitionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RequisitionController()
        {
            // set up the base url address
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44368/api/");
        }

        public ActionResult List()
        {

            string url = "Requisitiondata/listRequisitions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Requisition> Requisitions = response.Content.ReadAsAsync<IEnumerable<Requisition>>().Result;

            return View(Requisitions);
        }

        public ActionResult Details(int id)
        {
            //communicate with Requisition Data api to retrieve one specific Requisition
            //e.g. curl https://localhost:44387/api/requisitiondata/findrequisition/{id}
            // create a ViewModel to show the relationship between Requisitions and Patients
            DetailsRequisition ViewModel = new DetailsRequisition();


            string url = "requisitiondata/findrequisition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);
            RequisitionDto SelectedRequisition = response.Content.ReadAsAsync<RequisitionDto>().Result;

            ViewModel.SelectedRequisition = SelectedRequisition;

            url = "patientdata/listpatientsforrequisition/" + id;
            response = client.GetAsync(url).Result;
            PatientDto RelatedPatient = response.Content.ReadAsAsync<PatientDto>().Result;

            ViewModel.RelatedPatient = RelatedPatient;

            return View(ViewModel);
        }


        public ActionResult Error()
        {
            // error view page
            return View();
        }



        public ActionResult New()
        {
            //show a list of tests

            string url = "testdata/listtest";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Test> TestOptions = response.Content.ReadAsAsync<IEnumerable<Test>>().Result;

            return View(TestOptions);
        }

        // POST: Requisition/Create
        [HttpPost]
        public ActionResult Create(Requisition Requisition)
        {

            //add a new Requisition
            string url = "requisitiondata/addrequisition";

            // json
            string jsonpayload = jss.Serialize(Requisition);
            //Debug.WriteLine(jsonpayload);

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


        [HttpPost]
        public ActionResult Update(int id, Requisition Requisition)
        {

            string url = "requisitiondata/updaterequisition/" + id;
            string jsonpayload = jss.Serialize(Requisition);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            // Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        public ActionResult DeleteConfirm(int id)
        {
            string url = "requisitiondata/findrequisition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RequisitionDto RequisitionDto = response.Content.ReadAsAsync<RequisitionDto>().Result;
            return View(RequisitionDto);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "requisitiondata/deleterequisition/" + id;
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