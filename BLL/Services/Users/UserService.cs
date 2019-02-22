using BLL.Dto;
using BLL.Infastructure;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Services.Users
{
    public class UserService : IUserService
    {
        private IUnitOfWork _db;
        public UserService(IUnitOfWork db)
        {
            _db = db;
        }
        public async Task<OperationDetails> CreateAsync(UserDto userDto)
        {
            if (String.IsNullOrEmpty(userDto.UserName))
                return new OperationDetails(false, "userName can not be empty","UserName");
            if (String.IsNullOrEmpty(userDto.Password))
                return new OperationDetails(false, "password can not be empty", "Password");

            var tempUser = await _db.UserManager.FindByNameAsync(userDto.UserName);
            if(tempUser == null)
            {
                User user = new User() { UserName = userDto.UserName};
                var result = await _db.UserManager.CreateAsync(user, userDto.Password);
                if (!result.Succeeded)
                    return new OperationDetails(false,result.Errors.FirstOrDefault(),"");
                await _db.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                _db.ClientProfiles.Create(new ClientProfile() { UserName = user.UserName });

                await _db.SaveAsync();
                return new OperationDetails(true, "registration was successful", "");
            }
            return new OperationDetails(false, "user with this login busy", "UserName");
        }

        public bool IsExistsUsername(string userName)
        {
            ClientProfile user = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(userName))
                                                   .FirstOrDefault();
            return user != null ? true : false;
        }


        public async Task<ClaimsIdentity> AuthenticateAsync(UserDto userDto)
        {
            if (String.IsNullOrEmpty(userDto.UserName) || String.IsNullOrEmpty(userDto.Password))
                return null;

            ClaimsIdentity claim = null;
            User user = await _db.UserManager.FindAsync(userDto.UserName, userDto.Password);
            if(user != null)
                claim = await _db.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            return claim;   
        }

        public async Task SetInitialData(UserDto userDto, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                if(!await _db.RoleManager.RoleExistsAsync(role))
                {
                    Role newRole = new Role() { Name = role };
                    await _db.RoleManager.CreateAsync(newRole);
                }
            }
            await CreateAsync(userDto);
        }
    }
}
