using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AirLine.API.Repository;
using Moq;
using Airline.UnitTest.Controller;
using AirLine.API.Controllers;
using AirLine.Model;
using Org.BouncyCastle.Utilities;
using Castle.Core.Resource;
using AirLine.Model.Data;
using Microsoft.AspNetCore.Identity;

namespace Airline.UnitTest
{

    public class AirlineUnitTest
    {
        public Mock<IAirline> mock = new Mock<IAirline>();
        public Mock<IMailService> mockemail = new Mock<IMailService>();
        
        private readonly ApplicationDbContext _applicationDbContext;
        public Mock<IUnitTestingClassRepo> mockUnit = new Mock<IUnitTestingClassRepo>();
        
        [Fact]
        public void GetAirlineList()
        {

            CreateAirlineModel expected = new CreateAirlineModel { AirlineName = "Spice", FromCity = "John", ToCity = "Varanasi", Fare = 5000 };
            List<CreateAirlineModel> createAirlineModels = new List<CreateAirlineModel>();
            createAirlineModels.Add(expected);

            mock.Setup(p => p.GetAirlineList());
            AirlineTestController airline_API = new AirlineTestController(mock.Object, _applicationDbContext);
            List<CreateAirlineModel> result = airline_API.GetAirlineList().Result;
            //Assert.Equal(createAirlineModels, result);
        }
        [Fact]
        public void CreateSave()
        {

            CreateAirlineModel expected = new CreateAirlineModel { AirlineName = "Spice", FromCity = "John", ToCity = "Varanasi", Fare = 5000 };

            mockUnit.Setup(p => p.CreateSave(expected));
            AirlineTestController airline_API = new AirlineTestController(mock.Object, _applicationDbContext);
            var result = airline_API.CreateSave(expected).Result;
            //Assert.Equal(createAirlineModels, result);
        }

        [Fact]
        public void Update()
        {

            CreateAirlineModel expected = new CreateAirlineModel { AirlineName = "Spice", FromCity = "John", ToCity = "Varanasi", Fare = 5000 };
            mockUnit.Setup(p => p.Update(expected));
            AirlineTestController airline_API = new AirlineTestController(mock.Object, _applicationDbContext);
            var result = airline_API.Update(expected).Result;
            //Assert.Equal(createAirlineModels, result);
        }

        [Fact]
        public void Details()
        {

            mockUnit.Setup(p => p.Details(1));
            AirlineTestController airline_API = new AirlineTestController(mock.Object, _applicationDbContext);
            var result = airline_API.Details(1).Result;
            //Assert.Equal(createAirlineModels, result);
        }
        [Fact]
        public void RegisterAdmin()
        {
            IdentityUserProp expected = new IdentityUserProp { EmailId = "abcds@gmail.com", PanNo = "John2345", Password = "Varanasi@123", ConfirmPassword = "Varanasi@123" };
            mockUnit.Setup(p => p.Register(expected));
            AirlineTestController airline_API = new AirlineTestController(mock.Object, _applicationDbContext);
            var result = airline_API.RegisterAdmin(expected).Result;
            //Assert.Equal(createAirlineModels, result);
        }
    }
}
