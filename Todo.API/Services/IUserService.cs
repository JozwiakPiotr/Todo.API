using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.API.Commands;

namespace Todo.API.Services
{
    public interface IUserService
    {
        Task<Guid> Register(RegisterUser command);

        Task<string> Login(LoginUser command);
    }
}