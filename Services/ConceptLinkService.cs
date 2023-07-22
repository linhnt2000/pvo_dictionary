using System;
using System.Linq;
using AutoMapper;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Repositories;

namespace thu6_pvo_dictionary.Services
{
    public class ConceptLinkService
    {
        private readonly ConceptLinkRepository _conceptLinkRepository;
        private readonly ApiOption _apiOption;
        private readonly IMapper _mapper;

        public ConceptLinkService(ApiOption apiOption, AppDbContext databaseContext, IMapper mapper)
        {
            _conceptLinkRepository = new ConceptLinkRepository(apiOption, databaseContext, mapper);
            _apiOption = apiOption;
            _mapper = mapper;
        }

        public object GetListConceptLink(int userId)
        {
            try
            {
                var conceptLinkList = _conceptLinkRepository.GetEntitiesByCondition(conceptLinkItem => conceptLinkItem.user_id == userId).ToList();
                var result = conceptLinkList.Select(conceptLinkItem => new
                {
                    ConceptLinkId = conceptLinkItem.concept_link_id,
                    SysConceptLinkId = conceptLinkItem.sys_concept_link_id,
                    ConceptLinkName = conceptLinkItem.concept_link_name,
                    ConceptLinkType = conceptLinkItem.concept_link_type,
                    SortOrder = conceptLinkItem.sort_order,
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
