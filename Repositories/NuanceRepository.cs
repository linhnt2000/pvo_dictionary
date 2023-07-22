using AutoMapper;
using thu6_pvo_dictionary.Common;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.Models.Entity;
using thu6_pvo_dictionary.Respositories;

namespace thu6_pvo_dictionary.Repositories
{
    public class NuanceRepository : BaseRespository<Nuance>
    {
        private readonly IMapper _mapper;
        public NuanceRepository(ApiOption apiConfig, AppDbContext databaseContext, IMapper mapper) : base(apiConfig, databaseContext)
        {
            _mapper = mapper;
        }
    }
}
