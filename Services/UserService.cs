﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Entity;
using thu6_pvo_dictionary.Models.Request;
using thu6_pvo_dictionary.Repositories;

namespace thu6_pvo_dictionary.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly AuditLogRepository _auditLogRepository;
        private readonly ApiOption _apiOption;
        private readonly IMapper _mapper;

        public UserService(ApiOption apiOption, AppDbContext databaseContext, IMapper mapper)
        {
            _userRepository = new UserRepository(apiOption, databaseContext, mapper);
            _auditLogRepository = new AuditLogRepository(apiOption, databaseContext, mapper);
            _apiOption = apiOption;
            _mapper = mapper;
        }

        /// <summary>
        /// update password
        /// </summary>
        /// <param name="updatePasswordRequest"></param>
        /// <returns></returns>
        public bool UpdatePassword(int userId, UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                var user = _userRepository.FindEntityById(userId);
                if (user == null)
                {
                    throw new Exception("User does not exist in DB!");
                }

                if (user.password != Untill.CreateMD5(updatePasswordRequest.OldPassword))
                {
                    throw new ValidateError(1000, "Incorrect email or password: Mật khẩu cũ cung cấp không đúng "); 
                }

                user.password = Untill.CreateMD5(updatePasswordRequest.NewPassword);
                _userRepository.UpdateEntity(user);
                _userRepository.SaveChange();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get logs
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="limit"></param>
        /// <param name="searchFilter"></param>
        /// <returns></returns>
        public object GetLogs(int userId, DateTime dateFrom, DateTime dateTo, int pageIndex, int limit, string? searchFilter)
        {
            try
            {
                var query = _auditLogRepository.GetAllEntities().Where(row => row.user_id == userId && row.created_date >= dateFrom && row.created_date <= dateTo);
                if(!string.IsNullOrEmpty(searchFilter))
                {
                    query = query.Where(row => row.screen_info.Contains(searchFilter) || row.reference.Contains(searchFilter) || row.description.Contains(searchFilter));
                }

                var total = query.Count();
                int tmpByInt = total / limit;
                double tmpByDouble = (double)total / (double)limit;
                int totalPage = 1;
                if (tmpByDouble > (double)tmpByInt)
                {
                    totalPage = tmpByInt + 1;
                }
                else
                {
                    totalPage = tmpByInt;
                }
                query = query.Skip((pageIndex - 1) * limit).Take(limit);
                var amount = query.Count();
                return new
                {
                    data = query.ToList(),
                    PageSize = limit,
                    TotalRecord = total,
                    TotalPage = totalPage,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object UpdateUserInfor(int userId, UpdateUserInforRequest updateUserInforRequest)
        {
            try { 
    var user = _userRepository.FindEntityById(userId);
            if (user == null)
            {
                throw new Exception("User does not exist in DB!");
            }

            user.full_name = updateUserInforRequest.FullName;
            user.display_name = updateUserInforRequest.DisplayName;
            user.birthday = updateUserInforRequest.Birthday;
            user.position = updateUserInforRequest.Position;

            // Process the Avatar
            if (updateUserInforRequest.avatar != null)
            {
                var filePath = $"/Users/nguye/Desktop/20222/PTPMHMH/pvo_dictionary/pvo-dictionary-api/Image/{userId}_{updateUserInforRequest.avatar.FileName}";
                using (var stream = System.IO.File.Create(filePath))
                {
                    updateUserInforRequest.avatar.CopyTo(stream);
                }
                user.avatar = filePath;
            }

            _userRepository.UpdateEntity(user);
            _userRepository.SaveChange();
                var data = new
                {
                    Data = user,
                    DisplayName = user.display_name,
                    Status = user.status,
                    Avatar = "file:///C:" +user.avatar,
                };
            return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Save audit log
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public object SaveLog(int userId, SaveLogRequest request)
        {
            try
            {
                var newAuditLog = new AuditLog()
                {
                    user_id = userId,
                    screen_info = request.ScreenInfo,
                    description = request.Description
                };

                if (!string.IsNullOrEmpty(request.Reference))
                {
                    newAuditLog.reference = request.Reference;
                }

                _auditLogRepository.CreateEntity(newAuditLog);
                _auditLogRepository.SaveChange();
                return newAuditLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
