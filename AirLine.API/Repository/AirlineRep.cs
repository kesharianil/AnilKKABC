using AirLine.Model;
using AirLine.Model.Data;
using Microsoft.EntityFrameworkCore;

namespace AirLine.API.Repository
{
    public class AirlineRep : IAirline
    {
        ApplicationDbContext _applicationDbContext;
        public AirlineRep(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext=applicationDbContext;
        }
        public async Task<List<CreateAirlineModel>> GetAirlineList()
        {
            if (_applicationDbContext != null)
            {
                return await _applicationDbContext.Tbl_AirLines.ToListAsync();
            }
            return null;
        }

        //public async Task<List<AirlineModelProp>> GetApproveList()
        //{
        //    if (_applicationDbContext != null)
        //    {
        //        return await _applicationDbContext.AspNetUserRoles.ToListAsync();
        //    }
        //    return null;
        //}
    }
}
