using KafeApi.Application.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kafeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult CreateResponse<T>(ResponseDto<T> response) where T : class
        {

            if (response.Success)
            {
                return Ok(response);
            }

            return response.ErrorCode switch
            {
                ErrorCodes.NOT_FOUND_STATUS => NotFound(response),
                ErrorCodes.VALIDATION_ERROR => BadRequest(response),
                ErrorCodes.EXCEPTION => StatusCode(StatusCodes.Status500InternalServerError, response),
                ErrorCodes.UNAUTHORIZED => Unauthorized(response),
                ErrorCodes.CONFLICT => StatusCode(StatusCodes.Status409Conflict, response),
                ErrorCodes.BADREQUEST => BadRequest(response),
                ErrorCodes.FORBIDDEN => StatusCode(StatusCodes.Status403Forbidden, response),
                _ => BadRequest(response),
            };

        }

    }
}
