using AirLine.API;
using AirLine.API.Repository;
using AirLine.Model;
using AirLine.Model.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Employee = AirLine.Model.Employee;

namespace AirLine.Web.Controllers
{
    public class EmployeeWebController : Controller
    {
        private readonly CommonFunction _commonFunction;
        public EmployeeWebController(CommonFunction commonFunction)
        {
            _commonFunction = commonFunction;
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = "http://localhost:5101/";
                //using (var client = new HttpClient())
                //{
                try
                {
                    //client.BaseAddress = new Uri(apiUrl);
                    //client.DefaultRequestHeaders.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //var stringcontent = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                    //var result = await client.PostAsync("api/Employee/SaveEmployee", stringcontent);
                    //if (result.IsSuccessStatusCode)
                    //{
                    //    var response = result.Content.ReadAsStringAsync().Result;
                    //    return RedirectToAction("GetAirlineList", "AirlinesWeb");
                    //}
                    var result = await _commonFunction.Post<Employee>(employee, "api/Employee/SaveEmployee", apiUrl, "");

                    if (result != null)
                    {
                        //return RedirectToAction("GetAirlineList", "AirlinesWeb");
                        return Json(result);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                //}
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeList()
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetAsync("api/Employee/GetEmployeeList");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        List<Employee> employeeListProps = JsonConvert.DeserializeObject<List<Employee>>(response);
                        return View(employeeListProps);
                    }
                    //var result = await _commonFunction.Get<List<AirLine.Model.Employee>>("api/Employee/GetEmployeeList", apiUrl, "");
                    //return View(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddEmployeeOrganization(int employeeId)
        {
            //ViewBag.EmployeeId = employeeId;
            EmpolyeeOrganization empolyeeOrganization = new EmpolyeeOrganization
            {
                FEmployeeId = employeeId
            };
            return View(empolyeeOrganization);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployeeOrganization(EmpolyeeOrganization empolyeeOrganization)
        {
            EmpolyeeOrganization empolyeeOrg = null;
            if (empolyeeOrganization.OrganizationName != null)
            {
                Employee employee = await PostAPICall(empolyeeOrganization.FEmployeeId);
                if (employee != null)
                {
                    empolyeeOrg = new EmpolyeeOrganization
                    {
                        FEmployeeId = empolyeeOrganization.FEmployeeId,
                        OrganizationName = empolyeeOrganization.OrganizationName,
                        FromDate = empolyeeOrganization.FromDate,
                        ToDate = empolyeeOrganization.ToDate,
                        Employee = new Employee
                        {
                            Name = employee.Name,
                            Address = employee.Address,
                            PhoneNo = employee.PhoneNo
                        }
                    };
                }
                string apiUrl = "http://localhost:5101/";
                //using (var client = new HttpClient())
                //{
                try
                {
                    //client.BaseAddress = new Uri(apiUrl);
                    //client.DefaultRequestHeaders.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //var stringcontent = new StringContent(JsonConvert.SerializeObject(empolyeeOrg), Encoding.UTF8, "application/json");
                    //var result = await client.PostAsync("api/Employee/SaveEmployeeOrganization", stringcontent);
                    //if (result.IsSuccessStatusCode)
                    //{
                    //    var response = result.Content.ReadAsStringAsync().Result;
                    //    return RedirectToAction("EmployeeList", "EmployeeWeb");
                    //}
                    if (empolyeeOrg != null)
                    {
                        var result = await _commonFunction.Post<EmpolyeeOrganization>(empolyeeOrg, "api/Employee/SaveEmployeeOrganization", apiUrl, "");

                        if (result != null)
                        {
                            return RedirectToAction("EmployeeList", "EmployeeWeb");
                        }
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
        public async Task<IActionResult> GetEmployeeOrganizationByEmpId(int employeeId)
        {
            string apiUrl = "http://localhost:5101/";
            //using (var client = new HttpClient())
            //{
            try
            {
                //client.BaseAddress = new Uri(apiUrl);
                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //var result = await client.GetAsync("api/Employee/GetEmployeeOrganizationByEmpId?employeeId=" + employeeId + "");
                //if (result.IsSuccessStatusCode)
                //{
                //    var response = result.Content.ReadAsStringAsync().Result;
                //    List<EmpolyeeOrganization> employeeListProps = JsonConvert.DeserializeObject<List<EmpolyeeOrganization>>(response);
                //    return PartialView("../Shared/_ListEmpOrganization", employeeListProps);

                //}
                var result = await _commonFunction.Get<EmpolyeeOrganization>("api/Employee/GetEmployeeOrganizationByEmpId?employeeId=" + employeeId + "", apiUrl, "");

                if (result != null)
                {
                    return PartialView("../Shared/_ListEmpOrganization", result);
                }
            }
            catch (Exception)
            {

                throw;
            }
            //}
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int? employeeId)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetAsync("api/Employee/EditEmployee?employeeId=" + employeeId + "");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        Employee employee = JsonConvert.DeserializeObject<Employee>(response);
                        return View(employee);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //var result = await _commonFunction.Get<Employee>("api/Employee/EditEmployee?employeeId=" + employeeId + "", apiUrl);
            //if (result != null)
            //{
            //    return View(result);
            //}
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    var stringcontent = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("api/Employee/UpdateEmployee", stringcontent);
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.updateEmployee = "Update Successfully!!";
                        return RedirectToAction("EmployeeList", "EmployeeWeb");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Json("ok");
        }

        [HttpPut]
        public async Task<IActionResult> DeleteEmployee(long? employeeId)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetAsync("api/Employee/DeleteEmployee?employeeId=" + employeeId + "");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        CreateAirlineModel createAirlineModel = JsonConvert.DeserializeObject<CreateAirlineModel>(response);
                        return Json(createAirlineModel);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }

        public async Task<Employee> PostAPICall(int value)
        {
            Employee employee = null;
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetAsync("api/Employee/GetEmployeeById?id=" + value + "");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        employee = JsonConvert.DeserializeObject<Employee>(response);
                        return employee;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return employee;
        }

        [HttpPut]
        public async Task<IActionResult> DeleteOrganization(long? Id)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetAsync("api/Employee/DeleteOrganization?id=" + Id + "");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        EmpolyeeOrganization empolyeeOrg = JsonConvert.DeserializeObject<EmpolyeeOrganization>(response);
                        return Json(empolyeeOrg);
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
        public async Task<IActionResult> EditOrganization(int? employeeId)
        {
            string apiUrl = "http://localhost:5101/";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var result = await client.GetAsync("api/Employee/EditOrganization?employeeId=" + employeeId + "");
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        EmpolyeeOrganization employee = JsonConvert.DeserializeObject<EmpolyeeOrganization>(response);
                        return View(employee);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrganization(EmpolyeeOrganization empolyeeOrganization)
        {
            EmpolyeeOrganization empolyeeOrganizations = null;
            Employee employee = await PostAPICall(empolyeeOrganization.FEmployeeId);
            empolyeeOrganizations = new EmpolyeeOrganization
            {
                FEmployeeId = empolyeeOrganization.FEmployeeId,
                OrganizationName = empolyeeOrganization.OrganizationName,
                FromDate = empolyeeOrganization.FromDate,
                ToDate = empolyeeOrganization.ToDate,
                Id = empolyeeOrganization.Id,
                Employee = new Employee
                {
                    Name = employee.Name,
                    Address = employee.Address,
                    PhoneNo = employee.PhoneNo,
                }
            };

            string apiUrl = "http://localhost:5101/";
            //using (var client = new HttpClient())
            //{
            try
            {
                //client.BaseAddress = new Uri(apiUrl);
                //client.DefaultRequestHeaders.Clear();
                //var stringcontent = new StringContent(JsonConvert.SerializeObject(empolyeeOrganizations), Encoding.UTF8, "application/json");
                //var result = await client.PostAsync("api/Employee/UpdateOrganization", stringcontent);
                //if (result.IsSuccessStatusCode)
                //{
                //    ViewBag.updateEmployee = "Update Successfully!!";
                //    return RedirectToAction("EmployeeList", "EmployeeWeb");
                //}
                var result = await _commonFunction.Post<EmpolyeeOrganization>(empolyeeOrganizations, "api/Employee/UpdateOrganization", apiUrl, "");
                if (result != null)
                {
                    ViewBag.updateEmployee = "Update Successfully!!";
                    return RedirectToAction("EmployeeList", "EmployeeWeb");
                }
            }
            catch (Exception)
            {
                throw;
            }
            //}
            return Json("ok");
        }

        [HttpGet]
        public async Task<IActionResult> CheckWetherOrganizationValid(string organizationName)
        {
            try
            {
                string apiUrl = "http://localhost:5101/";
                string result = await _commonFunction.GetAPI("api/Employee/CheckWetherOrganizationValid?organizationName=" + organizationName + "", apiUrl);

                if (result != null)
                {
                    return Json(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CheckWetherEmployeePhoneExist(string phoneNo)
        {
            try
            {
                string apiUrl = "http://localhost:5101/";
                string result = await _commonFunction.GetAPI("api/Employee/CheckWetherEmployeePhoneExist?phoneNo=" + phoneNo + "", apiUrl);

                if (result != null)
                {
                    return Json(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
    }
}
