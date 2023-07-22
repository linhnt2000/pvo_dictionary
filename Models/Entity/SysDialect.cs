
using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class SysDialect : BaseModel
    {
        [Key]
        public int sys_dialect_id { get; set; }
        public string dialect_name { get; set; }
        public int dialect_type { get; set; }
        public int sort_order { get; set; }
    }
}
