using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Repositories;


namespace thu6_pvo_dictionary.Services
{
    public class UserConfigService
    {
        private readonly ToneRepository _toneRepository;
        private readonly ModeRepository _modeRepository;
        private readonly RegisterRepository _registerRepository;
        private readonly NuanceRepository _nuanceRepository;
        private readonly DialectRepository _dialectRepository;
        private readonly ExampleLinkRepository _exampleLinkRepository;
        private readonly ApiOption _apiOption;
        private readonly IMapper _mapper;

        public UserConfigService(ApiOption apiOption, AppDbContext databaseContext, IMapper mapper)
        {
            _toneRepository = new ToneRepository(apiOption, databaseContext, mapper);
            _modeRepository = new ModeRepository(apiOption, databaseContext, mapper);
            _registerRepository = new RegisterRepository(apiOption, databaseContext, mapper);
            _nuanceRepository = new NuanceRepository(apiOption, databaseContext, mapper);
            _dialectRepository = new DialectRepository(apiOption, databaseContext, mapper);
            _exampleLinkRepository = new ExampleLinkRepository(apiOption, databaseContext, mapper);
            _apiOption = apiOption;
            _mapper = mapper;
        }

        public object getListTone(int userId)
        {
            try
            {
                var ToneList = _toneRepository.GetAllEntities().Where(row => row.user_id == userId).ToList();
                return ToneList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object getListMode(int userId)
        {
            try
            {
                var modeList = _modeRepository.GetAllEntities().Where(row => row.user_id == userId).ToList();
                return modeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object getListRegister(int userId)
        {
            try
            {
                var RegistList = _registerRepository.GetAllEntities().Where(row => row.user_id == userId).ToList();
                return RegistList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object getListNunce(int userId)
        {
            try
            {
                var NunceList = _nuanceRepository.GetAllEntities().Where(row => row.user_id == userId).ToList();
                return NunceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object getListDialect(int userId)
        {
            try
            {
                var DialectList = _dialectRepository.GetAllEntities().Where(row => row.user_id == userId).ToList();
                return DialectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object getListExampleAttribute(int userId)
        {
            try
            {
                var data = new
                {
                    Tone = getListTone(userId),
                    Mode = getListMode(userId),
                    Register = getListRegister(userId),
                    Nuance = getListNunce(userId),
                    Dialect = getListDialect(userId)
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetListExampleLink(int userId)
        {
            try
            {
                var exampleLinks = _exampleLinkRepository.GetEntitiesByCondition(el => el.user_id == userId).ToList();

                var data = exampleLinks.Select(el => new
                {
                    ExampleLinkId = el.example_link_id,
                    SysExampleLinkId = el.sys_example_link_id,
                    ExampleLinkName = el.example_link_name,
                    ExampleLinkType = el.example_link_type,
                    SortOrder = el.sort_order
                }).ToList();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
