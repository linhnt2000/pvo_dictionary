using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class Nuance : BaseModel
    {
        [Key]
        public int nuance_id { get; set; }
        public int sys_nuance_id { get; set; }
        public int user_id { get; set; }
        public string nuance_name { get; set; }
        public int nuance_type { get; set; }
        public int sort_order { get; set; }

    }
}
