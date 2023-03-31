using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalProjectStJoeseph.Models;
using System.Web.Script.Serialization;

namespace HospitalProjectStJoeseph.Controllers
{
    public class TestController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TestController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44368/api/");
        }


        public ActionResult List()
        {
            //e.g. curl https://localhost:44368/api/testdata/listtests

            string url = "testdata/listtests";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine(response.StatusCode);
            Debug.WriteLine(response.Content);

            IEnumerable<TestDto> Tests = response.Content.ReadAsAsync<IEnumerable<TestDto>>().Result;

            Debug.WriteLine(Tests.Count());

            return View(Tests);
        }


        public ActionResult Error()
        {

            return View();
        }

        // GET: Test/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Test/Create
        [HttpPost]
        public ActionResult Create(Test Test)
        {


            string url = "testdata/addtest";

            string jsonpayload = jss.Serialize(Test);
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

        // GET: Test/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "testdata/findtest/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TestDto TestDto = response.Content.ReadAsAsync<TestDto>().Result;
            return View(TestDto);
        }

        // POST: Test/Update/5
        [HttpPost]
        public ActionResult Update(int id, Test Test)
        {
            // update a specific Test
            string url = "testdata/updatetest/" + id;
            string jsonpayload = jss.Serialize(Test);

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

        // GET: Test/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            // confirmation of a Test deletion
            string url = "testdata/findtest/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TestDto TestDto = response.Content.ReadAsAsync<TestDto>().Result;
            return View(TestDto);
        }

        // POST: Test/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "testdata/deletetest/" + id;
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