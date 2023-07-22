
using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class Concept : BaseModel
    {
        [Key]
        public int concept_id { get; set; }
        public int dictionary_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
