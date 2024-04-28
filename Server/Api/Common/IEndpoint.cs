namespace Api.Common;

public interface IEndpoint
{
    abstract static void Map(IEndpointRouteBuilder app);
}

