using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Dtos.ResponseDtos
{
    public class ResponseDto<T>  where T : class
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; }
        public string ErrorCode { get; set; }
    }
}
