using AirLine.Model;
using AirLine.Model.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using System.Web.Mvc;

namespace AirLine.API.Repository
{
    public class UnitTestingClassRepo: IUnitTestingClassRepo
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;
        private readonly ApplicationDbContext _applicationDbContext;
        public UnitTestingClassRepo(RoleManager<IdentityRole> roleManager,
           UserManager<ApplicationUser> userManager, IMailService mailService, ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mailService = mailService;
            _applicationDbContext = applicationDbContext;
        }

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

        public async Task<CreateAirlineModel> Details(long? employeeId)
        {
            if (employeeId == null)
            {
                return null;
            }
            CreateAirlineModel employee = await _applicationDbContext.Tbl_AirLines.FirstOrDefaultAsync(m => m.AirlineId == employeeId);
            if (employee == null)
            {
                return employee;
            }
            return employee;
        }

        public async Task<IdentityUserProp> Register(IdentityUserProp identityUserProp)
        {
            bool isSend = false;
            IdentityUserProp identityUser = null;
            try
            {

                var userExists = await _userManager.FindByEmailAsync(identityUserProp.EmailId);
                if (userExists != null)
                    return identityUser;

                ApplicationUser user = new()
                {
                    Email = identityUserProp.EmailId,
                    PanNo = identityUserProp.PanNo,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = String.Empty,
                    LastName = String.Empty,
                    UserName = identityUserProp.EmailId
                };
                IdentityResult result = await _userManager.CreateAsync(user, identityUserProp.Password);
                if (!result.Succeeded)
                    return identityUser;


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
                isSend = _mailService.EmailSend(identityUserProp.EmailId);
                return identityUser;
            }
            catch (Exception)
            {
                throw;
            }
            return identityUser;
        }

        public async Task<string> Update(CreateAirlineModel updateAirline)
        {
            CreateAirlineModel createAirline = new CreateAirlineModel();
            if (updateAirline == null)
            {
                return "Update failed!";
            }
            else
            {
                //if (ModelState.IsValid)
                //{
                    //createAirline.AirlineName = updateAirline.AirlineName;
                    //createAirline.FromCity = updateAirline.FromCity;
                    //createAirline.ToCity = updateAirline.ToCity;
                    //createAirline.Fare = updateAirline.Fare;
                    _applicationDbContext.Tbl_AirLines.Update(updateAirline);
                //}
                await _applicationDbContext.SaveChangesAsync();

                return "Update Successfull!!";
            }
        }
    }
}
