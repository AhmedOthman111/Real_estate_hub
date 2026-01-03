using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Common
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Errors { get; set; }

        public ErrorResponse(int statusCode, string message, object? errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }
    }
}
