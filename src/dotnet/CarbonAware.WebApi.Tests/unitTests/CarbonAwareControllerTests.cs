namespace CarbonAware.WepApi.UnitTests;

using System.Collections.Generic;
using System.Net;
using CarbonAware.Model;
using CarbonAware.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

/// <summary>
/// Tests that the Web API controller handles and packages various responses from a plugin properly 
/// including empty responses and exceptions.
/// </summary>
[TestFixture]
public class CarbonAwareControllerTests : TestsBase
{
    /// <summary>
    /// Tests that successfull call to plugin with any data returned results in action with OK status.
    /// </summary>
    [Test]
    public async Task SuccessfulCallReturnsOk()
    {
        string location = "Sydney";
        var data = new List<EmissionsData>()
        {
            new EmissionsData()
            {
                Location = location,
                Rating = 0.9,
                Time = DateTime.Now
            }
        };
        var controller = new CarbonAwareController(this.MockCarbonAwareLogger.Object, CreateAggregatorWithData(data).Object);

        IActionResult ar1 = await controller.GetEmissionsDataForLocationByTime(location);
        IActionResult ar2 = await controller.GetBestEmissionsDataForLocationsByTime(new string[] { location });
        IActionResult ar3 = await controller.GetEmissionsDataForLocationsByTime(new string[] { location });
        IActionResult ar4 = await controller.GetAverageEmissionsDataForLocationByTime(location, DateTime.Now, DateTime.Now);

        TestHelpers.AssertStatusCode(ar1, HttpStatusCode.OK);
        TestHelpers.AssertStatusCode(ar2, HttpStatusCode.OK);
        TestHelpers.AssertStatusCode(ar3, HttpStatusCode.OK);
        TestHelpers.AssertStatusCode(ar4, HttpStatusCode.OK);
    }

    /// <summary>
    /// Tests that a success call to plugin with no data returned results in action with No Content status.
    /// </summary>
    [Test]
    public async Task EmptyResultRetunsNoContent()
    {
        var controller = new CarbonAwareController(this.MockCarbonAwareLogger.Object, CreateAggregatorWithData(new List<EmissionsData>()).Object);

        string location = "Sydney";
        IActionResult ar1 = await controller.GetEmissionsDataForLocationByTime(location);
        IActionResult ar2 = await controller.GetBestEmissionsDataForLocationsByTime(new string[] { location });
        IActionResult ar3 = await controller.GetEmissionsDataForLocationsByTime(new string[] { location });
        IActionResult ar4 = await controller.GetAverageEmissionsDataForLocationByTime(location, DateTime.Now, DateTime.Now);

        //Assert
        TestHelpers.AssertStatusCode(ar1, HttpStatusCode.NoContent);
        TestHelpers.AssertStatusCode(ar2, HttpStatusCode.NoContent);
        TestHelpers.AssertStatusCode(ar3, HttpStatusCode.NoContent);
        TestHelpers.AssertStatusCode(ar4, HttpStatusCode.NoContent);
    }
}
