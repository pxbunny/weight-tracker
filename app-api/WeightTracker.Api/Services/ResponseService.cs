using System.Net;

namespace WeightTracker.Api.Services;

internal sealed class ResponseService
{
    public static Result HandleResponse(ResponseTuple request) => request.Success
        ? Result.Success()
        : request.Code switch
        {
            HttpStatusCode.BadRequest => Errors.BadRequestError("Bad request"),
            HttpStatusCode.NotFound => Errors.NotFoundError("Data not found"),
            _ => Errors.InternalError("Internal server error")
        };
}
