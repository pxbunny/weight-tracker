using WeightTracker.Api.ErrorDefinitions;

namespace WeightTracker.Api.Services;

internal sealed class ErrorsService
{
    public static IResult HandleError(ErrorBase error) => error switch
    {
        BadRequestError => TypedResults.BadRequest(),
        NotFoundError => TypedResults.NotFound(),
        _ => TypedResults.InternalServerError(error.Message)
    };
}
