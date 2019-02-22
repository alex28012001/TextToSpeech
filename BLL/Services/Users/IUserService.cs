using BLL.Dto;
using BLL.Infastructure;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Services.Users
{
    public interface IUserService
    {
        Task<OperationDetails> CreateAsync(UserDto user);
        bool IsExistsUsername(string userName);
        Task<ClaimsIdentity> AuthenticateAsync(UserDto user);
        Task SetInitialData(UserDto userDto,IEnumerable<string> roles);
    }
}
