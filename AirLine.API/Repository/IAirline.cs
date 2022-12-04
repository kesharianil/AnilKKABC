using AirLine.Model;

namespace AirLine.API.Repository
{
    public interface IAirline
    {
        Task<List<CreateAirlineModel>> GetAirlineList();
        //Task<List<CreateAirlineModel>> GetApproveList();
    }
}
