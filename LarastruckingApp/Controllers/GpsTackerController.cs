using LarastruckingApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    public class GpsTackerController : Controller
    {
        // GET: GpsTacker
        string Baseurl = "https://qws.quartix.net/";
        public async Task<ActionResult> LoadData()
        {
            List<GpsTracker> EmpInfo = new List<GpsTracker>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("v2/api/auth");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    EmpInfo = JsonConvert.DeserializeObject<List<GpsTracker>>(EmpResponse);
                }
                //returning the employee list to view
                return View(EmpInfo);
            }
        }


    }
}