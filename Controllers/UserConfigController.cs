﻿using AutoMapper;
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

namespace thu6_pvo_dictionary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserConfigController : BaseApiController<UserConfigController>
    {
        private readonly UserConfigService _userConfigService;
        public UserConfigController(AppDbContext databaseContext, IMapper mapper, ApiOption apiConfig)
        {
            _userConfigService = new UserConfigService(apiConfig, databaseContext, mapper);
        }

        /// <summary>
        /// Get dictionary list by user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_list_example_link")]
        public MessageData GetListExampleLink()
        {
            try
            {
                var res = _userConfigService.GetListExampleLink(UserId);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        [HttpGet]
        [Route("get_list_example_attribute")]
        public MessageData GetListExampleAttribute()
        {
            try
            {
                var res = _userConfigService.getListExampleAttribute(UserId);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
