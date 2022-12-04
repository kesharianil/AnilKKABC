using AirLine.API.Repository;
using AirLine.Model;
using AirLine.Model.Data;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMailService = AirLine.API.Repository.IMailService;

namespace Airline.UnitTest.Controller
{
    public class AirlineTestController : ControllerBase
    {
        #region Property  
        private readonly IAirline _airline;
        private readonly ApplicationDbContext _applicationDbContext;
       
        #endregion

        #region Constructor  
        public AirlineTestController(IAirline airline, ApplicationDbContext applicationDbContext)
        {
            _airline = airline;
            _applicationDbContext = applicationDbContext;
          
        }
        #endregion

        [HttpGet(nameof(GetAirlineList))]
        public async Task<List<CreateAirlineModel>> GetAirlineList()
        {
            var result = await _airline.GetAirlineList();
            return result;
        }
        [HttpPost(nameof(CreateSave))]
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
        [HttpPost(nameof(Update))]
        public async Task<IActionResult> Update(CreateAirlineModel updateAirline)
        {
            
                return Ok("Update Successfull!!");
        }

        [HttpGet(nameof(Details))]
        public async Task<IActionResult> Details(long? employeeId)
        {
           
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin([FromBody] IdentityUserProp model)
        {
            return Ok();
        }
    }
}
