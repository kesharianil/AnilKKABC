using AirLine.API.Repository;
using AirLine.Model;
using AirLine.Model.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Xml.Linq;

namespace AirLine.API.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Route("[controller]")]
    [ApiController]
    public class Airline_APIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly CommonFunction _commonFunction;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IAirline _airline;
        private readonly IMailService _mailService;
        private readonly ILoggerManager _logger;


        public Airline_APIController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, CommonFunction commonFunction,
            ApplicationDbContext applicationDbContext, IAirline airline, IMailService mailService, ILoggerManager logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _commonFunction = commonFunction;
            _applicationDbContext = applicationDbContext;
            _airline = airline;
            _mailService = mailService;
            _logger = logger;
        }

        // role based create 
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin([FromBody] IdentityUserProp model)
        {
            bool isSend = false;
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.EmailId);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                ApplicationUser user = new()
                {
                    Email = model.EmailId,
                    PanNo = model.PanNo,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    UserName = model.EmailId
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });


                //create role if role not exist in role table
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                //create role if role not exist in role table
                if (!await _roleManager.RoleExistsAsync(UserRoles.Operator))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Operator));
                }
                var userRoles = await _userManager.GetRolesAsync(user);
                //assign role to user if not assigned
                if (!await _userManager.IsInRoleAsync(user, UserRoles.Operator) || (userRoles?.Contains(UserRoles.Operator) ?? false))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Operator);
                }


                isSend = _mailService.EmailSend(model.EmailId);
                return Ok(new Response { Status = "Success", Message = "User created successfully!", isMailSend = isSend });
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(new Response { Status = "failed", Message = "User created failed!", isMailSend = isSend });
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInfo("Here is info message from the controller.");
            var user = await _userManager.FindByEmailAsync(model.EmailId);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                //if user is admin then we are allowing the login.
                //if (userRoles?.Contains(UserRoles.Admin) ?? false)
                if (user.IsApprove == true)
                {
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = _commonFunction.GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        roles = userRoles,
                        IsApprove = true
                    });
                }
                else
                {
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var token = _commonFunction.GetToken(authClaims);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        roles = userRoles,
                        success = true,
                        status = "Login Succesfully!!",
                        message = "Please Appove user for Admin",
                        IsApprove = false
                    });
                }
            }
            return Ok(new
            {
                success = false,
                status = "Login failed",
                message = "user enters invalid credentials"
            });
        }

        [Authorize(Roles = "Operator")]
        [HttpPost]
        public async Task<string> CreateSave(CreateAirlineModel post)
        {
            try
            {
                if (_applicationDbContext != null)
                {
                    await _applicationDbContext.Tbl_AirLines.AddAsync(post);
                    await _applicationDbContext.SaveChangesAsync();

                    return "Create Successfull!";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return "Create failed!";
        }


        [Authorize(Roles = "Operator")]
        [HttpGet]
        public async Task<IActionResult> GetAirlineList()
        {
            try
            {
                var airlines = await _airline.GetAirlineList();
                if (airlines == null)
                {
                    return NotFound();
                }
                return Ok(airlines);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [Authorize(Roles = "Operator")]
        [HttpGet]
        public async Task<IActionResult> Edit(long? employeeId)
        {
            if (employeeId == null)
            {
                return Ok();
            }
            else
            {
                CreateAirlineModel employee = await _applicationDbContext.Tbl_AirLines.Where(x => x.AirlineId.Equals(employeeId)).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
        }

        [Authorize(Roles = "Operator")]
        [HttpPost]
        public async Task<IActionResult> Update(CreateAirlineModel updateAirline)
        {
            CreateAirlineModel createAirline = new CreateAirlineModel();
            if (updateAirline == null)
            {
                return Ok("Update failed!");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //createAirline.AirlineName = updateAirline.AirlineName;
                    //createAirline.FromCity = updateAirline.FromCity;
                    //createAirline.ToCity = updateAirline.ToCity;
                    //createAirline.Fare = updateAirline.Fare;
                    _applicationDbContext.Tbl_AirLines.Update(updateAirline);
                }
                await _applicationDbContext.SaveChangesAsync();

                return Ok("Update Successfull!!");
            }
        }

        [Authorize(Roles = "Operator")]
        [HttpGet]
        public async Task<IActionResult> Details(long? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _applicationDbContext.Tbl_AirLines.FirstOrDefaultAsync(m => m.AirlineId == employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [Authorize(Roles = "Operator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _applicationDbContext.Tbl_AirLines.FirstOrDefaultAsync(m => m.AirlineId == employeeId);

            return Ok(employee);
        }

        [Authorize(Roles = "Operator")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long employeeId)
        {
            var employee = await _applicationDbContext.Tbl_AirLines.FindAsync(employeeId);
            _applicationDbContext.Tbl_AirLines.Remove(employee);
            await _applicationDbContext.SaveChangesAsync();

            return Ok("Delete succesfully!!");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetApproveList()
        {
            try
            {

                var users = (from p in _userManager.Users
                             join pa in _applicationDbContext.UserRoles on p.Id equals pa.UserId
                             join pm in _applicationDbContext.Roles on pa.RoleId equals pm.Id
                             where pm.Name == "Operator"
                             select new { Id = p.Id, p.UserName, p.IsApprove, Name = pm.Name }).ToList();

                if (users == null)
                {
                    return NotFound();
                }
                return Ok(users);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateApprove(string userName)
        {
            //var user = await _userManager.FindByNameAsync(userName);
            var user = await _userManager.FindByEmailAsync(userName);
            //var userRole = await _applicationDbContext.UserRoles.Where(x=>x.RoleId==Id).FirstOrDefaultAsync();
            if (userName == null)
            {
                return Ok("Update failed!");
            }
            else
            {
                if (ModelState.IsValid && user != null)
                {
                    user.IsApprove = true ? user.IsApprove == false : user.IsApprove == true;
                    //user.IsApprove = true;

                    await _userManager.UpdateAsync(user);
                }
                return Ok("Update Successfull!!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RejectApprove(string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            if (userName == null)
            {
                return Ok("Reject failed!");
            }
            else
            {
                if (ModelState.IsValid && user != null)
                {
                    user.IsApprove = false;

                    await _userManager.UpdateAsync(user);
                }
                return Ok("Reject Successfull!!");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> HardCodeRegister([FromBody] IdentityUserProp model)
        {
            bool isSend = false;
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.EmailId);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                ApplicationUser user = new()
                {
                    Email = model.EmailId,
                    PanNo = model.PanNo,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    UserName = model.EmailId,
                    IsApprove = true
                };
                if (model.EmailId == "anil.keshari@kellton.com")
                {
                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });


                    //create role if role not exist in role table
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    }
                    //create role if role not exist in role table
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Operator))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Operator));
                    }
                    var userRoles = await _userManager.GetRolesAsync(user);
                    //assign role to user if not assigned
                    if (!await _userManager.IsInRoleAsync(user, UserRoles.Admin) || (userRoles?.Contains(UserRoles.Admin) ?? false))
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                    }
                    isSend = _mailService.EmailSend(model.EmailId);
                    return Ok(new Response { Status = "Success", Message = "User created successfully!", isMailSend = isSend });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(new Response { Status = "failed", Message = "User created failed!", isMailSend = isSend });
        }

        [Authorize(Roles = "Operator")]
        [HttpPost]
        public async Task<IActionResult> CheckAirLineName(string airLineName)
        {
            try
            {
                if (airLineName != null)
                {
                    var recordexist = await _applicationDbContext.Tbl_AirLines.Where(x => x.AirlineName == airLineName).FirstOrDefaultAsync();
                    if (recordexist != null)
                        return Ok("AirLine Name Exists");
                    else
                        return Ok("AirLine Name valid");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Ok("failed");
        }
        [Authorize(Roles = "Operator")]
        [HttpPost]
        public async Task<IActionResult> CheckUserNameExit(string userName)
        {
            try
            {
                if (userName != null)
                {
                    var userExists = await _userManager.FindByEmailAsync(userName);
                    if (userExists != null)
                        return Ok("User Name Exists");
                    else
                        return Ok("User Name valid");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Ok("failed");
        }

    }
}
