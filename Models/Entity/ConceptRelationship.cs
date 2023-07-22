
using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class ConceptRelationship : BaseModel
    {
        [Key]
        public int concept_relationship_id { get; set; }
        public int dictionary_id { get; set; }
        public int concept_id { get; set; }
        public int parent_id { get; set; }
        public int concept_link_id { get; set; }
    }
}
