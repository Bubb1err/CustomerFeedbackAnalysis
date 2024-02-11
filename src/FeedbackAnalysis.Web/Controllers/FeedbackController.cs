using FeedbackAnalysis.Web.Models.Feedback;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FeedbackAnalysis.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IConfiguration _configuration;

        public FeedbackController(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetFeedbackInput()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostFeedbackInput(string feedback)
        {
            string token = Request.Cookies["JwtToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (string.IsNullOrWhiteSpace(feedback)) return View();
            HttpResponseMessage response;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["FeedbackAPI:ApiHost"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(feedback);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                response = await client.PostAsync("api/Feedback", content);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(GetFeedback));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFeedback()
        {
            string token = Request.Cookies["JwtToken"];

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_configuration["FeedbackAPI:ApiHost"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(_configuration["FeedbackAPI:Feedback"]);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<IEnumerable<FeedbackViewModel>>(responseContent);
                return View(responseObject);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }
        }
    }
}
