using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermentFactoryAPI.Services
{
    public interface IBusinessResult
    {   
        int Status { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }

    }

    public interface IBusinessResult<T> : IBusinessResult
    {
        T? Data { get; }
    }

}
