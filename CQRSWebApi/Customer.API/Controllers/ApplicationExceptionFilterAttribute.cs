﻿using Microsoft.AspNetCore.Mvc;
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
                    context.Result = new BadRequestObjectResult(e.Message);
                    return;
                case ConflictException e:
                    context.Result = new ConflictObjectResult(e.Message);
                    return;
                case EntityNotFoundException e:
                    context.Result = new NotFoundObjectResult(e.Message);
                    return;
                case ForbiddenException e:
                    context.Result = new ForbidResult();
                    return;
                case UnauthorizedException e:
                    context.Result = new UnauthorizedResult();
                    return;
            }
        }
    }
}
