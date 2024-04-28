using Microsoft.AspNetCore.Mvc.Testing;

namespace FunctionalTests.Features.Driver.Common;

[CollectionDefinition("DriverTestsCollection")]
public class DriverTestsCollection : ICollectionFixture<WebApplicationFactory<Program>>
{
    // No code is needed here because ICollectionFixture<T> is used just to mark a class as a collection fixture.
    // This fixture will ensure that the collection is shared among test classes.
}
