using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermentFactoryAPI.Services
{
    public class BusinessResult : IBusinessResult
    {
        public int Code { get; }
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public BusinessResult()
        {
            Status = -1;
            Message = "Action fail";
        }

        public BusinessResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public BusinessResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public BusinessResult(int code, string message, object data, int status)
        {
            Code = code;
            Message = message;
            Data = data;
            Status = status;
        }

    }

    public class BusinessResult<T> : BusinessResult, IBusinessResult<T>
    {
        public T? Data { get; set; }

        public BusinessResult() : base()
        {
        }

        public BusinessResult(int status, string message, T data) : base(status, message)
        {
            Data = data;
        }
    }
}
