using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AlexClient.Interfaces;
using AlexClient.Models;
using AlexClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlexClient.Controllers
{
    public class UserController : Controller
    {
        private readonly ITokenService tokenService;

        public UserController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpGet]
        public IActionResult Subscription()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscription(UserSubscriptionModels subscription)
        {
            if (!ModelState.IsValid)
            {
                return View(subscription);
            }
            var jsonString = JsonSerializer.Serialize(subscription);
            var client = new HttpClient();
            var result = new HttpResponseMessage();

            result = await client.PostAsync("https://localhost:5001/api/User/CreateNewUser", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            var resultInJson = result.Content.ReadAsStringAsync().Result;

            return View(subscription);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var jsonString = JsonSerializer.Serialize(login);


            var client = new HttpClient();

            var result = await client.PostAsync("https://localhost:5001/api/User/Login", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            if(!result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.StatusCode);

                return View();
            }

            var token = await tokenService.GetTokenAsync(login);

            return RedirectToAction("Index", "Home");
        }
    }
}