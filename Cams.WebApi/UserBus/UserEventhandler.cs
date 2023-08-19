using log4net;
using WorkSimple.Core.EventBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkSimple.AuthApi.Interfaces;
using WorkSimple.AuthApi.Models.User;
using Microsoft.AspNetCore.Identity;

namespace WorkSimple.Auth.UserBus
{
    public class UserEventHandler : IEventHandler<CreateUserEvent>, IEventHandler<UpdateUserEvent>
    {
        private readonly ILogger<UserEventHandler> _logger;
        private IUserRepositories _repo;
        private IPasswordHasher<UserModel> _passwordHasher;
        public UserEventHandler(ILogger<UserEventHandler> logger,
                                IUserRepositories repo,
                                IPasswordHasher<UserModel> passwordHasher)
        {
            _logger = logger;
            _repo = repo;
            _passwordHasher = passwordHasher;
        }
        public async Task Handler(CreateUserEvent eventData)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(eventData));
            var userModel = new UserModel
            {
                UserName = eventData.UserName,
                NormalizedUserName = eventData.UserName.ToUpper(),
                FirstName = eventData.FirstName,
                LastName=eventData.LastName                
            };
            var hashPassword = _passwordHasher.HashPassword(userModel, eventData.Password);
            userModel.PasswordHash = hashPassword;
            var res = _repo.Register(userModel).Result;
            if (res != null)
            {
                _logger.LogInformation("Create User Id " + userModel.UserId);
            }
            else
            {
               
            }
             await Task.FromResult(0);
        }

        public async Task Handler(UpdateUserEvent eventData)
        {
            await Task.FromResult(0);
        }

    }
}
