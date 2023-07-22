using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class Dialect : BaseModel
    {
        [Key]
        public int dialect_id { get; set; }
        public int sys_dialect_id { get; set; }
        public int user_id { get; set; }
        public string dialect_name { get; set; }
        public int dialect_type { get; set; }
        public int sort_order { get; set; }

    }
}
