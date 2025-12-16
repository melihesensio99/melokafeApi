using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Dtos.ResponseDtos
{
    public static class ErrorCodes
    {
        public const string NOT_FOUND_STATUS = "NOT_FOUND";
        public const string UNAUTHORIZED = "UNAUTHORIZED";
        public const string EXCEPTION = "EXCEPTION";
        public const string VALIDATION_ERROR = "VALIDATION_ERROR";
        public const string CONFLICT = "CONFLICT_ERROR";
        public const string BADREQUEST = "BADREQUEST";
        public const string FORBIDDEN = "FORBIDDEN";
    }
}
