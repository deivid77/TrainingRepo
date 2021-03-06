using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Customer.Service.Exceptions;

namespace Customer.API.Controllers
{
    public class ApplicationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case BadRequestException e:
                    context.Result = new BadRequestObjectResult(e.Message); //Error 400
                    return;
                case ConflictException e:
                    context.Result = new ConflictObjectResult(e.Message);   //Necessary?
                    return;
                case EntityNotFoundException e:
                    context.Result = new NotFoundObjectResult(e.Message);   //Error 404
                    return;
                case ForbiddenException e:
                    context.Result = new ForbidResult();                    //Error 403
                    return;
                case UnauthorizedException e:
                    context.Result = new UnauthorizedResult();              //Error 401
                    return;
                    //TODO: Manage Internal Server error 500
            }
        }
    }
}
