using AutoMapper;
using AutoMapper.Configuration;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Controllers;
using thu6_pvo_dictionary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Request;

namespace thu6_pvo_dictionary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AditlogController : BaseApiController<AditlogController>
    {
        private readonly UserService _userService;
        public AditlogController(AppDbContext databaseContext, IMapper mapper, ApiOption apiConfig)
        {
            _userService = new UserService(apiConfig, databaseContext, mapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="limit"></param>
        /// <param name="searchFilter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get_logs")]
        public MessageData GetLogs(DateTime dateFrom, DateTime dateTo, int pageIndex, int pageSize, string? searchFilter)
        {
            try
            {
                var res = _userService.GetLogs(UserId, dateFrom, dateTo, pageIndex, pageSize, searchFilter);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return new MessageData() { Code = "Fail", Message = ex.Message, Status = 2 };
            }
        }

        /// <summary>
        /// Save audit log
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("save_log")]
        public MessageData SaveLog(SaveLogRequest request)
        {
            try
            {
                var res = _userService.SaveLog(UserId, request);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return new MessageData() { Code = "Fail", Message = ex.Message, Status = 2 };
            }
        }
    }
}
