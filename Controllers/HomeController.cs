using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Mvc;
using System.Net.Http;
using AddWebTask.Models;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace AddWebTask.Controllers
{
    public class HomeController : Controller
    {
        public System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        public System.Web.Mvc.ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("addUser")]
        public System.Web.Mvc.ActionResult addUser()
        {
            if (Request.Form.Keys.Count > 0)
            {
                Users addedUser = new Users();
                addedUser.Fname = Request.Form.Keys[0];
                addedUser.Lname = Request.Form.Keys[1];
                string apiUrl = "https://localhost:44322/api/AddWebApi";
                HttpClient client = new HttpClient();
                
                HttpResponseMessage response = client.PostAsJsonAsync<String>(apiUrl,JsonConvert.SerializeObject(addedUser) ).Result;
                bool response1b = false;
                if (TempData.Peek("sessionExp") != null)
                {
                    List<Exprience> expList = JsonConvert.DeserializeObject<List<Exprience>>(TempData.Peek("sessionExp").ToString());
                    
                    if (expList.Count > 0)
                    {
                        apiUrl = "https://localhost:44322/api/AddWebApi/addExperience";
                        HttpClient client1 = new HttpClient();
                        HttpResponseMessage response1 = client.PostAsJsonAsync<String>(apiUrl, JsonConvert.SerializeObject(expList).ToString()).Result;
                        if (response1.IsSuccessStatusCode)
                        {
                            response1b = true;
                        }
                    }
                }
                
                
                
                if (response.IsSuccessStatusCode || response1b)
                {
                    return View("About");
                }

            }
            return View("About");
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("addExperience")]
       

        public Microsoft.AspNetCore.Mvc.NoContentResult addExperience()            
        {
            if (Request.Form.Keys.Count > 0)
            {
                Exprience exp = new Exprience();
                exp.position = Request.Form.Keys[0];
                exp.company = Request.Form.Keys[1];
                
                List<Exprience> expList = new List<Exprience>();
                
                if (TempData.Peek("sessionExp") == null)
                {
                    expList.Add(exp);
                    TempData["sessionExp"] = JsonConvert.SerializeObject(expList);
                }
                else
                {
                    expList = JsonConvert.DeserializeObject<List<Exprience>>(TempData.Peek("sessionExp").ToString());
                    expList.Add(exp);
                    TempData["sessionExp"] = JsonConvert.SerializeObject(expList);
                }
            }
            
            
            
            //expList.Add(exp);
          //  TempData["tempExp"] = JsonConvert.SerializeObject(expList);
            return new NoContentResult();
        }
        public System.Web.Mvc.ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            string apiUrl = "https://localhost:44322/api/AddWebApi";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            List<Users> users = new List<Users>();
            if (response.IsSuccessStatusCode)   
            {
                users = response.Content.ReadAsAsync<List<Users>>().Result;
              
            }
            ViewBag.Message = "Your contact page.";
            ViewBag.users = users;
            return View();
        }

    }
}