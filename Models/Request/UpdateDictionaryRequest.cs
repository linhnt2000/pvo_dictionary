using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Request
{
    public class UpdateDictionaryRequest
    {
        public int DictionaryId { get; set; }
        public string DictionaryName { get; set; }
    }
}
