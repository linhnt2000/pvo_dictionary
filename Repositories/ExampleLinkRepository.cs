using AutoMapper;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Entity;
using thu6_pvo_dictionary.Respositories;

namespace thu6_pvo_dictionary.Repositories
{
    public class ExampleLinkRepository : BaseRespository<ExampleLink>
    {
        private readonly IMapper _mapper;
        public ExampleLinkRepository(ApiOption apiConfig, AppDbContext databaseContext, IMapper mapper) : base(apiConfig, databaseContext)
        {
            _mapper = mapper;
        }
    }
}
