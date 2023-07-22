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
using System.ComponentModel.Design;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Request;

namespace thu6_pvo_dictionary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExampleController : BaseApiController<ExampleController>
    {
        private readonly ExampleService _exampleService;
        public ExampleController(AppDbContext databaseContext, IMapper mapper, ApiOption apiConfig)
        {
            _exampleService = new ExampleService(apiConfig, databaseContext, mapper);
        }

        /// <summary>
        /// Get dictionary list by user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("add_example")]
        public MessageData AddExample(AddExampleRequest request)
        {
            try
            {
                var res = _exampleService.AddExample(UserId, request);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        /// <summary>
        /// search example
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="toneId"></param>
        /// <param name="modeId"></param>
        /// <param name="registerId"></param>
        /// <param name="nuanceId"></param>
        /// <param name="dialectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search_example")]
        public MessageData SearchExample(string? keyword, int? toneId, int? modeId, int? registerId, int? nuanceId, int? dialectId)
        {
            try
            {
                var res = _exampleService.SearchExample(keyword, toneId, modeId, registerId, nuanceId, dialectId);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        /// <summary>
        /// Get example id
        /// </summary>
        /// <param name="exampleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get_example")]
        public MessageData GetExample(int exampleId)
        {
            try
            {
                var res = _exampleService.GetExample(exampleId);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
        [HttpGet]
        [Route("get_linked_example_by_relationship_type")]
        public MessageData getLinkedExample(int conceptId, int exampleLinkId)
        {
            try
            {
                var res = _exampleService.GetLinkedExamplesByRelationshipType(conceptId, exampleLinkId);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }


        /// <summary>
        /// update example
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update_example")]
        public MessageData UpdateExample(UpdateExampleRequest request)
        {
            try
            {
                var res = _exampleService.UpdateExample(request);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }

        /// <summary>
        /// Get example id
        /// </summary>
        /// <param name="exampleId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete_example")]
        public MessageData DeleteExample(int exampleId)
        {
            try
            {
                var res = _exampleService.DeleteExample(exampleId);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return NG(ex);
            }
        }
    }
}
