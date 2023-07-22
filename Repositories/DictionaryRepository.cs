using AutoMapper;
using thu6_pvo_dictionary.Common;

using thu6_pvo_dictionary.Respositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using thu6_pvo_dictionary.Models.Entity;
using thu6_pvo_dictionary.Models.DataContext;

namespace thu6_pvo_dictionary.Repositories
{
    public class DictionaryRepository : BaseRespository<Dictionary>
    {
        private readonly IMapper _mapper;
        public DictionaryRepository(ApiOption apiConfig, AppDbContext databaseContext, IMapper mapper) : base(apiConfig, databaseContext)
        {
            _mapper = mapper;
        }

        internal object GetById(int dictionaryId)
        {
            throw new NotImplementedException();
        }
    }
}
