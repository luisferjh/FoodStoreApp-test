﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OnlineStore.DTOs;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Enums;
using OnlineStoreApp.UseCases.Helpers;
using OnlineStoreApp.UseCases.Interfaces;
using System.Security.Claims;

namespace OnlineStoreApp.UseCases.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWorkAdapter _unitOfWorkAdapter;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUnitOfWorkAdapter unitOfWorkAdapter,
            IOptions<KeyValuesConfiguration> keyValuesConfiguration,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWorkAdapter = unitOfWorkAdapter;
            _httpContextAccessor = httpContextAccessor;
            SecurityTools._keyValuesConfiguration = keyValuesConfiguration.Value;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var users = await _unitOfWork.UnitOfWorkRepositories.UserRepository.GetAllAsync();
            var userDtos = users.Select(s => new UserDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                State = s.State
            }).ToList();
            return userDtos;
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var user = await _unitOfWork.UnitOfWorkRepositories.UserRepository.GetAsync(id);
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                State = user.State
            };
            return userDto;
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var user = await _unitOfWork.UnitOfWorkRepositories.UserRepository.Get(email);
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                State = user.State
            };
            return userDto;
        }

        public string GetEmailUserAuth()
        {
            var usuario = _httpContextAccessor.HttpContext.User;
            var email = usuario.FindFirst(ClaimTypes.Email)?.Value;
            return email;
        }

        public async Task<ResultService> InsertAsync(UserCreateDTO model)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            try
            {
                model.Password = SecurityTools.EncryptPlainText(model.Password);

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    State = (int)RecordState.Active,
                    Password = model.Password,
                    CreatedDate = DateTime.Now
                };

                await _unitOfWork.UnitOfWorkRepositories.UserRepository.AddAsync(user);

                var result = await _unitOfWork.SaveAsync();
                List<UserClaims> claimsRoles = new List<UserClaims>
                {
                    new UserClaims
                    {
                        UserId = user.Id,
                        ClaimType = "isUser",
                        ClaimValue = "1"
                    },
                    new UserClaims
                    {
                        UserId = user.Id,
                        ClaimType = "logUser",
                        ClaimValue = "isUser"
                    },
                };
                await _unitOfWork.UnitOfWorkRepositories.UserRepository.AddUserClaimsAsync(claimsRoles);

                await _unitOfWork.SaveAsync();

                if (result)
                    return new ResultService
                    {
                        Message = "User registered successfully",
                        IsSuccess = true,
                        Data = null,
                    };
                else
                    return new ResultService
                    {
                        Message = "Error occurred",
                        IsSuccess = false,
                        Data = model,
                    };

            }
            catch (Exception ex)
            {
                return new ResultService
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    Data = model,
                };
            }
        }
    }
}
