using AirLine.Model;
using Microsoft.AspNetCore.Mvc;

namespace AirLine.API.Repository
{
    public interface IUnitTestingClassRepo
    {
        Task<IdentityUserProp> Register(IdentityUserProp airlineModel);
        Task<string> Update(CreateAirlineModel updateAirline);
        Task<string> CreateSave(CreateAirlineModel post);
        Task<CreateAirlineModel> Details(long? employeeId);
    }
}
