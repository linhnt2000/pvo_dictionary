using AutoMapper;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Entity;
using thu6_pvo_dictionary.Respositories;

namespace thu6_pvo_dictionary.Repositories
{
    public class DialectRepository : BaseRespository<Dialect>
    {
        private readonly IMapper _mapper;
        public DialectRepository(ApiOption apiConfig, AppDbContext databaseContext, IMapper mapper) : base(apiConfig, databaseContext)
        {
            _mapper = mapper;
        }
    }
}
