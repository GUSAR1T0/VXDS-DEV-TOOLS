using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;

namespace VXDesign.Store.DevTools.Core.Utils.Controllers
{
    public static class ApiControllerUtils
    {
        public static ObjectResult Forbidden(object value) => new ObjectResult(value)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        public static ObjectResult InternalServerError(object value) => new ObjectResult(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        public static ObjectResult HandleException(Func<ResponseResult, ObjectResult> response, OperationException exception) => response(new ResponseResult
        {
            OperationId = exception.OperationId.ToString(),
            Message = exception.Message
        });

        public static ObjectResult HandleException(Func<ResponseResult, ObjectResult> response, Exception exception) => response(new ResponseResult
        {
            OperationId = "Non-operational incident",
            Message = exception.Message
        });
    }
}