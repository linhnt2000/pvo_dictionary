using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Request
{
    public class AddConceptRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DictionaryId { get; set; }
    }
}
