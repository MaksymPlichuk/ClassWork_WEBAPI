using ClassWork_WEBAPI.BLL.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClassWork_WEBAPI.API.Extensions
{
    public static class ControllerBaseExtension
    {
        public static IActionResult GetAction(this ControllerBase controller, ServiceResponse response)
        {
            if (response.Success)
            {
                return controller.Ok(response);
            }
            else
            {
                return controller.BadRequest(response);
            }
        }
    }
}
