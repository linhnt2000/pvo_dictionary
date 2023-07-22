using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Request
{
    public class AddDictionaryRequest
    {
        public string DictionaryName { get; set; }
        public int? CloneDictionaryId { get; set; }
    }
}
