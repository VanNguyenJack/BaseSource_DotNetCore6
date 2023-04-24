using BaseSource.Application.Features.Vehicles.Queries;
using BaseSource.Domain.CommonFilters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

using static BaseSource.UnitTest.Testing;

namespace BaseSource.UnitTest.AdminManagerment
{
    public class GetListVehicleUnitTest 
    {
        private readonly PaginationSpecificationFilter _Filter;
        private readonly string _Route;

        public GetListVehicleUnitTest()
        {

            _Filter = new Mock<PaginationSpecificationFilter>().Object;
            _Route = "";
        }

        [Test]
        public async Task GetListVehicleHandler_Should_Return_Vehicle_List()
        {
            //// Arrange
            //var query = new GetAllVehicleQuery(_Filter, false, _Route);
            //// Act
            //var results = await SendAsync(query);
            //// Assert
            //Assert.That(results.TotalRecords, Is.EqualTo(1));
        }
    }
}
