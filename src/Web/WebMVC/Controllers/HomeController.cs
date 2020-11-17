using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Test();
            return RedirectToAction(nameof(MailBoxController.Inbox), "MailBox");
        }

        private static readonly HttpStatusCode[] validStatuses = new[]
        {
            HttpStatusCode.OK,
            HttpStatusCode.Found,
        };

        private void Test()
        {
            var mode = "http";
            var server = "192.168.8.36";
            var userName = "cpsystemadmin";
            var password = "itsysdev*1";
            //This is a example request and should be replaced with a real request.
            var request = "[{\"EmployeeNumber\":\"123\",\"Details\":{\"FirstName\":\"Jon\",\"LastName\":\"doe\",\"EmploymentStartDate\":\"2020-03-26\"}}]";
            //This is a call to get a autentication token;
            var getTokenResponse = new RestCall
            {
                URL = $"{mode}://{server}/Rainmaker.Integration.facade/Token",
                Method = HttpMethod.Get,
                UserName = userName,
                Password = password,
            }.ExecuteAsync().Result;
            if (string.IsNullOrWhiteSpace(getTokenResponse.ErrorMessage) && validStatuses.Contains(getTokenResponse.Status))
            {
                var payload = JObject.Parse(getTokenResponse.Response);
                //This is the actual call to the SaveCrews
                var result = new RestCall
                {
                    URL = $"{mode}://{server}/Rainmaker.Integration.facade/OperationsCrew/SaveCrews",
                    Method = HttpMethod.Post,
                    Request = request,
                    AuthToken = payload.Value<string>("Value"),
                }.ExecuteAsync().Result;
                if (string.IsNullOrWhiteSpace(result.ErrorMessage) && result.Status == HttpStatusCode.OK)
                    Console.WriteLine(result.Response);
            }
        }
    }

    public static class RestServiceHelper
    {
        private static readonly HttpClientHandler httpClientHandler = new HttpClientHandler { AllowAutoRedirect = false, };
        private static readonly HttpClient Client = new HttpClient(httpClientHandler);

        public static async Task<ResultCall> ExecuteAsync(this RestCall request, string accept = "application/json", string mediaType = "application/json")
        {
            var result = new ResultCall();
            try
            {
                using (var message = new HttpRequestMessage(request.Method, request.URL))
                {
                    message.Headers.Accept.Clear();
                    message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
                    if (!string.IsNullOrWhiteSpace(request.Cookies))
                        message.Headers.Add("cookie", request.Cookies);
                    if (!string.IsNullOrWhiteSpace(request.AuthToken))
                        message.SetBearerToken(request.AuthToken);
                    if (!string.IsNullOrWhiteSpace(request.UserName) && !string.IsNullOrWhiteSpace(request.Password))
                        message.SetBasicAuthentication(request.UserName, request.Password);
                    if (request.Method == HttpMethod.Post && !string.IsNullOrWhiteSpace(request.Request))
                        message.Content = new StringContent(request.Request, Encoding.ASCII, mediaType);
                    var responseMessage = await Client.SendAsync(message);
                    result.Response = await responseMessage.Content.ReadAsStringAsync();
                    result.Status = responseMessage.StatusCode;
                    var cookie = responseMessage.Headers.SingleOrDefault(c => c.Key == "Set-Cookie").Value;
                    if (cookie != null)
                        result.Cookies = string.Join(";", cookie);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }

    public class RestCall
    {
        public string URL { get; set; }
        public HttpMethod Method { get; set; }
        public string Request { get; set; }
        public string Cookies { get; set; }
        public string AuthToken { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ResultCall
    {
        public string Response { get; set; }
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; }
        public string Cookies { get; set; }
    }
}