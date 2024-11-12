using DemoNetCoreProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DemoNetCoreProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        public EmployeeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET: EmployeeController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            //model.Username = "admin";
            //model.Password = "abc";
            //var apiUrl= "https://localhost:7173/api/";
            var jsonModel = JsonConvert.SerializeObject(model);

            // Create a StringContent object to send the JSON data
            var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7173/api/EmployeeAPI/GenerateToken", content);
            if (response.IsSuccessStatusCode)
            {
                // Read and deserialize the response content to the model
                var token = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<LoginModel>(token);
                ViewBag.Token = token;
                // Return the employee data to the view
                return View();
            }
            return Unauthorized();
        }

        [HttpPost]// GET: EmployeeController/Details/5
        public async Task<ActionResult> Details()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");  // Redirect to login if no token
            }

            // Set the Bearer token in the Authorization header of HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //var apiUrl = "https://localhost:7173/api/";
            var response = await _httpClient.GetAsync("https://localhost:7173/api/EmployeeAPI/getEmployee");
            if (response.IsSuccessStatusCode)
            {
                // Read and deserialize the response content to the model
                var content = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<LoginModel>(content);
                ViewBag.Employee = employee;
                // Return the employee data to the view
                return View();
            }
            return View();
        }


        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
