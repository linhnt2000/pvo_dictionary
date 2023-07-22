using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Request
{
    public class TranferDictionaryRequest
    {
        public int SourceDictionaryId { get; set; }
        public int DestDictionaryId { get; set; }
        public bool IsDeleteDestData { get; set; }
    }
}
