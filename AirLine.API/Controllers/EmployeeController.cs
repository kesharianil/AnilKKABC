using AirLine.Model.Data;
using AirLine.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AirLine.API.Repository;
using Microsoft.EntityFrameworkCore;
using Employee = AirLine.Model.Employee;

namespace AirLine.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public EmployeeController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [HttpPost]
        public async Task<string> SaveEmployee(Employee employee)
        {
            try
            {
                if (_applicationDbContext != null)
                {
                    await _applicationDbContext.Tbl_Employee.AddAsync(employee);
                    await _applicationDbContext.SaveChangesAsync();

                    return "Save Successfull!";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return "Create failed!";
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeList()
        {
            try
            {
                var employeeList = await _applicationDbContext.Tbl_Employee.ToListAsync();
                if (employeeList == null)
                {
                    return NotFound();
                }
                return Ok(employeeList);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<string> SaveEmployeeOrganization(EmpolyeeOrganization empolyeeOrganization)
        {
            try
            {
                if (empolyeeOrganization != null)
                {
                    EmpolyeeOrganization empolyeeOrganizationss = new EmpolyeeOrganization
                    {
                        OrganizationName = empolyeeOrganization.OrganizationName,
                        FEmployeeId = empolyeeOrganization.FEmployeeId,
                        FromDate = empolyeeOrganization.FromDate,
                        ToDate = empolyeeOrganization.ToDate
                    };
                    await _applicationDbContext.Tbl_EmpOrganization.AddAsync(empolyeeOrganizationss);
                    await _applicationDbContext.SaveChangesAsync();

                    return "Save Successfull!";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return "Create failed!";
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeOrganizationByEmpId(int employeeId)
        {
            try
            {
                var employeeList = await _applicationDbContext.Tbl_EmpOrganization.Where(x => x.FEmployeeId == employeeId).ToListAsync();
                if (employeeList == null)
                {
                    return NotFound();
                }
                return Ok(employeeList);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int? employeeId)
        {
            if (employeeId == null)
            {
                return Ok();
            }
            else
            {
                Employee employee = await _applicationDbContext.Tbl_Employee.Where(x => x.EmployeeId == employeeId).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(Employee employees)
        {
            //Employee employee = new Employee();
            if (employees == null)
            {
                return Ok("Update failed!");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _applicationDbContext.Tbl_Employee.Update(employees);
                }
                await _applicationDbContext.SaveChangesAsync();

                return Ok("Update Successfull!!");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(int? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _applicationDbContext.Tbl_Employee.FirstOrDefaultAsync(m => m.EmployeeId == employeeId);
            if (employee != null)
            {
                var employeeOrganization = await _applicationDbContext.Tbl_EmpOrganization.FirstOrDefaultAsync(m => m.FEmployeeId == employeeId);
                _applicationDbContext.Remove(employee);
                if (employeeOrganization != null)
                {
                    _applicationDbContext.Remove(employeeOrganization);
                }
            }
            await _applicationDbContext.SaveChangesAsync();

            return Ok(employee);
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employeeList = await _applicationDbContext.Tbl_Employee.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
                if (employeeList == null)
                {
                    return NotFound();
                }
                return Ok(employeeList);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpGet]
        public async Task<IActionResult> DeleteOrganization(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            EmpolyeeOrganization employeeOrganization = await _applicationDbContext.Tbl_EmpOrganization.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (employeeOrganization != null)
            {
                _applicationDbContext.Remove(employeeOrganization);
            }
            await _applicationDbContext.SaveChangesAsync();

            return Ok(employeeOrganization);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrganization(int? employeeId)
        {
            if (employeeId == null)
            {
                return Ok();
            }
            else
            {
                EmpolyeeOrganization employee = await _applicationDbContext.Tbl_EmpOrganization.Where(x => x.Id == employeeId).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrganization(EmpolyeeOrganization empolyeeOrganization)
        {
            if (empolyeeOrganization == null)
            {
                return Ok("Update failed!");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    EmpolyeeOrganization data = await _applicationDbContext.Tbl_EmpOrganization.Where(x => x.FEmployeeId == empolyeeOrganization.FEmployeeId && x.Id == empolyeeOrganization.Id).FirstOrDefaultAsync();
                    data.OrganizationName = empolyeeOrganization.OrganizationName;
                    data.FromDate = empolyeeOrganization.FromDate;
                    data.ToDate = empolyeeOrganization.ToDate;
                    _applicationDbContext.Entry(data).State = EntityState.Modified;
                }

                await _applicationDbContext.SaveChangesAsync();

                return Ok("Update Successfull!!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckWetherOrganizationValid(string organizationName)
        {
            EmpolyeeOrganization organization = new EmpolyeeOrganization();
            if (organizationName == null)
            {
                return Ok("Organization Name is blank!");
            }
            else
            {
                organization = await _applicationDbContext.Tbl_EmpOrganization.Where(x => x.OrganizationName == organizationName).FirstOrDefaultAsync();
                if (organization != null)
                {
                    return Ok("Organization is already exists!!");
                }
                else
                {
                    return Ok("Organization is valid!!");
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> CheckWetherEmployeePhoneExist(string phoneNo)
        {
            Employee employee = new Employee();
            if (phoneNo == null)
            {
                return Ok(CoomonStaticData.PhoneNoExits);
            }
            else
            {
                employee = await _applicationDbContext.Tbl_Employee.Where(x => x.PhoneNo == phoneNo).FirstOrDefaultAsync();
                if (employee != null)
                {
                    return Ok(CoomonStaticData.PhoneNotExits);
                }
                else
                {
                    return Ok(CoomonStaticData.PhoneValid);
                }
            }
        }

    }
}
