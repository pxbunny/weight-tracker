using WeightTracker.Api.ErrorDefinitions;

namespace WeightTracker.Api.Services;

internal sealed class ErrorsService
{
    public static IResult HandleError(ErrorBase error) => error switch
    {
        BadRequestError => Results.BadRequest(),
        NotFoundError => Results.NotFound(),
        _ => Results.InternalServerError(error.Message)
    };
}
