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

namespace thu6_pvo_dictionary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConceptLinkController : BaseApiController<ConceptLinkController>
    {
        private readonly ConceptLinkService _conceptLinkService;
        public ConceptLinkController(AppDbContext databaseContext, IMapper mapper, ApiOption apiConfig)
        {
            _conceptLinkService = new ConceptLinkService(apiConfig, databaseContext, mapper);
        }

        /// <summary>
        /// Get dictionary list by user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_list_concept_link")]
        public MessageData GetListConceptLink()
        {
            try
            {
                var res = _conceptLinkService.GetListConceptLink(UserId);
                return new MessageData { Data = res, Status = 1 };
            }
            catch (Exception ex)
            {
                return new MessageData() { Code = "Fail", Message = ex.Message, Status = 2 };
            }
        }
    }
}
