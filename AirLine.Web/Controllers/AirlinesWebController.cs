using AirLine.API;
using AirLine.API.Repository;
//using AirLine.API.Repository;
using AirLine.Model;
using AirLine.Model.Data;
using AirLine.Web.Models;
//using AirLine.Model.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace AirLine.Web.Controllers
{
    //[Authorize(Roles = UserRoles.Admin)]
    public class AirlinesWebController : Controller
    {
        //private readonly CommonFunction _commonFunction;

        //public AirlinesWebController(CommonFunction commonFunction)
        //{
        //    _commonFunction = commonFunction;

        //}
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly CommonFunction _commonFunction;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly LoginAuthorize _LoginAuthorize;
        


        public AirlinesWebController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, CommonFunction commonFunction,
            ApplicationDbContext applicationDbContext, LoginAuthorize LoginAuthorize)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _commonFunction = commonFunction;
            _applicationDbContext = applicationDbContext;
            _LoginAuthorize = LoginAuthorize;
        }
        public IActionResult Index()
        {
            return View();
        }

        //
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(IdentityUserProp airlineModel)
        {
            try
            {
                string apiUrl = "http://localhost:5101/";
                //var result = _commonFunction.Post<AirlineModelProp>(airlineModel, "Post", apiUrl);
                //if (result != null)
                //{

                //}
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.BaseAddress = new Uri(apiUrl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            var stringcontent = new StringContent(JsonConvert.SerializeObject(airlineModel), Encoding.UTF8, "application/json");
                            var result = await client.PostAsync("api/Airline_API/RegisterAdmin", stringcontent);
                            if (result.IsSuccessStatusCode)
                            {
                                var response = result.Content.ReadAsStringAsync().Result;
                                Response responseMessage = JsonConvert.DeserializeObject<Response>(response);
                                ViewBag.registerMessage = responseMessage.Message;
                                //return RedirectToAction("Login", "AirlinesWeb");
                                return RedirectToAction("UnApproverdStatus", "Home");
                            }
                            else if (result.IsSuccessStatusCode == false)
                            {
                                var response = result.Content.ReadAsStringAsync().Result;
                                Response responseMessage = JsonConvert.DeserializeObject<Response>(response);
                                ViewBag.registerMessage = responseMessage.Message;
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            LoginResponseMessage loginResponseMessage = new LoginResponseMessage();
            if (ModelState.IsValid)
            {
                string apiUrl = "http://localhost:5101/";
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        var stringcontent = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
                        var result = await client.PostAsync("api/Airline_API/Login", stringcontent);
                        if (result.IsSuccessStatusCode)
                        {
                            var response = result.Content.ReadAsStringAsync().Result;
                            TokenProperty tokenProp = JsonConvert.DeserializeObject<TokenProperty>(response);
                            if (tokenProp.IsApprove == true)
                            {
                                HttpContext.Session.SetString("SessionKey", tokenProp.token);
                                if (tokenProp.roles[0] == "Operator")
                                {
                                    var results = await _LoginAuthorize.LoginAsync(loginModel);
                                    if (results.StatusCode == 1)
                                        return RedirectToAction("ApproverdStatusMenu", "Home");
                                }
                                else
                                {
                                    if (tokenProp.roles?.Contains(UserRoles.Admin) ?? false)
                                    {
                                        ViewBag.statusMessage = tokenProp.roles;
                                        var results = await _LoginAuthorize.LoginAsync(loginModel);
                                        if (results.StatusCode == 1)
                                            return RedirectToAction("GetApproveList", "AirlinesWeb");
                                    }
                                }

                            }
                            else if (tokenProp.IsApprove == false)
                            {
                                HttpContext.Session.SetString("SessionKey", tokenProp.token);
                                var responses = result.Content.ReadAsStringAsync().Result;
                                loginResponseMessage = JsonConvert.DeserializeObject<LoginResponseMessage>(responses);
                                if (loginResponseMessage.success != "Login failed")
                                {
                                    if (loginResponseMessage.message != "user enters invalid credentials")
                                    {
                                        if (tokenProp.roles?.Contains(UserRoles.Operator) ?? false)
                                        {
                                            var roletype = tokenProp.roles[0];
                                            HttpContext.Session.SetString("Role", roletype);
                                            var results = await _LoginAuthorize.LoginAsync(loginModel);
                                            if(results.StatusCode == 1)
                                            return RedirectToAction("UnApproverdStatus", "Home");
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.statusMessage = loginResponseMessage.message;
                                        return RedirectToAction("Login", "AirlinesWeb");
                                    }
                                }
                            }
                        }
                        else
                        {
                            return Ok("Something is wrong");
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return View();
        }

       
        public IActionResult CreateAirline()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAirline(CreateAirlineModel createAirline)
        {
            if (ModelState.IsValid)
            {

                string apiUrl = "http://localhost:5101/";
                using (var client = new HttpClient())
                {
                    try
                    {
                        string token = HttpContext.Session.GetString("SessionKey");
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var stringcontent = new StringContent(JsonConvert.SerializeObject(createAirline), Encoding.UTF8, "application/json");
                        var result = await client.PostAsync("api/Airline_API/CreateSave", stringcontent);
                        if (result.IsSuccessStatusCode)
                        {
                            var response = result.Content.ReadAsStringAsync().Result;
                            return RedirectToAction("GetAirlineList", "AirlinesWeb");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return View();
        }

        [Authorize(Roles = UserRoles.Operator)]
        [HttpGet]
        public async Task<IActionResult> GetAirlineList()
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //var stringcontent = new StringContent(JsonConvert.SerializeObject(createAirline), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.GetAsync("api/Airline_API/GetAirlineList");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        List<CreateAirlineModel> createAirlineModel = JsonConvert.DeserializeObject<List<CreateAirlineModel>>(response);
                        return View(createAirlineModel);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }
        [Authorize(Roles = UserRoles.Operator)]
        [HttpGet]
        public async Task<IActionResult> Edit(int? employeeId)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.GetAsync("api/Airline_API/Edit?employeeId=" + employeeId + "");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        CreateAirlineModel createAirlineModel = JsonConvert.DeserializeObject<CreateAirlineModel>(response);
                        return View(createAirlineModel);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }

        [Authorize(Roles = UserRoles.Operator)]
        [HttpPost]
        public async Task<IActionResult> Update(CreateAirlineModel createAirline)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    var stringcontent = new StringContent(JsonConvert.SerializeObject(createAirline), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.PostAsync("api/Airline_API/Update", stringcontent);
                    if (result.IsSuccessStatusCode)
                    {
                        //var response = result.Content.ReadAsStringAsync().Result;
                        //CreateAirlineModel createAirlineModel = JsonConvert.DeserializeObject<CreateAirlineModel>(response);
                        ViewBag.updateAirline = "Update Successfully!!";
                        return RedirectToAction("GetAirlineList", "AirlinesWeb");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Json("ok");
        }

        [Authorize(Roles = UserRoles.Operator)]
        [HttpGet]
        public async Task<IActionResult> Details(long? employeeId)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.GetAsync("api/Airline_API/Details?employeeId=" + employeeId + "");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        CreateAirlineModel createAirlineModel = JsonConvert.DeserializeObject<CreateAirlineModel>(response);
                        return View(createAirlineModel);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }
        [Authorize(Roles = UserRoles.Operator)]
        [HttpGet]
        public async Task<IActionResult> Delete(long? employeeId)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.GetAsync("api/Airline_API/Delete?employeeId=" + employeeId + "");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        CreateAirlineModel createAirlineModel = JsonConvert.DeserializeObject<CreateAirlineModel>(response);
                        return View(createAirlineModel);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }
        [Authorize(Roles = UserRoles.Operator)]
        [HttpPost]
        public async Task<IActionResult> DeleteData(long? AirlineId)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var stringcontent = new StringContent(JsonConvert.SerializeObject(AirlineId), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.PostAsync("api/Airline_API/Delete?employeeId=" + AirlineId + "", stringcontent);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        //CreateAirlineModel createAirlineModel = JsonConvert.DeserializeObject<CreateAirlineModel>(response);
                        return RedirectToAction("GetAirlineList", "AirlinesWeb");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }

        //[HttpGet, Authorize(Roles = UserRoles.Admin + "," + UserRoles.Operator)]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetApproveList()
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //var result = await client.GetAsync("api/Airline_API/GetApproveList?emailId=" + emailId + "");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.GetAsync("api/Airline_API/GetApproveList");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        List<AspnetUserProp> createAirlineModel = JsonConvert.DeserializeObject<List<AspnetUserProp>>(response);
                        return View(createAirlineModel);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateApprove(string userName,string id)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    var stringcontent = new StringContent(JsonConvert.SerializeObject(userName), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("api/Airline_API/UpdateApprove?userName=" + userName + "", stringcontent);
                    if (result.IsSuccessStatusCode)
                    {
                        //var response = result.Content.ReadAsStringAsync().Result;
                        //CreateAirlineModel createAirlineModel = JsonConvert.DeserializeObject<CreateAirlineModel>(response);
                        return Json("Approve Successfully!!");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Json("Approve failed!!");
        }

        
        [HttpGet]
        public async Task<IActionResult> RejectApprove(string userName)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    var stringcontent = new StringContent(JsonConvert.SerializeObject(userName), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("api/Airline_API/RejectApprove?userName=" + userName + "", stringcontent);
                    if (result.IsSuccessStatusCode)
                    {
                        //var response = result.Content.ReadAsStringAsync().Result;
                        //CreateAirlineModel createAirlineModel = JsonConvert.DeserializeObject<CreateAirlineModel>(response);
                        //ViewBag.statusMessage = "Reject Successfully!!";
                        //return RedirectToAction("", "AirlinesWeb");
                        return Json("Reject Successfully!!");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Json("Reject failed!!");
        }
        
        [HttpPost]
        public async Task<IActionResult> HardCodeRegister()
        {
            try
            {
                string apiUrl = "http://localhost:5101/";
                IdentityUserProp airlineModel = new IdentityUserProp
                {
                    EmailId = "anil.keshari@kellton.com",
                    PanNo = "ANK12378",
                    Password = "Kellton@20",
                    ConfirmPassword = "Kellton@20",
                };
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.BaseAddress = new Uri(apiUrl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            var stringcontent = new StringContent(JsonConvert.SerializeObject(airlineModel), Encoding.UTF8, "application/json");
                            var result = await client.PostAsync("api/Airline_API/HardCodeRegister", stringcontent);
                            if (result.IsSuccessStatusCode)
                            {
                                var response = result.Content.ReadAsStringAsync().Result;
                                Response responseMessage = JsonConvert.DeserializeObject<Response>(response);
                                ViewBag.registerMessage = responseMessage.Message;
                                return RedirectToAction("Login", "AirlinesWeb");
                            }
                            else if (result.IsSuccessStatusCode == false)
                            {
                                var response = result.Content.ReadAsStringAsync().Result;
                                Response responseMessage = JsonConvert.DeserializeObject<Response>(response);
                                ViewBag.registerMessage = responseMessage.Message;
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> CkeckAirLineUserName(string airlineName)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    var stringcontent = new StringContent(JsonConvert.SerializeObject(airlineName), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.PostAsync("api/Airline_API/CheckAirLineName?airLineName=" + airlineName + "", stringcontent);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        return Json(response);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> CheckUserNameExit(string userName)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    string token = HttpContext.Session.GetString("SessionKey");
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    var stringcontent = new StringContent(JsonConvert.SerializeObject(userName), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.PostAsync("api/Airline_API/CheckUserNameExit?userName=" + userName + "", stringcontent);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        return Json(response);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }
    }
}
