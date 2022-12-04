using AirLine.Model;

namespace AirLine.API.Repository
{
    public interface IMailService
    {
        public bool EmailSend(string emaiId);
    }
}
